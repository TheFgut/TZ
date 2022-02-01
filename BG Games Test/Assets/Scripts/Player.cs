using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Menu menu;
    public bool isActive;
    public GameObject Particle;
    public LineRenderer lineRend;
    public bool invincible;

    public float speed;
    public LevelMaker level;
    Stack<LevelMaker.Cell> way;
    Stack<LevelMaker.Cell> startWay;

    internal MeshRenderer rend;
    bool enableMove;
    bool allowedMove = true;

    bool Win;
    // Start is called before the first frame update
    void Start()
    {

        way = level.FindShortestWay(level.maze[0, 0], level.destination);
        startWay = new Stack<LevelMaker.Cell>(way);
        rend = GetComponent<MeshRenderer>();
        lineRend.SetPositions(drawWay(way.ToArray()));
        StartCoroutine(startTimer());
    }
    IEnumerator startTimer()
    {

        yield return new WaitForSeconds(2);
        Win = false;
        way = level.FindShortestWay(level.maze[0, 0], level.destination);
        startWay = new Stack<LevelMaker.Cell>(way);
        lineRend.SetPositions(drawWay(way.ToArray()));
        enableMove = true;
        Debug.Log("Done");
        yield break;
    }
    // Update is called once per frame
    void Update()
    {
        if (way.Count > 0)
        {
            if (enableMove && allowedMove)
            {
                LevelMaker.Cell cur = way.Peek();
                if ((transform.position - (new Vector3(cur.posX + 0.5f, 0, cur.posY + 0.5f) * level.cellSize + new Vector3(0, 1))).magnitude < 0.1f)
                {
                    way.Pop();
                    lineRend.SetPositions(drawWay(way.ToArray()));
                }
                else
                {
                    lineRend.SetPosition(0, transform.position);
                    transform.position -= (transform.position - (new Vector3(cur.posX + 0.5f, 0, cur.posY + 0.5f) * level.cellSize + new Vector3(0, 1))).normalized * speed * Time.deltaTime;
                }
            }
        }
        else if (Win == false)
        {
            Win = true;
            StartCoroutine(win());
            StartCoroutine(menu.Fade());
        }
    }
    public void NewGame()
    {
        for (int i = 0; i < level.gameObject.transform.childCount;i++)
        {
            Destroy(level.gameObject.transform.GetChild(i).gameObject);
        }
        transform.position = new Vector3(0.5f,0,0.5f) * level.cellSize + new Vector3(0, 1, 0);
        level.maze = level.GenLevel(10, 10);
        StartCoroutine(startTimer());
    }
    IEnumerator win()
    {
        int counter = 0;
        
        do
        {
            yield return new WaitForSeconds(0.15f);
            GameObject partcl = Instantiate(Particle, transform.position, Quaternion.identity);
            partcl.GetComponent<Particle>().GenColor();
            counter++;
        } while (counter < 25);
        enableMove = false;
        NewGame();

        yield break;
    }

    public void TryToDie()
    {
        if(invincible == false)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(Particle,transform.position,Quaternion.identity);
            }
            way = new Stack<LevelMaker.Cell>(startWay);
            transform.position = new Vector3(0.5f, 0, 0.5f) * level.cellSize;
            drawWay(way.ToArray());
        }
    }
    public Vector3[] drawWay(LevelMaker.Cell[] road)
    {
        Vector3[] roadPoints = new Vector3[road.Length];
        for (int i = 0; i < road.Length; i++)
        {
            roadPoints[i] = new Vector3(road[i].posX + 0.5f, 0, road[i].posY + 0.5f) * level.cellSize;
        }
        return roadPoints;
    }

    public void ActivateShield()
    {
        rend.material.color = new Color(0.6784314f, 1, 0.1843137f);
    }

    public void SetEnableMove()
    {
        if (allowedMove == true)
        {
            allowedMove = false;
        }
        else
        {
            allowedMove = true;
        }
    }
}
