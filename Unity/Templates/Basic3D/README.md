# RTGEF Unity lecture template

Minimal 3D baseline for independent lecture projects. No gameplay controller or lecture-specific demo is included.

## Versions and setup

- Unity 6000.3.25f1 (6.3 LTS).
- URP 17.3.0; Input System 1.20.0, new input only.
- Unity Pipeline 0.8.0-exp.1 for Editor automation.
- Existing Unity template packages and render settings retained; exact dependencies are in Packages/manifest.json and Packages/packages-lock.json.
- Windows standalone target; 1280 x 720 windowed default. Demo is the sole build scene.
- Force Text serialization and Visible Meta Files.

## Open and demonstrate

Open Assets/Scenes/Demo.unity and enter Play mode. The camera shows a blue cube on a grey ground plane, with a directional light and the starter Global Volume. There are no gameplay controls or movement scripts.

Use the Inspector to change the cube's position during Play mode, then stop Play mode to restore its saved position (0, 0.5, 0). Do not save runtime changes back to the scene. No persistent data or intentional faults exist in this baseline.

Assets/Materials contains Ground, Blue and Orange URP materials. Scripts and Prefabs are empty starting folders. Add Art and Audio when a lesson needs them. The supplied InputSystem_Actions asset is retained but no gameplay script consumes it yet.

## Reuse

Copy Assets, Packages and ProjectSettings, plus this README and .gitignore. Preserve every .meta file. Exclude Library, Temp, Logs, UserSettings, IDE files and builds. Give each copy its own product name and remove its local link to the template's cloud project; the repository copies have been prepared this way.

Lecture 2 will use direct keyboard polling through the new Input System; Input Actions are introduced in Lecture 3. Lecture 3 will start from a separately created Universal 2D project. Transfer only reusable materials/folder conventions/documentation as appropriate; preserve its 2D renderer and project settings.

## Build and assets

Use File > Build Profiles with Windows and the Demo scene. Write output to Builds/. A standalone build has not been tested.

Assets are Unity starter-template assets, Unity primitives and three simple materials created for this course. No third-party art or audio was added.

## Verification

2026-10-03: opened in Unity 6000.3.25f1; Play mode render visually checked; keyboard device detected; temporary cube movement reset correctly after stopping Play mode; Console reported zero warnings/errors. This verifies the baseline, not a complete lecture demonstration.

Repository copy: imported and scene validated in Unity 6000.3.25f1 on 2026-10-03. Product name is RTGEF Unity 3D Template; inherited Cloud binding removed.
