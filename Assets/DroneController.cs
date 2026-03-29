using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public Transform target;
    public Pathfinding pathfinder;
    public float speed = 5f;
    public float fixedHeight = 10f;
    public float stoppingDistance = 1f; // Distance from final target to stop

    private List<Node> path;
    private int pathIndex = 0;
    private Node currentNode;

    public static HashSet<Node> occupiedNodes = new HashSet<Node>();
    private Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    public void InitializePath()
    {
        occupiedNodes.Remove(currentNode); // Free the last node
        currentNode = null;

        if (pathfinder == null || target == null) return;

        Node startNode = pathfinder.grid.NodeFromWorldPoint(transform.position);
        Node targetNode = pathfinder.grid.NodeFromWorldPoint(target.position);

        path = pathfinder.FindPath(startNode.worldPosition, targetNode.worldPosition);

        if (path == null || path.Count == 0)
        {
            Debug.LogError("NO PATH FOUND: " + gameObject.name);
        }
        else
        {
            Debug.Log($"{gameObject.name} Path length: {path.Count}");
        }

        pathIndex = 0; // Reset path index
    }

    void FixedUpdate()
    {
        if (path == null) return;

        Vector3 targetPos;

        // If we reached the last node, move toward the actual target position
        if (pathIndex >= path.Count)
        {
            targetPos = target.position;
        }
        else
        {
            Node nextNode = path[pathIndex];
            targetPos = new Vector3(nextNode.worldPosition.x, fixedHeight, nextNode.worldPosition.z);
        }

        Vector3 dir = targetPos - transform.position;
        dir.y = 0; // Keep movement horizontal

        // Check stopping condition for the final target
        if (pathIndex >= path.Count && dir.magnitude <= stoppingDistance)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // Move drone
        rb.velocity = dir.normalized * speed;

        // Rotate drone smoothly
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);

        // Advance path index if close to node
        if (pathIndex < path.Count && dir.magnitude < 0.5f)
        {
            pathIndex++;
           // rb.velocity = Vector3.zero;
        }
        rb.velocity = dir.normalized * speed;
    }
}