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
    public Point[] gridPoints;
    private float offsetPosition;
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
        maxObstacle = (int)(gridSize * gridSize * Random.Range(0.3f, maxObstaclePercentage));
        obstacles = new Transform[maxObstacle];


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

        /*
        //Spawing Obstacle Items
        for (int i = 0; i < maxObstacle; i++)
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
        }*/

        StartCoroutine(CheckNeighbourGridCell());

        /*
        //Spawing Box and Flags Items
        for (int i = 0; i < maxFlags; i++)
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
                    flags[i].position = new Vector3(gridPoints[p].position.x, -0.3f, gridPoints[p].position.y);
                    gridPoints[p].obj = boxes[i];
                    objPlacedDone = true;
                }
            }
        }*/
    }

    //Point Container
    public struct Point
    {
        public Vector2 position;
        public Transform obj;
    }

    IEnumerator CheckNeighbourGridCell()
    {
        for (int i = 0; i < gridPoints.Length; i++)
        {
            print(i);
            yield return new WaitForSeconds(0.2f);
            Point pC = gridPoints[i]; //center point
            Point pR; //Right point
            Point pL; //Left point
            Point pU; //Up point
            Point pD; //Down point

            Transform o = Instantiate(box, new Vector3(pC.position.x, 0, pC.position.y), Quaternion.identity, boxParent);
            o.GetComponent<Renderer>().material.color = Color.green;
            Transform o1 = o;
            Transform o2 = o;
            Transform o3 = o;
            Transform o4 = o;
            //Y for left-right and X for up-down
            if (pC.position.y  < offsetPosition)
            {
                pR = gridPoints[i + 1];
                o1 = Instantiate(obstacle, new Vector3(pR.position.x, 0, pR.position.y), Quaternion.identity, boxParent);
                o1.GetComponent<Renderer>().material.color = Color.red;
            }
            if (pC.position.y > -offsetPosition)
            {
                pL = gridPoints[i - 1];
                o2 = Instantiate(obstacle, new Vector3(pL.position.x, 0, pL.position.y), Quaternion.identity, boxParent);
                o2.GetComponent<Renderer>().material.color = Color.red;
            }
            if (pC.position.x > -offsetPosition)
            {
                pU = gridPoints[i - gridSize];
                o3 = Instantiate(obstacle, new Vector3(pU.position.x, 0, pU.position.y), Quaternion.identity, boxParent);
                o3.GetComponent<Renderer>().material.color = Color.red;
            }
            if (pC.position.x < offsetPosition)
            {
                pD = gridPoints[i + gridSize];
                o4 = Instantiate(obstacle, new Vector3(pD.position.x, 0, pD.position.y), Quaternion.identity, boxParent);
                o4.GetComponent<Renderer>().material.color = Color.red;
            }

            yield return new WaitForSeconds(0.3f);

            Destroy(o.gameObject);
            Destroy(o1.gameObject);
            Destroy(o2.gameObject);
            Destroy(o3.gameObject);
            Destroy(o4.gameObject);
        }
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
