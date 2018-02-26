using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


[System.Serializable]
public class MultiDimensionalInt
{
    public int[] intArray; // bool, could be color

    public MultiDimensionalInt(int range)
    {
        //intArray = new int[range];
    }
}

public class GridControl : MonoBehaviour {


    int cols = 0;
    int rows = 0;

    public GameObject cellPrefab;

    public Cell[] cells;// = new Cell[M, N];
    public float[] grid;
    public int speed;

    // Use this for initialization
    void Start()
    {
        Load("test20");

        Shader.SetGlobalInt("cols", cols);
        Shader.SetGlobalInt("rows", rows);

        int M = cols;
        int N = rows;

        cells = new Cell[M*N];
        for ( int i = 0; i < M; i++)
        {
            for ( int j = 0; j < N; j++)
            {
                GameObject go = Instantiate(cellPrefab, new Vector3(i, 0, j), Quaternion.identity);
                cells[i*N+j] = go.GetComponent<Cell>();
                cells[i*N+j].SetAlive((int)grid[i*N+j]);
            }
            //cells[i,j] = new Cell(i,j);
        }

        Shader.SetGlobalFloatArray("pixelsInfo", grid);

        InvokeRepeating("nextGeneration", 2.0f, 0.3f);
    }
    //[ExecuteInEditMode]
    void nextGeneration()
    {
        int M = cols;
        int N = rows;

        int[,] future = new int[M, N];

        // Loop through every cell
        for (int l = 1; l < M - 1; l++)
        {
            for (int m = 1; m < N - 1; m++)
            {
                // finding no Of Neighbours that are alive
                int aliveNeighbours = 0;
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        aliveNeighbours += (int)grid[(l+i)*N + m+j];

                // The cell needs to be subtracted from
                // its neighbours as it was counted before
                aliveNeighbours -= (int)grid[l*N+m];

                // Implementing the Rules of Life

                // Cell is lonely and dies
                if ((grid[l * N + m] == 1) && (aliveNeighbours < 2))
                    future[l, m] = 0;

                // Cell dies due to over population
                else if ((grid[l * N + m] == 1) && (aliveNeighbours > 3))
                    future[l, m] = 0;

                // A new cell is born
                else if ((grid[l * N + m] == 0) && (aliveNeighbours == 3))
                    future[l, m] = 1;

                // Remains the same
                else
                    future[l, m] = (int)grid[l * N + m];
            }
        }

        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                grid[i*N+j] = future[i, j];
                cells[i*N+j].SetAlive(future[i,j]);
            }
        }

        Shader.SetGlobalFloatArray("pixelsInfo", grid);
        //cols = Shader.GetGlobalInt("cols");
        //grid = Shader.GetGlobalFloatArray("pixelsInfo");
    }

    // Update is called once per frame
    [ExecuteInEditMode]
    void Update ()
    {
        if ( cols == 0)
        Shader.SetGlobalInt("cols", 0);

    }

    //Load map file or use random ( with 20% uniform crowdness )
    public bool Load(string levelName)
    {
        TextAsset lvl = Resources.Load(levelName) as TextAsset;
        Debug.Log(lvl.text);

        string text = lvl.text;

        string[] fs = new string[] { "\r\n", "\r", "\n" };
        string[] fLines = text.Split(fs, StringSplitOptions.None);

        
        //read size of the map
        if (Int32.TryParse(fLines[0], out cols))
            Console.WriteLine(cols);
        
        if (Int32.TryParse(fLines[1], out rows))
            Console.WriteLine(rows);

        grid = new float[cols * rows];

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                grid[i * cols + j] = fLines[i+2][j] - '0';
            }

            Debug.Log(fLines[i+2] + "\n");
            //string valueLine = fLines[i];
            //string[] values = Regex.Split(valueLine, ";"); // your splitter here

            //Spell newSpell = new Spell(values[0], ... ) // etc
            //return newSpell;
        }

        return true;
    }
}
