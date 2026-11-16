# Lecture 11 — Behaviour trees

Open `Lecture11.uproject` in **Unreal Engine 5.8.4**. The startup map is `/Game/Lecture11/Maps/L_PatrolCourtyard`.

Walk around the cover walls to reveal yourself to the guard, then hide or outrun it. The guard wanders, chases a visible player and remembers the target for three seconds after losing sight.

- **WASD / mouse / Space:** move, look and jump.
- **R:** reset the current map.
- **1:** task version.
- **2:** service version (`L_ServicePatrol`).

Stop and restart Play for a full reset. AI Blueprints, the Blackboard and both trees are under `/Game/Lecture11/AI`. Retain the project's existing plugins.
