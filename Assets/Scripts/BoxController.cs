using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    //Caching
    private List<Transform> boxes = new List<Transform>();
    private GridBuilder.Point[] points;
    private bool moved;
    public List<BreakableObstacle> breakableObstacles;
    private bool canAllowToMove;
    private bool playerCanControl;

    //Instance
    private GridBuilder gridBuiler;

    // Start is called before the first frame update
    void Start()
    {
        gridBuiler = GridBuilder.instance;
        canAllowToMove = true;
        playerCanControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!LevelCreator.instance.levelCreated && !playerCanControl) { return; }
        //Y for left-right and X for up-down
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (canAllowToMove)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    boxes[i].GetComponent<BoxMovement>().GoRight();
                }
            }
            canAllowToMove = false;
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (canAllowToMove)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    boxes[i].GetComponent<BoxMovement>().GoLeft();
                }
            }
            canAllowToMove = false;
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (canAllowToMove)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    boxes[i].GetComponent<BoxMovement>().GoUp();
                }
            }
            canAllowToMove = false;
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (canAllowToMove)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    boxes[i].GetComponent<BoxMovement>().GoDown();
                }
            }
            canAllowToMove = false;
            moved = true;
        }

        if (moved)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                BoxMovement bm = boxes[i].GetComponent<BoxMovement>();
                bm.ResetHoleInfo();
                if(!points[bm.gridIndex].hasHole) { points[bm.gridIndex].obj = bm.transform; }
                if (points[bm.prevGridIndex].obj != null && points[bm.prevGridIndex].obj.tag == "Box" && points[bm.prevGridIndex].obj.GetComponent<BoxMovement>().id == bm.id)
                {
                    points[bm.prevGridIndex].obj = null;
                }
            }

            foreach (BreakableObstacle o in breakableObstacles)
            {
                o.maxMove--;
                o.PlayAnimation();
            }
            StartCoroutine(ResetPermissionToMove());
            moved = false;
        }
    }

    //Assigning Box
    public void AssignBox(Transform[] _boxes, GridBuilder.Point[] _points)
    {
        points = _points;
        foreach (Transform t in _boxes)
        {
            boxes.Add(t);
        }
    }

    //Reset Moving permission
    IEnumerator ResetPermissionToMove()
    {
        yield return new WaitForSeconds(0.2f);
        canAllowToMove = true;
    }

    //Skip Level
    public IEnumerator SkipLevel()
    {
        playerCanControl = false;
        ResetLevel();
        List<int> playerMoves = LevelCreator.instance.moves;

        //Move boxes to backward
        for (int i = 0; i < playerMoves.Count; i++)
        {
            if (playerMoves[i] == 0) //Move to Right
            {
                for (int j = 0; j < boxes.Count; j++)
                {
                    boxes[j].GetComponent<BoxMovement>().GoRight();
                }
            }
            else if (playerMoves[i] == 1) //Move to Down
            {
                for (int j = 0; j < boxes.Count; j++)
                {
                    boxes[j].GetComponent<BoxMovement>().GoDown();
                }
            }
            else if (playerMoves[i] == 2) //Move to Left
            {
                for (int j = 0; j < boxes.Count; j++)
                {
                    boxes[j].GetComponent<BoxMovement>().GoLeft();
                }
            }
            else if (playerMoves[i] == 3) //Move to Up
            {
                for (int j = 0; j < boxes.Count; j++)
                {
                    boxes[j].GetComponent<BoxMovement>().GoUp();
                }
            }

            for (int j = 0; j < boxes.Count; j++)
            {
                BoxMovement bm = boxes[j].GetComponent<BoxMovement>();
                bm.ResetHoleInfo();
                if (!points[bm.gridIndex].hasHole) { points[bm.gridIndex].obj = bm.transform; }
                if (points[bm.prevGridIndex].obj != null && points[bm.prevGridIndex].obj.tag == "Box" && points[bm.prevGridIndex].obj.GetComponent<BoxMovement>().id == bm.id)
                {
                    points[bm.prevGridIndex].obj = null;
                }
            }

            foreach (BreakableObstacle o in breakableObstacles)
            {
                o.maxMove--;
                o.PlayAnimation();
            }
            StartCoroutine(ResetPermissionToMove());

            yield return new WaitForSeconds(0.2f);
        }
    }

    //Reset Level
    public void ResetLevel()
    {
        playerCanControl = false;

        for (int i = 0; i < boxes.Count; i++)
        {
            BoxMovement bm = boxes[i].GetComponent<BoxMovement>();
            bm.ResetGridIndex();
            bm.ResetBoxPosition();
        }

        for (int i = 0; i < breakableObstacles.Count; i++)
        {
            breakableObstacles[i].ResetBreakableObstacle();
        }
        playerCanControl = true;
    }
}
