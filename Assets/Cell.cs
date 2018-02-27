using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    
    //public bool isAliveNextFrame;
    //[SerializeField]private Material liveMat;
    //[SerializeField]private Material deatMat;
    //public Cell[] neighbors;

    //public int x, y;
    //public float t;

    public void SetAlive(int alive)
    {
        if ( alive != 0 )
        {
            this.gameObject.SetActive(true);
            // GetComponent<Renderer>().material = liveMat;
        }
        else
        {
            //GetComponent<Renderer>().material = deatMat;
            this.gameObject.SetActive(false);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
