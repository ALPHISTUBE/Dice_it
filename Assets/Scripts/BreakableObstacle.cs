using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObstacle : MonoBehaviour
{
    public int maxMove;
    public int gridIndex;
    private int startMaxMove;

    // Update is called once per frame
    void Update()
    {
        if (maxMove <= 0)
        {
            GridBuilder.instance.gridPoints[gridIndex].obj = null;
            GridBuilder.instance.gridPoints[gridIndex].isValid = true;
            gameObject.SetActive(false);
        }
    }

    //Access to value
    public int GetStartMaxMove()
    {
        return startMaxMove;
    }

    public void SetStartMaxMove(int i)
    {
        startMaxMove = i;
    }
}
