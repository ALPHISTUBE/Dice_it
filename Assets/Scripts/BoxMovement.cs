using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    public int id;
    //Caching
    public int gridIndex;
    public bool canMoveRight;
    public bool canMoveLeft;
    public bool canMoveUp;
    public bool canMoveDown;
    public Vector2 currentPosition;
    public int prevGridIndex;

    //Instance
    [HideInInspector]public GridBuilder gridBuilder;

    // Start is called before the first frame update
    void Start()
    {
        id = GetInstanceID();    
    }

    // Update is called once per frame
    void Update()
    {
        CheckPossibleDirection();
    }

    //Direction Movement Functions
    public void GoRight()
    {
        if (canMoveRight)
        {
            prevGridIndex = gridIndex;
            gridIndex++;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
            transform.position += new Vector3(0, 0, 1);
        }
    }
    public void GoLeft()
    {
        if (canMoveLeft)
        {
            prevGridIndex = gridIndex;
            gridIndex--;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
            transform.position += new Vector3(0, 0, -1);
        }
    }
    public void GoUp()
    {
        if (canMoveUp)
        {
            prevGridIndex = gridIndex;
            gridIndex -= gridBuilder.gridSize;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
            transform.position += new Vector3(-1, 0, 0);
        }
    }
    public void GoDown()
    {
        if (canMoveDown)
        {
            prevGridIndex = gridIndex;
            gridIndex += gridBuilder.gridSize;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
            transform.position += new Vector3(1, 0, 0);
        }
    }


    //Checking for possible direction for box to move
    public void CheckPossibleDirection()
    {
        if (currentPosition.y < gridBuilder.offsetPosition) //Right
        {
            for (int i = 1; i < gridBuilder.gridSize - 1; i++)
            {
                GridBuilder.Point pR = gridBuilder.gridPoints[gridIndex + i];
                if (pR.isValid)
                {
                    if (pR.obj != null)
                    {
                        if(pR.obj.tag == "Obstacle")
                        {
                            canMoveRight = false; break;
                        }
                        else if (pR.obj.tag == "Box")
                        {
                            if (pR.obj.GetComponent<BoxMovement>().canMoveRight) { canMoveRight = true; } else { canMoveRight = false; }
                        }
                    }
                    else
                    {
                        canMoveRight = true; break;
                    }
                }
                else
                {
                    canMoveRight = false; break;
                }
            }
            
        }
        else
        {
            canMoveRight= false;
        }

        if (currentPosition.y > -gridBuilder.offsetPosition) //Left
        {            
            for (int i = 1; i < gridBuilder.gridSize - 1; i++)
            {
                GridBuilder.Point pL = gridBuilder.gridPoints[gridIndex - 1];
                if (pL.isValid)
                {
                    if (pL.obj != null)
                    {
                        if (pL.obj.tag == "Obstacle")
                        {
                            canMoveLeft = false; break;
                        }
                        else if (pL.obj.tag == "Box")
                        {
                            if (pL.obj.GetComponent<BoxMovement>().canMoveLeft) { canMoveLeft = true; } else { canMoveLeft = false; }
                        }
                    }
                    else
                    {
                        canMoveLeft = true; break;
                    }
                }
                else
                {
                    canMoveLeft = false; break;
                }
            }

        }
        else
        {
            canMoveLeft = false;
        }

        if (currentPosition.x > -gridBuilder.offsetPosition) //UP
        {
            for (int i = 1; i < gridBuilder.gridSize - 1; i++)
            {
                GridBuilder.Point pU = gridBuilder.gridPoints[gridIndex - gridBuilder.gridSize];
                if (pU.isValid)
                {
                    if (pU.obj != null)
                    {
                        if (pU.obj.tag == "Obstacle")
                        {
                            canMoveUp = false; break;
                        }
                        else if (pU.obj.tag == "Box")
                        {
                            if (pU.obj.GetComponent<BoxMovement>().canMoveUp) { canMoveUp = true; } else { canMoveUp= false; }
                        }
                    }
                    else
                    {
                        canMoveUp = true; break;
                    }
                }
                else
                {
                    canMoveUp = false; break;
                }
            }

        }
        else
        {
            canMoveUp = false;
        }

        if (currentPosition.x < gridBuilder.offsetPosition)
        {
            for (int i = 1; i < gridBuilder.gridSize - 1; i++)
            {
                GridBuilder.Point pD = gridBuilder.gridPoints[gridIndex + gridBuilder.gridSize];
                if (pD.isValid)
                {
                    if (pD.obj != null)
                    {
                        if (pD.obj.tag == "Obstacle")
                        {
                            canMoveDown = false; break;
                        }
                        else if (pD.obj.tag == "Box")
                        {
                            if (pD.obj.GetComponent<BoxMovement>().canMoveDown) { canMoveDown = true; } else { canMoveDown = false; }
                        }
                    }
                    else
                    {
                        canMoveDown = true; break;
                    }
                }
                else
                {
                    canMoveDown = false; break;
                }
            }

        }
        else
        {
            canMoveDown = false;
        }
    }
}
