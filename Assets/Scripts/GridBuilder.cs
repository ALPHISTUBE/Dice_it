using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    //Settings
    [Header("Settings")]
    [Range(3, 10)]
    public int gridSize = 3;
    [Range(1, 6)]
    public int maxFlags;
    [Range(0, 0.5f)]
    public float maxObstaclePercentage = 0.2f;

    //Prefabs
    [Header("Prefabs")]
    public Transform obstacle;
    public Transform box;
    public Transform flag;

    //Caching
    private Point[] gridPoints;
    private Transform[] boxes;
    private int maxObstacle;
    private Transform[] obstacles;
    private Transform[] flags;

    //Parents
    [Header("Parents")]
    public Transform obstacleParent;
    public Transform boxParent;
    public Transform flagParent;

    void Start()
    {
        //Setting up Level Properties
        boxes = new Transform[maxFlags];
        flags = new Transform[maxFlags];
        maxObstacle = (int)(gridSize * gridSize * Random.Range(0.3f, maxObstaclePercentage));
        obstacles = new Transform[maxObstacle];


        //Creating Grid Points
        gridPoints = new Point[gridSize * gridSize];
        float offsetPosition = ((float)gridSize - 1) / 2;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                gridPoints[i * gridSize + j].position = new Vector2(i - offsetPosition, j - offsetPosition);
            }
        }

        //Spawing level Items
        for (int i = 0; i < maxObstacle; i++) //Obstacle
        {
            obstacles[i] = Instantiate(obstacle, obstacleParent);
            bool objPlacedDone = false;
            while (!objPlacedDone)
            {
                int p = Random.Range(0, gridPoints.Length - 1);
                if (gridPoints[p].obj == null)
                {
                    obstacles[i].position = new Vector3(gridPoints[p].position.x, 0, gridPoints[p].position.y);
                    gridPoints[p].obj = obstacles[i];
                    objPlacedDone = true;
                }
            }            
        }

        for (int i = 0; i < maxFlags; i++) //Obstacle
        {
            boxes[i] = Instantiate(box, boxParent);
            boxes[i].GetComponent<Renderer>().material.color = Color.blue;
            flags[i] = Instantiate(flag, flagParent);
            flags[i].GetComponent<Renderer>().material.color = Color.green;
            bool objPlacedDone = false;
            while (!objPlacedDone)
            {
                int p = Random.Range(0, gridPoints.Length - 1);
                if (gridPoints[p].obj == null)
                {
                    boxes[i].position = new Vector3(gridPoints[p].position.x, 0, gridPoints[p].position.y);
                    flags[i].position = new Vector3(gridPoints[p].position.x, 0, gridPoints[p].position.y);
                    gridPoints[p].obj = boxes[i];
                    objPlacedDone = true;
                }
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    //Point Container
    public struct Point
    {
        public Vector2 position;
        public Transform obj;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (gridPoints != null)
        {
            foreach (Point point in gridPoints)
            {
                Gizmos.DrawWireSphere(new Vector3(point.position.x, 0, point.position.y), 0.1f);
            }
        }
    }
}
