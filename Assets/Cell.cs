﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public bool isAlive {
        get { return isAlive; }
        set { isAlive = value; }
    }
    public bool isAliveNextFrame;

    [SerializeField]private Material liveMat;
    [SerializeField]private Material deatMat;

    public int x, y;
    public Cell[] neighbors;

    public Cell(int x, int y)
    {
        transform.position = new Vector3(x, y, 0);
    }

    public void SetAlive(int alive)
    {
        if ( alive != 0 )
        {
            GetComponent<Renderer>().material = liveMat;
        }
        else
        {
            GetComponent<Renderer>().material = deatMat;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
