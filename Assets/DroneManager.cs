using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public GameObject dronePrefab;
    public List<Transform> targets;
    public Pathfinding pathfinder;
    public int maxDrones = 4;           // Number of drones
    public float droneHeight = 10f;     // Y position
    public float spawnOffsetRadius = 5f; // Random offset from target

    private List<DroneController> drones = new List<DroneController>();

    void Start()
    {
        DroneController.occupiedNodes.Clear(); // Reset for each play
        StartCoroutine(SpawnDronesNextFrame());
    }
    IEnumerator SpawnDronesNextFrame()
    {
        yield return null; // Wait a frame to ensure grid is initialized
        SpawnDrones();
    }


    void SpawnDrones()
    {
        for (int i = 0; i < Mathf.Min(maxDrones, targets.Count); i++)
        {
            Node targetNode = pathfinder.grid.NodeFromWorldPoint(targets[i].position);

            // Pick a random direction (unit vector) and distance between min and max
            float minDistance = 80f; // minimum distance from target
            float maxDistance = 120f; // maximum distance
            Vector2 dir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDistance, maxDistance);

            Vector3 spawnPos = targetNode.worldPosition + new Vector3(dir.x, 0, dir.y) * distance;
            spawnPos.y = droneHeight;

            Node spawnNode = pathfinder.grid.NodeFromWorldPoint(spawnPos);

            // Ensure we don't spawn inside an obstacle
            if (!spawnNode.walkable)
            {
                Debug.LogWarning($"Drone {i} spawn node blocked. Adjusting position slightly.");
                spawnPos += Vector3.right * 2f; // small nudge
            }

            GameObject droneObj = Instantiate(dronePrefab, spawnPos, Quaternion.identity);
            DroneController ctrl = droneObj.GetComponent<DroneController>();
            ctrl.pathfinder = pathfinder;
            ctrl.target = targets[i];
            ctrl.fixedHeight = droneHeight;
            ctrl.InitializePath();

            drones.Add(ctrl);
        }
    }
}