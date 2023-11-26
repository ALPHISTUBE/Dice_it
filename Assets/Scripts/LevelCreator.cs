using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public BoxController bc;
    public bool levelCreated;
    public int maxMove;
    private int level;
    private int levelDifficulty;
    public List<int> moves;
    private int prevMove;

    //Caching
    private Transform[] boxes;
    private GridBuilder.Point[] points;
    private Transform[] flags;
    private List<int> holes = new List<int>();

    //Instance
    private GridBuilder gb;
    public static LevelCreator instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gb = GridBuilder.instance;
    }

    public IEnumerator CreateLevel()
    {
        for (int i = 0; i < maxMove; i++)
        {
            bool moveHasBeenSet = false;
            while(!moveHasBeenSet)
            {
                int nextMove = Random.Range(0, 4);
                bool success = true;

                for (int j = 0; j < boxes.Length; j++)
                {
                    BoxMovement bm = boxes[j].GetComponent<BoxMovement>();
                    if (bm.dirHoles[nextMove]) { print(bm.dirHoles[nextMove]); success = false; break; }
                }                

                if (success)
                {
                    if (nextMove == 0)
                    {
                        for (int j = 0; j < boxes.Length; j++)
                        {
                            boxes[j].GetComponent<BoxMovement>().GoRight();
                        }
                    }
                    else if (nextMove == 1)
                    {
                        for (int j = 0; j < boxes.Length; j++)
                        {
                            boxes[j].GetComponent<BoxMovement>().GoDown();
                        }
                    }
                    else if (nextMove == 2)
                    {
                        for (int j = 0; j < boxes.Length; j++)
                        {
                            boxes[j].GetComponent<BoxMovement>().GoLeft();
                        }
                    }
                    else if (nextMove == 3)
                    {
                        for (int j = 0; j < boxes.Length; j++)
                        {
                            boxes[j].GetComponent<BoxMovement>().GoUp();
                        }
                    }
                    moves.Add(nextMove);
                    moveHasBeenSet = true;
                }
            }            

            for (int j = 0; j < boxes.Length; j++)
            {
                BoxMovement bm = boxes[j].GetComponent<BoxMovement>();
                bm.ResetHoleInfo();
                if (!points[bm.gridIndex].hasHole) { points[bm.gridIndex].obj = bm.transform; }
                if (points[bm.prevGridIndex].obj != null && points[bm.prevGridIndex].obj.tag == "Box" && points[bm.prevGridIndex].obj.GetComponent<BoxMovement>().id == bm.id)
                {
                    points[bm.prevGridIndex].obj = null;
                }
            }


            foreach (BreakableObstacle o in bc.breakableObstacles)
            {
                o.maxMove--;
            }
            yield return new WaitForSeconds(0.3f);
        }

        for (int j = 0; j < boxes.Length; j++)
        {
            BoxMovement bm = boxes[j].GetComponent<BoxMovement>();
            flags[j].position = new Vector3(points[bm.gridIndex].position.x, -0.48f, points[bm.gridIndex].position.y);
            //print($"{bm.GetStartGridIndex()},{bm.gridIndex}, {points[bm.gridIndex].position}, {flags[j].position}");
            bm.ResetGridIndex();
        }

        foreach (BreakableObstacle o in bc.breakableObstacles)
        {
            o.gameObject.SetActive(true);
            o.maxMove = o.GetStartMaxMove();
            gb.gridPoints[o.gridIndex].obj = o.transform;
            gb.gridPoints[o.gridIndex].isValid = false;
        }

        foreach(int i in holes)
        {
            //gb.gridPoints[i].isValid = true;
        }

        levelCreated = true;
    }

    //Assigning Box
    public void AssignBox(Transform[] _boxes, GridBuilder.Point[] _points, Transform[] _flags, List<int> _holes)
    {
        points = _points;
        flags = _flags;
        boxes = _boxes;
        holes = _holes;
    }
}
