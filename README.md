# Multiplayer Third-Person Shooter

## 📝 Overview
A fast-paced, action-oriented multiplayer third-person shooter developed in Unity. This project serves as a comprehensive showcase of real-time multiplayer networking, precise state synchronization, and scalable code architecture using event-driven programming and object pooling.

## ✨ Key Technical Features
* **Real-Time Networking:** Engineered seamless multiplayer lobbies and room logic using **Photon PUN 2**.
* **State Synchronization & RPCs:** Leveraged Remote Procedure Calls (RPCs) to handle critical game events (shooting, ability usage) ensuring simultaneous execution across all clients, combined with smooth network transforms for position syncing.
* **Networked Health System:** Built a secure, master-client authoritative player health and damage system that accurately registers hits and broadcasts UI updates to all players.
* **Optimized Architecture:**  Implemented **Object Pooling** for  particle effects to eliminate garbage collection spikes during heavy combat.
  * Utilized an **Event-Driven UI** pattern to decouple the game logic from the visual representation (e.g., death notifications, ammo counters).
* **AI Bot Integration:** Developed AI combatants using a robust **State Machine** architecture, allowing them to navigate the networked environment and provide dynamic gameplay.

## 🛠️ Tech Stack
* **Game Engine:** Unity 
* **Language:** C#
* **Networking Framework:** Photon PUN 2
* **Version Control:** Git / GitHub





<img width="1920" height="1080" alt="Screenshot 2026-04-07 125314" src="https://github.com/user-attachments/assets/c4ccf9e7-48a0-423d-b37b-c1008370cbb8" />

