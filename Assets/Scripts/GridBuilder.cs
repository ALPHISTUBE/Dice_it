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
    public Transform border;

    //Caching
    public Point[] gridPoints;
    [HideInInspector] public float offsetPosition;
    private Transform[] boxes;
    private int maxObstacle;
    private Transform[] flags;
    private List<int> holes = new List<int>();

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
        float borderScale = ((gridSize - 5) * 0.5f) + 1.5f;
        border.localScale = new Vector3(borderScale, borderScale, 0.5f); //Z axis is up
        //1st border
        for (int i = 0; i < gridSize; i++)
        {
            //Transform obstacles = Instantiate(obstacle, obstacleParent);
            //obstacles.position = new Vector3(gridPoints[i].position.x, -0.35f, gridPoints[i].position.y);
            gridPoints[i].obj = border;
        }
        //Middle border
        for (int i = gridSize; i < (gridSize * (gridSize - 2)) + 1; i += gridSize)
        {
            //Transform obstacles = Instantiate(obstacle, obstacleParent);
            //obstacles.position = new Vector3(gridPoints[i].position.x, -0.35f, gridPoints[i].position.y);
            gridPoints[i].obj = border;

            int _i = i + (gridSize - 1);
            //Transform obstacles1 = Instantiate(obstacle, obstacleParent);
            //obstacles1.position = new Vector3(gridPoints[_i].position.x, -0.35f, gridPoints[_i].position.y);
            gridPoints[_i].obj = border;
        }
        //Last border
        for (int i = gridSize * (gridSize - 1); i < gridSize * gridSize; i++)
        {
            //Transform obstacles = Instantiate(obstacle, obstacleParent);
            //obstacles.position = new Vector3(gridPoints[i].position.x, -0.35f, gridPoints[i].position.y);
            gridPoints[i].obj = border;
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
                    obstacles.name = $"{obstacles.name}({p})";
                    obstacles.position = new Vector3(gridPoints[p].position.x, -0.35f, gridPoints[p].position.y);
                    gridPoints[p].obj = obstacles;
                    if (obstacles.GetComponent<BreakableObstacle>())
                    {
                        Transform _floor = Instantiate(floor, floorParent);
                        _floor.position = new Vector3(gridPoints[p].position.x, -0.5f, gridPoints[p].position.y);
                        boxController.breakableObstacles.Add(obstacles.GetComponent<BreakableObstacle>());
                        int _maxMove = Random.Range(1, 4);
                        obstacles.GetComponent<BreakableObstacle>().maxMove = _maxMove;
                        obstacles.GetComponent<BreakableObstacle>().SetStartMaxMove(_maxMove);
                        obstacles.GetComponent<BreakableObstacle>().gridIndex = p;
                    }else if(obstacles.tag == "Hole")
                    {
                        holes.Add(p);
                        obstacles.position = new Vector3(gridPoints[p].position.x, -0.5f, gridPoints[p].position.y);
                        gridPoints[p].hasHole = true;
                        gridPoints[p].isValid = true;
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
            if (gridPoints[i].isValid && !gridPoints[i].hasHole)
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
                    BoxMovement bm = boxes[i].GetComponent<BoxMovement>();
                    bm.pos = new Vector3(gridPoints[p].position.x, -0.06f, gridPoints[p].position.y);
                    bm.currentPosition = gridPoints[p].position;
                    bm.gridIndex = p;
                    bm.SetStartGridIndex(p);
                    bm.gridBuilder = instance;
                    bm.CheckPossibleDirection();
                    flags[i].position = new Vector3(gridPoints[p].position.x, -0.48f, gridPoints[p].position.y);
                    gridPoints[p].obj = boxes[i];
                    objPlacedDone = true;
                }
            }
        }

        boxController.AssignBox(boxes, gridPoints);
        LevelCreator.instance.AssignBox(boxes, gridPoints, flags, holes);

        gridBuilded = true;
        StartCoroutine(LevelCreator.instance.CreateLevel());
    }

    //Point Container
    [System.Serializable]
    public struct Point
    {
        public Vector2 position;
        public Transform obj;
        public bool isValid;
        public bool hasHole;
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

                if (ways > 0)
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

    //Reset Holes
    public void ResetHoleVaildity()
    {
        foreach(int i in holes)
        {
            gridPoints[i].isValid = true;
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
