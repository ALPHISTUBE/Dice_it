using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool inHole;
    private bool inHoleR;
    private bool inHoleL;
    private bool inHoleU;
    private bool inHoleD;
    public Vector3 pos;
    private bool stopMoving;

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
        if(transform.position != pos && !stopMoving)
        {
            transform.position = Vector3.Lerp(transform.position, pos, 0.1f);
        }
        else
        {
            if (inHole)
            {
                stopMoving = true;
                if (!transform.GetComponent<Rigidbody>()) { transform.AddComponent<Rigidbody>(); }
                StartCoroutine(Disable());
            } 
        }
    }

    //Direction Movement Functions
    public void GoRight()
    {
        if (canMoveRight)
        {
            prevGridIndex = gridIndex;
            gridIndex++;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
            pos = transform.position + new Vector3(0, 0, 1);
        }

        if(inHoleR)
        {
            inHole = true;            
        }
    }
    public void GoLeft()
    {
        if (canMoveLeft)
        {
            prevGridIndex = gridIndex;
            gridIndex--;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
            pos = transform.position + new Vector3(0, 0, -1);
        }

        if (inHoleL)
        {
            inHole = true;
        }
    }
    public void GoUp()
    {
        if (canMoveUp)
        {
            prevGridIndex = gridIndex;
            gridIndex -= gridBuilder.gridSize;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
            pos = transform.position + new Vector3(-1, 0, 0);            
        }

        if (inHoleU)
        {
            inHole = true;
        }
    }
    public void GoDown()
    {
        if (canMoveDown)
        {
            prevGridIndex = gridIndex;
            gridIndex += gridBuilder.gridSize;
            currentPosition = gridBuilder.gridPoints[gridIndex].position;
            pos = transform.position + new Vector3(1, 0, 0);
        }

        if (inHoleD)
        {
            inHole = true;
        }
    }

    //Disable Object
    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
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
                        else if (pR.obj.tag == "Hole")
                        {
                            inHoleR = true;
                            canMoveRight = true; break;
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
                        else if(pL.obj.tag == "Hole")
                        {
                            inHoleL = true;
                            canMoveLeft = true; break;
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
                        else if (pU.obj.tag == "Hole")
                        {
                            inHoleU = true;
                            canMoveUp = true; break;
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
                        else if (pD.obj.tag == "Hole")
                        {
                            inHoleD = true;
                            canMoveDown = true;
                            break;
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
    public void ResetHoleInfo()
    {
        inHoleR = false;
        inHoleL = false;
        inHoleU = false;
        inHoleD = false;
    }
}
