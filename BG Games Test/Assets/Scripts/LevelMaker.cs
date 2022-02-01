using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaker : MonoBehaviour
{
    public GameObject deadCell;
    public Cell destination;
    public GameObject CellPrefab;
    public float cellSize = 5;
    public Cell[,] maze;
    // Start is called before the first frame update
    void Awake()
    {
        maze = GenLevel(10, 10);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Stack<Cell> FindShortestWay(Cell currentPos,Cell destination)
    {
        Stack<Cell> Open = new Stack<Cell>();
        List<Cell> Closed = new List<Cell>();

        currentPos.parent = null;
        Open.Push(currentPos);

        do
        {
            Open = new Stack<Cell>(Open);
            Cell workingCell = Open.Pop();
            Open = new Stack<Cell>(Open);

            Cell[] neighBours = workingCell.connections.ToArray();
            for (int n = 0; n < neighBours.Length;n++)
            {
                if (neighBours[n] == destination)
                {
                    Stack<Cell> road = new Stack<Cell>();
                    road.Push(destination);
                    if (workingCell.parent != null)
                    {
                        do
                        {
                            road.Push(workingCell);
                            workingCell = workingCell.parent;
                        } while (workingCell.parent != null);
                    }
                    road.Push(workingCell);
                    return road;
                }

                if (Closed.Contains(neighBours[n]) == false && Open.Contains(neighBours[n]) == false)
                {
                    neighBours[n].parent = workingCell;
                    Open.Push(neighBours[n]);
                }
            }
            Closed.Add(workingCell);
        } while (Open.Count > 0);

        return null;
    }
    public Cell[,] GenLevel(int sizeX, int sizeY)
    {
        Cell[,] newMaze = new Cell[sizeX, sizeY];
        for (int Y = 0; Y < sizeY; Y++)
        {
            for (int X = 0; X < sizeX; X++)
            {
                newMaze[X, Y] = new Cell();
                newMaze[X, Y].posX = X;
                newMaze[X, Y].posY = Y;
            }
        }
        //ways generator
        Stack<Cell> queue = new Stack<Cell>();

        queue.Push(newMaze[0, 0]);
        do
        {

            Cell currentCell = queue.Peek();
            currentCell.visited = true;



            Cell[] availblWArray = checkCellNeighbours(currentCell,newMaze,true);
            if (availblWArray.Length > 0)
            {
                int identif = Random.Range(0, availblWArray.Length);
                currentCell.connections.Push(availblWArray[identif]);
                availblWArray[identif].connections.Push(currentCell);
                
                queue.Push(availblWArray[identif]);
            }
            else
            {
                queue.Pop();
            }
        } while(queue.Count > 0);

        //walls Generator
        for (int Y = 0; Y < sizeY; Y++)
        {
            for (int X = 0; X < sizeX; X++)
            {
                Cell currentCell = newMaze[X, Y];
                GameObject obj = Instantiate(CellPrefab,transform);
                obj.transform.localPosition = new Vector3(X,0,Y)*cellSize;
                currentCell.cellObj = obj;
                Cell[] connectedCells = currentCell.connections.ToArray();
                for (int i = 0; i < connectedCells.Length;i++)
                {
                    if (connectedCells[i].posX < currentCell.posX)
                    {
                        Destroy(obj.transform.GetChild(0).gameObject);
                    }
                    if (connectedCells[i].posY < currentCell.posY)
                    {
                        Destroy(obj.transform.GetChild(1).gameObject);
                    }
                }
            }
        }

        //dead zones generator
        for (int i = 0; i < 10;i++)
        {
            Cell cl = newMaze[Random.Range(0, newMaze.GetLength(0)), Random.Range(0, newMaze.GetLength(0))];
            if (cl.posX + cl.posY == 0 || cl.posX + cl.posY == newMaze.GetLength(0) + newMaze.GetLength(1) - 2)
            {
                i++;
            }
            else
            {
                Cell[] neighbours = cl.connections.ToArray();
                bool enableToGen = true;
                for (int n = 0; n < neighbours.Length; n++)
                {
                    if (neighbours[n].isDangerous == true)
                    {
                        i++;
                        enableToGen = false;
                        break;
                    }
                }
                if (enableToGen == true)
                {
                    cl.isDangerous = true;
                    GameObject obj = Instantiate(deadCell, transform);
                    obj.transform.localPosition = new Vector3(cl.posX + 0.5f, 0.01f, cl.posY + 0.5f) * cellSize;
                }
            }

            
        }
        destination = newMaze[newMaze.GetLength(0) - 1, newMaze.GetLength(1) - 1];
        return newMaze;
    }
    void DebugDraw(Cell cel, Vector3 drag, Cell prevCell = null)
    {
        Cell[] connect = cel.connections.ToArray();
        cel.visited = true;
        for (int i = 0; i < connect.Length; i++)
        {
            if (connect[i] != prevCell)
            {
                if (connect[i].visited == true)
                {
                    Vector3 newDrag = drag + (new Vector3(connect[i].posX - cel.posX, 0, connect[i].posY - cel.posY) * cellSize);
                    Debug.DrawLine(drag, newDrag,Color.black);
                }
                else
                {
                    Vector3 newDrag = drag + (new Vector3(connect[i].posX - cel.posX,0, connect[i].posY - cel.posY) * cellSize);
                    Debug.DrawLine(drag, newDrag, Color.black);
                    DebugDraw(connect[i], newDrag, cel);
                }

            }
        }
    }

    Cell[] checkCellNeighbours(Cell cell,Cell[,] maze,bool waysGen = false)
    {
        int curX = cell.posX;
        int curY = cell.posY;
        Stack<Cell> availableWays = new Stack<Cell>();
        if (waysGen == true)
        {
            if (curX > 0 && maze[curX - 1, curY].visited == false)
            {
                availableWays.Push(maze[curX - 1, curY]);
            }
            if (curX < maze.GetLength(0) - 1 && maze[curX + 1, curY].visited == false)
            {
                availableWays.Push(maze[curX + 1, curY]);
            }
            if (curY < maze.GetLength(1) - 1 && maze[curX, curY + 1].visited == false)
            {
                availableWays.Push(maze[curX, curY + 1]);
            }
            if (curY > 0 && maze[curX, curY - 1].visited == false)
            {
                availableWays.Push(maze[curX, curY - 1]);
            }
        }
        else
        {
            if (curX > 0)
            {
                availableWays.Push(maze[curX - 1, curY]);
            }
            if (curX < maze.GetLength(0) - 1)
            {
                availableWays.Push(maze[curX + 1, curY]);
            }
            if (curY < maze.GetLength(1) - 1)
            {
                availableWays.Push(maze[curX, curY + 1]);
            }
            if (curY > 0)
            {
                availableWays.Push(maze[curX, curY - 1]);
            }
        }


        return availableWays.ToArray();
    }

    public class Cell
    {
        public bool isDangerous;
        public GameObject cellObj;

        public int posX;
        public int posY;

        public Stack<Cell> connections = new Stack<Cell>();

        public bool visited;

        public Cell parent;
    }
}
