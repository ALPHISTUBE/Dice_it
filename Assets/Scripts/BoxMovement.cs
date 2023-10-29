using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    //Caching
    [HideInInspector] public int gridIndex;

    //Instance
    private GridBuilder gridBuilder;

    // Start is called before the first frame update
    void Start()
    {
        gridBuilder = GridBuilder.instance;    
    }

    // Update is called once per frame
    void Update()
    {
        GridBuilder.Point p = gridBuilder.gridPoints[gridIndex];

        //Y for left-right and X for up-down
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (p.position.y < gridBuilder.offsetPosition)
            {
                GridBuilder.Point pR = gridBuilder.gridPoints[gridIndex + 1];
                if(pR.isValid)
                {
                    transform.position = new Vector3(0, transform.position.z, 1);
                    gridBuilder.gridPoints[gridIndex].obj = null;
                    gridBuilder.gridPoints[gridIndex + 1].obj = this.transform;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (p.position.y > -gridBuilder.offsetPosition)
            {
                GridBuilder.Point pL = gridBuilder.gridPoints[gridIndex - 1];
                if (pL.isValid)
                {
                    transform.position = new Vector3(0, transform.position.z, -1);
                    gridBuilder.gridPoints[gridIndex].obj = null;
                    gridBuilder.gridPoints[gridIndex - 1].obj = this.transform;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (p.position.x > -gridBuilder.offsetPosition)
            {
                GridBuilder.Point pU = gridBuilder.gridPoints[gridIndex - gridBuilder.gridSize];
                if (pU.isValid)
                {
                    transform.position = new Vector3(-1, transform.position.z, 0);
                    gridBuilder.gridPoints[gridIndex].obj = null;
                    gridBuilder.gridPoints[gridIndex - gridBuilder.gridSize].obj = this.transform;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (p.position.x < gridBuilder.offsetPosition)
            {
                GridBuilder.Point pD = gridBuilder.gridPoints[gridIndex + gridBuilder.gridSize];
                if (pD.isValid)
                {
                    transform.position = new Vector3(1, transform.position.z, 0);
                    gridBuilder.gridPoints[gridIndex].obj = null;
                    gridBuilder.gridPoints[gridIndex + gridBuilder.gridSize].obj = this.transform;
                }
            }
        }
    }
}
