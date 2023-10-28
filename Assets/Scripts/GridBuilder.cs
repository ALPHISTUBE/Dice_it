using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [Range(3, 10)]
    public int gridSize = 3;

    //Caching
    public Point[] gridPoints; 

    void Update()
    {
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
