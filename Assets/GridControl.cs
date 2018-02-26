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
    int depth = 10;

    public GameObject cellPrefab;

    //public Cell[] cells;// = new Cell[M, N];
    public Cell[,,] cells;
    //public float[] grid;
    public int[,,] grid;
    public int speed;

    // Use this for initialization
    void Start()
    {
        Load("test20");

        Shader.SetGlobalInt("cols", cols);
        Shader.SetGlobalInt("rows", rows);

        int M = cols;
        int N = rows;
        int H = depth;

        ///cells = new Cell[M*N];
        cells = new Cell[M, N, H];
        for ( int i = 0; i < M; i++)
        {
            for ( int j = 0; j < N; j++)
            {
                for (int k = 0; k < H; k++)
                {
                    GameObject go = Instantiate(cellPrefab, new Vector3(i, k, j), Quaternion.identity);
///cells[i * N + j] = go.GetComponent<Cell>();
                   ///cells[i * N + j].SetAlive((int)grid[i * N + j]);
                    cells[i,j,k] = go.GetComponent<Cell>();
                    cells[i, j, k].SetAlive(grid[i, j, k]);
                }
            }
        }

        /// Shader.SetGlobalFloatArray("pixelsInfo", grid);

        InvokeRepeating("nextGeneration", 2.0f, 0.3f);
    }
    //[ExecuteInEditMode]
    void nextGeneration()
    {
        int M = cols;
        int N = rows;
        int H = depth;

        int[,,] future = new int[M, N, H];

        // Loop through every cell
        for (int l = 1; l < M - 1; l++)
        {
            for (int m = 1; m < N - 1; m++)
            {
                for (int n = 1; n < H - 1; n++)
                {
                    // finding no Of Neighbours that are alive
                    int aliveNeighbours = 0;
                    for (int i = -1; i <= 1; i++)
                        for (int j = -1; j <= 1; j++)
                            for ( int k = -1; k <= 1; k++)
                            aliveNeighbours += (int)grid[l + i, m + j, n+k];
                    //aliveNeighbours += (int)grid[(l+i)*N + m+j];

                    // The cell needs to be subtracted from
                    // its neighbours as it was counted before
                    aliveNeighbours -= (int)grid[l, m, n];

                    // Implementing the Rules of Life

                    // Cell is lonely and dies
                    if ((grid[l, m, n] == 1) && (aliveNeighbours < 2))
                        future[l, m, n] = 0;

                    // Cell dies due to over population
                    else if ((grid[l, m, n] == 1) && (aliveNeighbours > 3))
                        future[l, m, n] = 0;

                    // A new cell is born
                    else if ((grid[l, m, n] == 0) && (aliveNeighbours == 3))
                        future[l, m, n] = 1;

                    // Remains the same
                    else
                        future[l, m, n] = (int)grid[l, m, n];
                }
            }
        }

        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                for (int k = 0; k < H; k++)
                {
                    grid[i, j, k] = future[i, j, k];
                    cells[i, j, k].SetAlive(future[i, j, k]);
                }
            }
        }

        /// Shader.SetGlobalFloatArray("pixelsInfo", grid);
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
        if (Int32.TryParse(fLines[0], out cols)) { }
          //  Console.WriteLine(cols);
        
        if (Int32.TryParse(fLines[1], out rows)) { }
          //  Console.WriteLine(rows);

        grid = new int[cols , rows , depth];

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                //for (int k = 0; k < 10; k++)
                {
                    grid[i, j, depth/2] = fLines[i + 2][j] - '0';
                }
            }

            //Debug.Log(fLines[i+2] + "\n");
            //string valueLine = fLines[i];
            //string[] values = Regex.Split(valueLine, ";"); // your splitter here

            //Spell newSpell = new Spell(values[0], ... ) // etc
            //return newSpell;
        }

        return true;
    }
}
