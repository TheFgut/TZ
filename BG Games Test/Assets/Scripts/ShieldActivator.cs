using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShieldActivator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Player player;
    float shieldTimer = 2;

    public bool isPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetActive()
    {
        if (gameObject.active == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isPressed == true)
        {
            if (shieldTimer > 0)
            {
                shieldTimer -= Time.deltaTime;
                player.invincible = true;
                player.rend.material.color = new Color(0.6784314f, 1, 0.1843137f);
            }
            else
            {
                player.invincible = false;
                player.rend.material.color = new Color(1, 1, 0);
            }
        } 
    }

    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
    }
    public void OnPointerUp(PointerEventData data)
    {
        shieldTimer = 2;
        player.invincible = false;
        player.rend.material.color = new Color(1, 1, 0);
        isPressed = false;
    }
}
