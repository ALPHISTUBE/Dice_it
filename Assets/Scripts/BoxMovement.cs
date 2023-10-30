using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    //Caching
    public int gridIndex;
    public bool canMoveRight;
    public bool canMoveLeft;
    public bool canMoveUp;
    public bool canMoveDown;
    public Vector2 currentPosition;

    //Instance
    [HideInInspector]public GridBuilder gridBuilder;

    // Start is called before the first frame update
    void Start()
    {
        //gridBuilder = GridBuilder.instance;       
    }

    // Update is called once per frame
    void Update()
    {
        CheckPossibleDirection();
        //Y for left-right and X for up-down
        if (Input.GetKeyDown(KeyCode.D))
        {
            GoRight();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            GoLeft();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            GoUp();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            GoDown();
        }
    }

    //Direction Movement Functions
    public void GoRight()
    {
        if (canMoveRight)
        {
            transform.position += new Vector3(0, 0, 1);
            gridBuilder.gridPoints[gridIndex].obj = null;
            gridBuilder.gridPoints[gridIndex + 1].obj = this.transform;
            gridIndex++;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
        }
    }
    public void GoLeft()
    {
        if (canMoveLeft)
        {
            transform.position += new Vector3(0, 0, -1);
            gridBuilder.gridPoints[gridIndex].obj = null;
            gridBuilder.gridPoints[gridIndex - 1].obj = this.transform;
            gridIndex--;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
        }
    }
    public void GoUp()
    {
        if (canMoveUp)
        {
            transform.position += new Vector3(-1, 0, 0);
            gridBuilder.gridPoints[gridIndex].obj = null;
            gridBuilder.gridPoints[gridIndex - gridBuilder.gridSize].obj = this.transform;
            gridIndex -= gridBuilder.gridSize;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
        }
    }
    public void GoDown()
    {
        if (canMoveDown)
        {
            transform.position += new Vector3(1, 0, 0);
            gridBuilder.gridPoints[gridIndex].obj = null;
            gridBuilder.gridPoints[gridIndex + gridBuilder.gridSize].obj = this.transform;
            gridIndex += gridBuilder.gridSize;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
        }
    }


    //Checking for possible direction for box to move
    public void CheckPossibleDirection()
    {
        if (currentPosition.y < gridBuilder.offsetPosition) //Right
        {
            GridBuilder.Point pR = gridBuilder.gridPoints[gridIndex + 1];
            if (pR.isValid)
            {
                if (pR.obj == null)
                {
                    canMoveRight = true;
                }
                else
                {
                    if(pR.obj.tag == "Box")
                    {
                        canMoveRight = pR.obj.GetComponent<BoxMovement>().canMoveRight;
                    }
                    else
                    {
                        canMoveRight = false;
                    }
                }
            }
            else
            {
                canMoveRight = false;
            }
        }
        else
        {
            canMoveRight= false;
        }

        if (currentPosition.y > -gridBuilder.offsetPosition) //Left
        {            
            GridBuilder.Point pL = gridBuilder.gridPoints[gridIndex - 1];
            if (pL.isValid)
            {
                if (pL.obj == null)
                {
                    canMoveLeft = true;
                }
                else
                {
                    if (pL.obj.tag == "Box")
                    {
                        canMoveLeft = pL.obj.GetComponent<BoxMovement>().canMoveLeft;
                    }
                    else
                    {
                        canMoveLeft = false;
                    }
                }
            }
            else
            {
                canMoveLeft = false;
            }
        }
        else
        {
            canMoveLeft = false;
        }

        if (currentPosition.x > -gridBuilder.offsetPosition)
        {
            GridBuilder.Point pU = gridBuilder.gridPoints[gridIndex - gridBuilder.gridSize];
            if (pU.isValid)
            {
                if (pU.obj == null)
                {
                    canMoveUp = true;
                }
                else
                {
                    if (pU.obj.tag == "Box")
                    {
                        canMoveUp = pU.obj.GetComponent<BoxMovement>().canMoveUp;
                    }
                    else
                    {
                        canMoveUp = false;
                    }
                }
            }
            else
            {
                canMoveUp = false;
            }
        }
        else
        {
            canMoveUp = false;
        }

        if (currentPosition.x < gridBuilder.offsetPosition)
        {
            GridBuilder.Point pD = gridBuilder.gridPoints[gridIndex + gridBuilder.gridSize];
            if (pD.isValid)
            {
                if (pD.obj == null)
                {
                    canMoveDown = true;
                }
                else
                {
                    if (pD.obj.tag == "Box")
                    {
                        canMoveDown = pD.obj.GetComponent<BoxMovement>().canMoveDown;
                    }
                    else
                    {
                        canMoveDown = false;
                    }
                }
            }
            else
            {
                canMoveDown = false;
            }
        }
        else
        {
            canMoveDown = false;
        }
    }
}
