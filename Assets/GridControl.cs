using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

// [ExecuteInEditMode]
public class GridControl : MonoBehaviour {

    public bool is3D;

    int cols = 0;
    int rows = 0;
    int depth = 10;

    public GameObject cellPrefab;
    public TextAsset mapToLoad;

    public Cell[,,] cells;
    public int[,,] grid;
    public float refreshInterval = 0.3f;

    private void Reset()
    {
        CancelInvoke();

        int M = cols;
        int N = rows;
        int H = depth;

        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                for (int k = 0; k < H; k++)
                {
                    cells[i, j, k].SetAlive(0);
                }
            }
        }

        LoadSelectedMap(mapToLoad);
        
        M = cols;
        N = rows;
        depth = 10;

        Shader.SetGlobalInt("cols", cols);
        Shader.SetGlobalInt("rows", rows);
        
        cells = new Cell[M, N, H];
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                for (int k = 0; k < H; k++)
                {
                    GameObject go = Instantiate(cellPrefab, new Vector3(i - M/2, k, j - N/2), Quaternion.identity);

                    cells[i, j, k] = go.GetComponent<Cell>();
                    cells[i, j, k].SetAlive(grid[i, j, k]);
                }
            }
        }
        
        if (is3D)
            InvokeRepeating("nextGeneration3D", 1.0f, refreshInterval);
        else
            InvokeRepeating("nextGeneration", 1.0f, refreshInterval);


    }

    void Start()
    {
        Reset();
    }
    
    void nextGeneration()
    {
        int M = cols;
        int N = rows;
        int H = 1;
        int n = 0;

        int[,,] future = new int[M, N, H];

        // Loop through every cell
        for (int l = 1; l < M - 1; l++)
        {
            for (int m = 1; m < N - 1; m++)
            {
                // finding no Of Neighbours that are alive
                int aliveNeighbours = 0;
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                            aliveNeighbours += (int)grid[l + i, m + j, n];

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

        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                grid[i, j, 0] = future[i, j, 0];
                cells[i, j, 0].SetAlive(future[i, j, 0]);
            }
        }
        
    }
    int[,] offsets = {
{-1,-1,-1},{-1,-1,0},{-1,-1,1},{-1,0,-1},{-1,0,0},{-1,0,1},{-1,1,-1},{-1,1,0},{-1,1,1},
{0,-1,-1},{0,-1,0},{0,-1,1},{0,0,-1},{0,0,0},{0,0,1},{0,1,-1},{0,1,0},{0,1,1},
{1,-1,-1},{1,-1,0},{1,-1,1},{1,0,-1},{1,0,0},{1,0,1},{1,1,-1},{1,1,0},{1,1,1}};
    
    void nextGeneration3D()
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
                    for ( int off = 0; off < 27; off++ )
                    {
                        int i = offsets[off, 0];
                        int j = offsets[off, 1];
                        int k = offsets[off, 2];
                        aliveNeighbours += (int)grid[l + i, m + j, n + k];
                    }

                    // The cell needs to be subtracted from
                    // its neighbours as it was counted before
                    aliveNeighbours -= (int)grid[l, m, n];

                    // Implementing the Rules of Life

                    bool alive = (grid[l, m, n] == 1);

                    // Cell is lonely and dies
                    if (alive && (aliveNeighbours < 2))
                    {
                        future[l, m, n] = 0;
                    }

                    // Cell dies due to over population
                    else if (alive && (aliveNeighbours > 3))
                    {
                        future[l, m, n] = 0;
                    }

                    // A new cell is born
                    else if (!alive && (aliveNeighbours == 3))
                    {
                        future[l, m, n] = 1;
                    }

                    // Remains the same
                    else
                    {
                        future[l, m, n] = (int)grid[l, m, n];
                    }
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
    }

    public void LoadSelectedMap(TextAsset lvl)
    {
        string text = lvl.text;

        string[] fs = new string[] { "\r\n", "\r", "\n" };
        string[] fLines = text.Split(fs, StringSplitOptions.None);
        
        //read size of the map
        if (Int32.TryParse(fLines[0], out cols)) { }
        //  Console.WriteLine(cols);

        if (Int32.TryParse(fLines[1], out rows)) { }
        //  Console.WriteLine(rows); 

        grid = new int[cols, rows, depth];

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                //for (int k = 0; k < 10; k++)
                if (is3D)
                {
                    grid[i, j, depth / 2] = fLines[i + 2][j] - '0';
                }
                else
                {
                    grid[i, j, 0] = fLines[i + 2][j] - '0';
                }
            }
        }
    }
    //Load map file or use random ( with 20% uniform crowdness )
    public void Load(string levelName)
    {
        TextAsset lvl = Resources.Load(levelName) as TextAsset;
        Debug.Log(lvl.text);

        LoadSelectedMap(lvl);
    }
}
