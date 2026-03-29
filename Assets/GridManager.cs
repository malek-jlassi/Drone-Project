using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridSizeX = 400;
    public int gridSizeZ = 400;
    public float nodeSize = 8f;
    public LayerMask obstacleLayer;

    public Node[,] grid;

    void Awake() => CreateGrid();

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeZ];
        Vector3 bottomLeft = transform.position - new Vector3(gridSizeX * nodeSize / 2f, 0, gridSizeZ * nodeSize / 2f);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 worldPos = bottomLeft + new Vector3(x * nodeSize + nodeSize / 2f, 0, z * nodeSize + nodeSize / 2f);
                bool isObstacle = Physics.CheckSphere(worldPos + Vector3.up * 0.5f, nodeSize * 0.45f, obstacleLayer);
                grid[x, z] = new Node(!isObstacle, worldPos, x, z);
            }
        }

        Debug.Log("Grid created: " + gridSizeX + "x" + gridSizeZ);
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        Vector3 bottomLeft = transform.position - new Vector3(gridSizeX * nodeSize / 2f, 0, gridSizeZ * nodeSize / 2f);
        int x = Mathf.Clamp(Mathf.FloorToInt((worldPos.x - bottomLeft.x) / nodeSize), 0, gridSizeX - 1);
        int z = Mathf.Clamp(Mathf.FloorToInt((worldPos.z - bottomLeft.z) / nodeSize), 0, gridSizeZ - 1);
        return grid[x, z];
    }
}