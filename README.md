# RTGEF-Demos

Unity, Unreal Engine, and Godot lecture demos for the Real-Time Game Engine Fundamentals course.

## Layout

- [Unity Lecture02](Unity/Lecture02/README.md): playable fundamentals demo with movement, jumping, coins, score UI and scene loading.
- `Unity/Lecture04/`, `Lecture05/` and `Lecture06/`: minimal independent 3D Unity baselines.
- `Unity/Lecture01/`: unused placeholder; no dedicated demo currently needed.
- `Unity/Lecture03/`: awaits Alex's separately created Universal 2D project.
- [Unity 3D template](Unity/Templates/Basic3D/README.md): reusable baseline. `Unity/Templates/Basic2D/` remains a placeholder.
- `Unreal/Lecture07/` through `Unreal/Lecture12/`: independent Unreal projects.
- `Godot/Lecture13/`: the Godot project.

Unity 3D baselines have been created, and Lecture 2 gameplay is implemented. Other lesson-specific gameplay is still pending. Unreal, Godot and the Unity 2D folders remain placeholders. `.gitkeep` files let Git retain the empty folders and can be removed when projects are added. There is no Lecture 14 project because that week is for presentations.

## Coding conventions

Follow [C# coding guidelines](CODING_GUIDELINES.md) for every first-party project and reusable editor snippet. The repository .editorconfig supplies formatting defaults; access, Inspector grouping and behavior-preservation rules remain part of review. Course demos use built-in Unity Inspector features without NaughtyAttributes.

## Adding projects

Unity demos and templates use **Unity 6000.3.25f1 (6.3 LTS)**. The 3D baseline includes URP 17.3.0, Input System 1.20.0 and Unity Pipeline 0.8.0-exp.1. Lecture 2 polls the existing project-wide Move/Jump actions; Lecture 3 covers creating and configuring Input Actions. This supersedes the earlier direct-keyboard-polling plan for player movement. No legacy input is needed in the new projects.

Use each lecture folder as the project root, without an extra project-name subfolder. For example, Unity's `Assets/`, Unreal's `.uproject`, or Godot's `project.godot` should sit directly inside its lecture folder. Each project will have its own README describing the exact engine version, dependencies, starting scene, controls, demonstration steps, reset procedure and verification results.

The root `.gitignore` covers common local files. Each engine folder has its own `.gitignore` for its projects. Unity rules also cover the two template folders. Godot rules target version 4.1 and later; review older projects before importing them.

Put exported builds in the project's `Builds/` folder or the repository's root `Builds/` folder. Unreal's `Build/` folder remains tracked because it can contain required packaging resources. Required plugin binaries and source assets remain eligible for tracking.

Before importing large binary assets, configure Git LFS for the selected asset types. Git LFS has not been configured by this folder setup.
