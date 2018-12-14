using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//uses breadth first search to locate the easiest path of movement to a location(using tiles)
public class TacticsMove : MonoBehaviour
{
    public bool turn = false;
    List<tile> selectableTiles = new List<tile>();
    GameObject[] tiles;
    GameObject[] enemies;
    Stack<tile> path = new Stack<tile>();
    tile currentTile;
    public int move = 4;
    public float JumpHeight = 1;
    public float movespeed = 2;
    public float jumpvelocity = 4.5f;
    public bool moving = false;
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();
    float halfHeight = 0;
    bool FallingDown = false;
    bool JumpingUp = false;
    bool MovingEdge = false;
    Vector3 jumpTarget;
    public tile actualtargettile;

    public GameObject myhitbox;



    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("tile");
        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }


    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }
    public tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        tile tile = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<tile>();
        }
        return tile;

    }

    public void ComputeAdjacencyLists(float jumpHeight, tile target)
    {
        foreach (GameObject tile in tiles)
        {
            tile t = tile.GetComponent<tile>();
            t.FindNeighbors(JumpHeight, target);
        }
    }


    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists(JumpHeight, null);
        GetCurrentTile();
        Queue<tile> process = new Queue<tile>();
        process.Enqueue(currentTile);
        currentTile.visited = true;


        while (process.Count > 0)
        {
            tile t = process.Dequeue();
            selectableTiles.Add(t);
            t.selectable = true;
            if (t.distance < move)
            {
                foreach (tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MovetoTile(tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            tile t = path.Peek();
            Vector3 target = t.transform.position;
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;
            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jump = transform.position.y != target.y;
                if (jump)
                {
                    Jump(target);

                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;

            }
            else
            {
                transform.position = target;
                path.Pop();

            }
        }

        else
        {

            RemoveSelectableTiles();
            moving = false;

            if (moving == false)
            {
                TurnManager.EndTurn();

            }


        }


    }


    protected void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        foreach (tile tile in selectableTiles)
        {
            tile.Reset();
        }
        selectableTiles.Clear();
    }
    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }
    void SetHorizontalVelocity()
    {
        velocity = heading * movespeed;
    }
    void Jump(Vector3 target)   //allows a unit to move to a tile higher than it's current position
    {
        if (FallingDown)
        {
            FallDownward(target);
        }
        else if (JumpingUp)
        {
            JumpUpward(target);
        }
        else if (MovingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }

    }

    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;
        CalculateHeading(target);
        if (transform.position.y > targetY)
        {
            FallingDown = false;
            JumpingUp = false;
            MovingEdge = true;
            jumpTarget = transform.position + (target - transform.position) / 2.0f;

        }
        else
        {
            FallingDown = false;
            JumpingUp = true;
            MovingEdge = false;
            velocity = heading * movespeed / 3.0f;
            float difference = targetY - transform.position.y;
            velocity.y = jumpvelocity * (0.5f + difference / 2.0f);
        }
    }
    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;
        if (transform.position.y <= target.y)
        {
            FallingDown = false;
            JumpingUp = false;
            MovingEdge = false;
            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;
            velocity = new Vector3();

        }
    }
    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            JumpingUp = false;
            FallingDown = true;
        }
    }
    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();

        }
        else
        {
            MovingEdge = false;
            FallingDown = true;

            velocity /= 5.0f;
            velocity.y = 1.5f;
        }


    }
    protected tile FindLowestF(List<tile> list)
    {
        tile lowest = list[0];
        foreach (tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }
        list.Remove(lowest);
        return lowest;
    }

    protected tile findendtile(tile t)
    {
        Stack<tile> tempPath = new Stack<tile>();
        tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;

        }
        if (tempPath.Count <= move)
        {
            return t.parent;
        }
        tile endtile = null;
        for (int i = 0; i < move; i++)
        {
            endtile = tempPath.Pop();
        }
        return endtile;
    }
    protected void FindPath(tile target)
    {
        ComputeAdjacencyLists(JumpHeight, target);
        GetCurrentTile();
        List<tile> openList = new List<tile>();
        List<tile> closedList = new List<tile>();
        openList.Add(currentTile);
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;
        while (openList.Count > 0)
        {
            tile t = FindLowestF(openList);
            closedList.Add(t);
            if (t == target)
            {
                actualtargettile = findendtile(t);
                MovetoTile(actualtargettile);
                return;
            }
            foreach (tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                {

                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    if (tempG < tile.g)
                    {
                        tile.parent = t;
                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;
                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;
                    openList.Add(tile);


                }
            }
        }
        // problem with no target tile
        Debug.Log("Path not found");
    }
    public void BeginTurn()
    {
        turn = true;
    }
    public void EndTurn()
    {
        turn = false;
    }
}
