using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject pauseBut;
    public Image[] objects;

    public bool menuIsOpened;
    bool stop;
    // Start is called before the first frame update
    void Start()
    {
        closeMenu();
    }


    public void quit()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {

        if(stop == false)
        {
            if (menuIsOpened == true)
            {
                if (objects[0].color.a < 0.7f)
                {
                    objects[0].color = new Color(objects[0].color.r, objects[0].color.g, objects[0].color.b, objects[0].color.a + (Time.deltaTime * 0.5f));
                }
                for (int i = 1; i < objects.Length; i++)
                {
                    objects[i].color = new Color(objects[i].color.r, objects[i].color.g, objects[i].color.b, objects[i].color.a + (Time.deltaTime * 0.6f));
                }

            }
            else
            {
                if (objects[0].color.a > 0)
                {
                    objects[0].color = new Color(objects[0].color.r, objects[0].color.g, objects[0].color.b, objects[0].color.a - (Time.deltaTime * 4));
                }
                else
                {
                    objects[0].gameObject.SetActive(false);
                }

            }
        }


    }

    public IEnumerator Fade()
    {
        yield return new WaitForSeconds(2.75f);
        stop = true;
        objects[0].gameObject.SetActive(true);
        for (int i = 0; i < 100;i++)
        {
            objects[0].color = new Color(objects[0].color.r, objects[0].color.g, objects[0].color.b, objects[0].color.a + 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 100; i++)
        {
            objects[0].color = new Color(objects[0].color.r, objects[0].color.g, objects[0].color.b, objects[0].color.a - 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        objects[0].gameObject.SetActive(false);
        stop = false;
        yield break;
    }
    public void openMenu()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].gameObject.SetActive(true);
        }
        pauseBut.SetActive(false);
        menuIsOpened = true;
    }
    public void closeMenu()
    {
        for (int i = 1; i < objects.Length; i++)
        {
            objects[i].gameObject.SetActive(false);
        }
        pauseBut.SetActive(true);
        menuIsOpened = false;
    }
}
