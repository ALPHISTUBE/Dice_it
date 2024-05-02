using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObstacle : MonoBehaviour
{
    public int maxMove;
    public int gridIndex;
    private int startMaxMove;
    private Animator anim;
    public GameObject[] gpx;
    public ParticleSystem ps;

    private void Start()
    {
        anim = GetComponent<Animator>();
        int[] axis = { 90, 180, -90, 0 };
        transform.eulerAngles = new Vector3(0, axis[Random.Range(0,axis.Length)], 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (maxMove <= 0)
        {
            GridBuilder.instance.gridPoints[gridIndex].obj = null;
            GridBuilder.instance.gridPoints[gridIndex].isValid = true;
            //gameObject.SetActive(false);
        }
    }

    public void PlayAnimation()
    {
        if(maxMove < 0) { return; }
        ps.Play();
        anim.Play("break");
        gpx[maxMove].SetActive(true);
        for (int i = 0; i < gpx.Length; i++)
        {
            if(i != maxMove)
            {
                gpx[i].SetActive(false);
            }
        }
    }


    //Reset Breakable Obstacle
    public void ResetBreakableObstacle()
    {
        for (int i = 0;i < gpx.Length;i++)
        {
            gpx[i].SetActive(false);        
        }

        maxMove = GetStartMaxMove();
        PlayAnimation();
        GridBuilder.instance.gridPoints[gridIndex].obj = transform;
        GridBuilder.instance.gridPoints[gridIndex].isValid = false;
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
