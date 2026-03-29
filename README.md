# Drone Pathfinding in Unity

## 📌 Project Overview
This Unity project demonstrates a **multi-drone pathfinding system** using a grid-based approach.  
Each drone moves autonomously toward its assigned target while avoiding obstacles and other drones, using a custom **A* pathfinding algorithm**.

- Drones spawn the targets with a configurable random offset.
- The system supports multiple drones moving simultaneously.
- Paths are dynamically generated based on grid obstacles.

---

## 🚀 Features
- Grid-based navigation system (`GridManager` and `Node` classes)
- A* pathfinding (`Pathfinding` class)
- Multiple drones with collision-free movement (`DroneController`)
- Configurable drone speed, height, and stopping distance
- Randomized spawn points around targets (`DroneManager`)
- Automatic reset of occupied nodes each run
