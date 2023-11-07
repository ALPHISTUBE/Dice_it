using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    //Settings
    [Header("Settings")]
    [Range(5, 10)]
    public int gridSize = 3;
    [Range(1, 10)]
    public int maxFlags;
    [Range(0, 0.5f)]
    public float maxObstaclePercentage = 0.2f;
    public BoxController boxController;
    public bool gridBuilded;

    //Prefabs
    [Header("Prefabs")]
    public Transform obstacle;
    public Transform breakableObstacle;
    public Transform box;
    public Transform flag;
    public Transform floor;
    public Transform hole;

    //Caching
    public Point[] gridPoints;
    [HideInInspector] public float offsetPosition;
    private Transform[] boxes;
    private int maxObstacle;
    private Transform[] flags;

    //Parents
    [Header("Parents")]
    public Transform obstacleParent;
    public Transform boxParent;
    public Transform flagParent;
    public Transform floorParent;
    public Transform holeParent;

    //Instance
    public static GridBuilder instance;

    private void Awake()
    {
        instance = this;   
    }


    void Start()
    {
        gridSize++;
        SetLevel();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    //Set Level Object
    public void SetLevel()
    {
        //Setting up Level Properties
        boxes = new Transform[maxFlags];
        flags = new Transform[maxFlags];
        maxObstacle = (int)((gridSize - 2) * (gridSize - 2) * Random.Range(0.3f, maxObstaclePercentage));


        //Creating Grid Points
        gridPoints = new Point[gridSize * gridSize];
        offsetPosition = ((float)gridSize - 1) / 2;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                gridPoints[i * gridSize + j].position = new Vector2(i - offsetPosition, j - offsetPosition);
            }
        }

        //Setting up Border
        //1st border
        for (int i = 0; i < gridSize; i++)
        {
            Transform obstacles = Instantiate(obstacle, obstacleParent);
            obstacles.position = new Vector3(gridPoints[i].position.x, -0.35f, gridPoints[i].position.y);
            gridPoints[i].obj = obstacles;
        }
        //Middle border
        for (int i = gridSize; i < (gridSize * (gridSize - 2)) + 1; i += gridSize)
        {
            Transform obstacles = Instantiate(obstacle, obstacleParent);
            obstacles.position = new Vector3(gridPoints[i].position.x, -0.35f, gridPoints[i].position.y);
            gridPoints[i].obj = obstacles;

            int _i = i + (gridSize - 1);
            Transform obstacles1 = Instantiate(obstacle, obstacleParent);
            obstacles1.position = new Vector3(gridPoints[_i].position.x, -0.35f, gridPoints[_i].position.y);
            gridPoints[_i].obj = obstacles1;
        }
        //Last border
        for (int i = gridSize * (gridSize - 1); i < gridSize * gridSize; i++)
        {
            Transform obstacles = Instantiate(obstacle, obstacleParent);
            obstacles.position = new Vector3(gridPoints[i].position.x, -0.35f, gridPoints[i].position.y);
            gridPoints[i].obj = obstacles;
        }

        //Spawing Obstacle Items
        for (int i = 0; i < maxObstacle; i++)
        {
            Transform obstacles;
            int rnd = Random.Range(0, 4);
            if (rnd == 0)
            {
                obstacles = Instantiate(breakableObstacle, obstacleParent);
            }
            else if (rnd == 1)
            {
                obstacles = Instantiate(hole, holeParent);
            }else
            {
                obstacles = Instantiate(obstacle, obstacleParent);
            }

            bool objPlacedDone = false;
            while (!objPlacedDone)
            {
                int p = Random.Range(0, gridPoints.Length - 1);
                if (gridPoints[p].obj == null)
                {
                    obstacles.position = new Vector3(gridPoints[p].position.x, -0.35f, gridPoints[p].position.y);
                    gridPoints[p].obj = obstacles;
                    if (obstacles.GetComponent<BreakableObstacle>())
                    {
                        Transform _floor = Instantiate(floor, floorParent);
                        _floor.position = new Vector3(gridPoints[p].position.x, -0.5f, gridPoints[p].position.y);
                        boxController.breakableObstacles.Add(obstacles.GetComponent<BreakableObstacle>());
                        obstacles.GetComponent<BreakableObstacle>().maxMove = Random.Range(1, 4);
                        obstacles.GetComponent<BreakableObstacle>().gridIndex = p;
                    }else if(obstacles.tag == "Hole")
                    {
                        obstacles.position = new Vector3(gridPoints[p].position.x, -0.5f, gridPoints[p].position.y);
                    }
                    objPlacedDone = true;
                }
            }
        }

        //Checks for vailidy
        CheckNeighbourGridCell();

        //Setting up floors
        for (int i = 0; i < gridPoints.Length; i++)
        {
            if (gridPoints[i].isValid)
            {
                Transform _floor = Instantiate(floor, floorParent);
                _floor.position = new Vector3(gridPoints[i].position.x, -0.5f, gridPoints[i].position.y);
            }
        }

        //Spawing Box and Flags Items
        for (int i = 0; i < maxFlags; i++)
        {
            boxes[i] = Instantiate(box, boxParent);
            flags[i] = Instantiate(flag, flagParent);
            bool objPlacedDone = false;
            while (!objPlacedDone)
            {
                int p = Random.Range(0, gridPoints.Length - 1);
                if (gridPoints[p].isValid && gridPoints[p].obj == null)
                {
                    boxes[i].position = new Vector3(gridPoints[p].position.x, -0.06f, gridPoints[p].position.y);
                    boxes[i].GetComponent<BoxMovement>().currentPosition = gridPoints[p].position;
                    boxes[i].GetComponent<BoxMovement>().gridIndex = p;
                    boxes[i].GetComponent<BoxMovement>().gridBuilder = instance;
                    boxes[i].GetComponent<BoxMovement>().CheckPossibleDirection();
                    flags[i].position = new Vector3(gridPoints[p].position.x, -0.48f, gridPoints[p].position.y);
                    gridPoints[p].obj = boxes[i];
                    objPlacedDone = true;
                }
            }
        }

        boxController.AssignBox(boxes, gridPoints);

        gridBuilded = true;
    }

    //Point Container
    [System.Serializable]
    public struct Point
    {
        public Vector2 position;
        public Transform obj;
        public bool isValid;
    }

    //Check for neighbour cell and Set Current cell to valid if it can move
    void CheckNeighbourGridCell()
    {
        for (int i = 0; i < gridPoints.Length; i++)        {

            if (gridPoints[i].obj == null)
            {

                Point pC = gridPoints[i]; //center point

                int ways = 0;

                //Y for left-right and X for up-down
                if (pC.position.y < offsetPosition)
                {
                    Point pR = gridPoints[i + 1];
                    if (pR.obj == null) { ways++; }
                }

                if (pC.position.y > -offsetPosition)
                {
                    Point pL = gridPoints[i - 1];
                    if (pL.obj == null) { ways++; }
                }

                if (pC.position.x > -offsetPosition)
                {
                    Point pU = gridPoints[i - gridSize];
                    if (pU.obj == null) { ways++; }
                }

                if (pC.position.x < offsetPosition)
                {
                    Point pD = gridPoints[i + gridSize];
                    if (pD.obj == null) { ways++; }
                }

                if(ways > 0) 
                { 
                    gridPoints[i].isValid = true; 
                } 
                else 
                {
                    Transform obstacles = Instantiate(obstacle, obstacleParent);
                    obstacles.position = new Vector3(gridPoints[i].position.x, -0.35f, gridPoints[i].position.y);
                    gridPoints[i].obj = obstacles;
                }
                //print($"P:{i}, W:{ways}, VAILD:{gridPoints[i].isVaild}");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (gridPoints != null)
        {
            foreach (Point point in gridPoints)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(new Vector3(point.position.x, 0, point.position.y), 0.1f);
            }
        }
    }
}
