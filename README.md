# RTGEF-Demos

Unity, Unreal Engine, and Godot lecture demos for the Real-Time Game Engine Fundamentals course.

## Layout

- `Unity/Lecture01/` through `Unity/Lecture06/`: independent Unity projects.
- `Unity/Templates/Basic3D/` and `Unity/Templates/Basic2D/`: reserved for reusable Unity templates.
- `Unreal/Lecture07/` through `Unreal/Lecture12/`: independent Unreal projects.
- `Godot/Lecture13/`: the Godot project.

These folders are placeholders; no engine projects or templates have been created yet. `.gitkeep` files let Git retain the empty folders and can be removed when projects are added. There is no Lecture 14 project because that week is for presentations.

## Adding projects

Use each lecture folder as the project root, without an extra project-name subfolder. For example, Unity's `Assets/`, Unreal's `.uproject`, or Godot's `project.godot` should sit directly inside its lecture folder. Each project will have its own README describing the exact engine version, dependencies, starting scene, controls, demonstration steps, reset procedure and verification results.

The root `.gitignore` covers common local files. Each engine folder has its own `.gitignore` for its projects. Unity rules also cover the two template folders. Godot rules target version 4.1 and later; review older projects before importing them.

Put exported builds in the project's `Builds/` folder or the repository's root `Builds/` folder. Unreal's `Build/` folder remains tracked because it can contain required packaging resources. Required plugin binaries and source assets remain eligible for tracking.

Before importing large binary assets, configure Git LFS for the selected asset types. Git LFS has not been configured by this folder setup.
