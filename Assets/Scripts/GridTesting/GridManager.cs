using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab; // Assign your grid cell prefab in the inspector
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public float cellSize = 40f;

    private GameObject[,] grid;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GameObject[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 spawnPosition = new Vector3(x * cellSize, 0f, y * cellSize);
                GameObject cell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity);
                cell.transform.SetParent(transform); // Parent the cell to the GridManager GameObject

                grid[x, y] = cell;
            }
        }
    }


}