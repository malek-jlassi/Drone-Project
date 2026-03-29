using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GridManager grid;

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        foreach (Node n in grid.grid)
        {
            n.gCost = int.MaxValue;
            n.parent = null;
        }

        startNode.gCost = 0;
        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            foreach (Node node in openSet)
                if (node.fCost < currentNode.fCost || (node.fCost == currentNode.fCost && node.hCost < currentNode.hCost))
                    currentNode = node;

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;
                int newCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;
                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;
                if (checkX < 0 || checkX >= grid.gridSizeX || checkZ < 0 || checkZ >= grid.gridSizeZ) continue;
                if (x != 0 && z != 0) // diagonal cut prevention
                {
                    if (!grid.grid[node.gridX + x, node.gridZ].walkable || !grid.grid[node.gridX, node.gridZ + z].walkable)
                        continue;
                }
                neighbors.Add(grid.grid[checkX, checkZ]);
            }
        return neighbors;
    }

    int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dz = Mathf.Abs(a.gridZ - b.gridZ);
        return dx > dz ? 14 * dz + 10 * (dx - dz) : 14 * dx + 10 * (dz - dx);
    }

    List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;
        while (current != start) { path.Add(current); current = current.parent; }
        path.Reverse();
        return path;
    }
}