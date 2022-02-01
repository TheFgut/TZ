using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    float timer;
    Vector3 flyVect;
    float flySpeed;
    Vector3 rotation;
    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(0.3f, 0.6f);
        flyVect = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
        rotation = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
        flySpeed = Random.Range(10, 15);
        transform.localScale = new Vector3(1,1,1)* Random.Range(0.2f, 0.5f);
        
    }
    public void GenColor()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material.color = new Color(Random.Range(0,1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }
    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
        {
            Destroy(gameObject);
        }
        timer -= Time.deltaTime;
        transform.position += flyVect * Time.deltaTime * flySpeed;
        transform.Rotate(rotation * Time.deltaTime);
    }
}
