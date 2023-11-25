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

    //Instance
    private GridBuilder gridBuiler;

    // Start is called before the first frame update
    void Start()
    {
        gridBuiler = GridBuilder.instance;
        canAllowToMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!LevelCreator.instance.levelCreated) { return; }
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
            }
            StartCoroutine(ResetMove());
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

    //Reset Moving
    IEnumerator ResetMove()
    {
        yield return new WaitForSeconds(0.2f);
        canAllowToMove = true;
    }
}
