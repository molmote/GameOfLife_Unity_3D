using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    int M = 10;
    int N = 10;

    public GameObject cellPrefab;

    public Cell[,] cells;// = new Cell[M, N];
    public MultiDimensionalInt[] grid;
    public int speed;

    // Use this for initialization
    void Start()
    {

        grid = new MultiDimensionalInt[10];
        for (int i = 0; i < M; i++)
        {
            grid[i] = new MultiDimensionalInt(N);
        }

        grid[0].intArray = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        grid[1].intArray = new int[] { 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 };
        grid[2].intArray = new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
        grid[3].intArray = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        grid[4].intArray = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        grid[5].intArray = new int[] { 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 };
        grid[6].intArray = new int[] { 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 };
        grid[7].intArray = new int[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 };
        grid[8].intArray = new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
        grid[9].intArray = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        cells = new Cell[M, N];
        for ( int i = 0; i < M; i++)
        {
            for ( int j = 0; j < N; j++)
            {
                GameObject go = Instantiate(cellPrefab, new Vector3(i, 0, j), Quaternion.identity);
                cells[i, j] = go.GetComponent<Cell>();
                cells[i, j].SetAlive(grid[i].intArray[j]);
            }
            //cells[i,j] = new Cell(i,j);
        }

        InvokeRepeating("nextGeneration", 2.0f, 0.3f);
    }
    //[ExecuteInEditMode]
    void nextGeneration()
    {
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
                        aliveNeighbours += grid[l + i].intArray[m + j];

                // The cell needs to be subtracted from
                // its neighbours as it was counted before
                aliveNeighbours -= grid[l].intArray[m];

                // Implementing the Rules of Life

                // Cell is lonely and dies
                if ((grid[l].intArray[m] == 1) && (aliveNeighbours < 2))
                    future[l, m] = 0;

                // Cell dies due to over population
                else if ((grid[l].intArray[m] == 1) && (aliveNeighbours > 3))
                    future[l, m] = 0;

                // A new cell is born
                else if ((grid[l].intArray[m] == 0) && (aliveNeighbours == 3))
                    future[l, m] = 1;

                // Remains the same
                else
                    future[l, m] = grid[l].intArray[m];
            }
        }

        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                grid[i].intArray[j] = future[i, j];
                cells[i, j].SetAlive(future[i,j]);
            }
        }
    }

    float elapsed;
// Update is called once per frame
void Update ()
    {
       // Time.deltaTime;
		
	}
}
