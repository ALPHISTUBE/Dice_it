using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    //Caching
    private List<Transform> boxes = new List<Transform>();
    private GridBuilder.Point[] points;
    private int maxBox;
    private bool moved;

    //Instance
    private GridBuilder gridBuiler;

    // Start is called before the first frame update
    void Start()
    {
        gridBuiler = GridBuilder.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Y for left-right and X for up-down
        if (Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].GetComponent<BoxMovement>().GoRight();
            }
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].GetComponent<BoxMovement>().GoLeft();
            }
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].GetComponent<BoxMovement>().GoUp();
            }
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].GetComponent<BoxMovement>().GoDown();
            }
            moved = true;
        }

        if (moved)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                BoxMovement bm = boxes[i].GetComponent<BoxMovement>();
                points[bm.gridIndex].obj = bm.transform;
                if (points[bm.prevGridIndex].obj != null && points[bm.prevGridIndex].obj.tag == "Box" && points[bm.prevGridIndex].obj.GetComponent<BoxMovement>().id == bm.id)
                {
                    points[bm.prevGridIndex].obj = null;
                }
            }
            moved = false;
        }
    }

    //Assigning Box
    public void AssignBox(Transform[] _boxes, GridBuilder.Point[] _points)
    {
        points = _points;
        maxBox = _boxes.Length;
        foreach (Transform t in _boxes)
        {
            boxes.Add(t);
        }
    }
}
