using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    Player pl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            pl = col.gameObject.GetComponent<Player>();
        }

    }
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            pl.TryToDie();
        }

    }
}
