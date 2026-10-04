# RTGEF Unity Lecture02

A small cube-and-coins demo supporting Lecture 2, Unity fundamental concepts. Open **Assets/Scenes/Demo.unity** and press Play.

## Requirements

Unity **6000.3.25f1**, URP **17.3.0**, Input System **1.20.0**, Unity Pipeline **0.8.0-exp.1**, uGUI **2.0.0** with TextMeshPro essentials. Exact dependencies are committed in Packages. Active Input Handling uses the new Input System only. The existing Assets/InputSystem_Actions.inputactions asset is already assigned as the project-wide actions and contains Player/Move and Player/Jump. No new action setup or PlayerInput component is required; project-wide actions are enabled automatically. The UI uses InputSystemUIInputModule.

## Controls

| Control | Behaviour |
|---|---|
| WASD, arrow keys or gamepad left stick | Move on the XZ plane |
| Space or gamepad south button | Jump while grounded |
| R or Restart button | Reset score, restore all coins and return to Demo |
| N or Next level button | Switch between Demo and Level2 while retaining score |
| B or Add bonus button | Load BonusArea additively once in the current level |

Focus the Game view for keyboard input. The blue player collects gold spinning coins; every coin adds one point. The grey step supports jumping, the solid obstacle blocks movement, the green zone demonstrates trigger enter/stay/exit, the moving block demonstrates kinematic MovePosition, and the coral ball demonstrates a bouncy physics material. Use Restart to reset the player; automatic fall respawn was removed to keep the controller focused.

## Classroom sequence and concept map

| Lecture topic | What to show |
|---|---|
| Git and project settings | Review the engine/project ignore files, Assets with .meta files, Packages and ProjectSettings. Force Text and Visible Meta Files are configured. Demonstrate commits separately in GitKraken; no commits were made by this setup. |
| Vectors, normalization and diagonal speed | Player.Update reads Move as a Vector2, maps x/y to the Vector3 XZ plane and normalizes the direction. Compare one direction with a diagonal. This simple controller uses constant speed for any nonzero input, including a partly tilted gamepad stick. |
| Time.deltaTime | Coin.Update rotates at degrees per second multiplied by deltaTime. Change rotationSpeed on the prefab and inspect its instances. |
| Input | Player.Start caches Player/Move and Player/Jump with InputSystem.actions.FindAction. Update polls ReadValue<Vector2>() and WasPressedThisFrame(). Use the existing default bindings in Lecture 2; creating/configuring actions is covered in Lecture 3. DemoUI still polls R/N/B directly. The lecture slides use these existing actions. |
| Components and references | Player has BoxCollider, Rigidbody and Player. Player.Start caches rb using GetComponent. Camera target, UI labels and zone label demonstrate Inspector references. |
| Update / FixedUpdate / LateUpdate | Read input and queue the jump in Update, apply player physics in FixedUpdate, follow the player in LateUpdate. |
| Rigidbody movement | The dynamic player sets horizontal linearVelocity while preserving vertical velocity and uses AddForce with Impulse for jumping. Neither is multiplied by deltaTime. MovingPlatform uses a kinematic Rigidbody and MovePosition with fixedDeltaTime, preserving the lecture's position-based example without using it for the dynamic jumping player. |
| Colliders and physics materials | Walk into SolidObstacle; inspect PlayerNoFriction and Bouncy on their colliders. The ball falls and bounces. |
| Static flags | Ground and fixed blocks use Batching Static. Player, coins, ball and moving platform do not. No light baking or occlusion bake is required by this demo. |
| Collision callbacks | Use the lecture explanation or add a short callback live when covering it. Optional logging and the empty Stay callback were removed from Player to keep the movement example short. |
| Trigger callbacks | Green TriggerZone changes the status label on entry/exit; inspect Stay Steps during overlap. Enable Log Events for optional entry/exit Console messages. Coins use OnTriggerEnter. |
| Tags and layers | Player has the Player tag; Coin and TriggerZone use CompareTag. Ground layer filters the player's ground ray; coins use Collectible. |
| Prefabs | Coin.prefab combines a trigger, Coin script and cylinder visual. Change rotationSpeed or material on the prefab and compare instances. A guard prevents duplicate scoring before Destroy completes. |
| Scene loading | Demo and Level2 are in the build scene list. Next level uses LoadSceneMode.Single. BonusArea adds three coins using LoadSceneMode.Additive and contains no camera, player, UI or manager. |
| GameManager and persistence | GameManager.Instance holds score, rejects duplicates in Awake and uses DontDestroyOnLoad. Inspect the surviving manager after switching scenes. Keep this to the singleton already present in the lecture; no extra architecture is introduced. |
| Canvas, TMP, anchors and scaling | Canvas uses Screen Space Overlay, CanvasScaler at 1280 x 720, corner anchors, TMP text and a GraphicRaycaster. Explain other Canvas modes conceptually using the existing slides. |
| UI events | Inspect each Button's persistent On Click listener to DemoUI. Follow the call to GameManager or the additive scene load. |

## Debugging demonstrations

The normal Demo scene is the working reference. Open **Assets/Scenes/Debugging.unity** as the only scene for a prepared three-fault exercise. Diagnose these after covering movement, triggers and UI; do not reveal the causes upfront.

Instructor answers:
1. Player > Player > Speed is 0. Add the temporary movement log below live, hold W to inspect input/speed, then set Speed to 5 during Play mode.
2. The coin at (0, 0.8, -1) has Sphere Collider > Is Trigger disabled. Compare another coin and enable it on this instance without applying the override to the prefab.
3. The visible score stays at 00 although coins disappear. ScoreUI > Score Text is unassigned, and RefreshScore silently returns when it is null. Add a temporary log after score += amount in GameManager.AddScore to show points are awarded, then inspect the UI code/reference. Assign the existing Score text object to ScoreUI > Score Text during Play mode; the label catches up.

Fix all three during Play mode; stopping Play restores the saved faults. R/Restart and N/Next Level leave this exercise for the normal scenes, so avoid them. If fixes were saved outside Play mode, restore the three values above and save Debugging. Normal scenes retain their assigned UI references and the Coin prefab is unchanged. ScoreUI now returns early when scoreText is null; only Debugging deliberately omits that reference. Debugging is not in the build scene list. Created through Unity, with no further runtime or build tests at Alex's request.

The following missing-reference example is optional and is not part of the prepared scene.

### Missing Rigidbody reference

1. Stop Play mode. In Player.Start, comment out only `rb = GetComponent<Rigidbody>();`.
2. Keep Player's Rigidbody component and leave Player > Rb unassigned in the Inspector.
3. Enter Play mode, inspect the exception in Player.FixedUpdate, then inspect the empty field and existing component.
4. Stop Play mode, restore the assignment, allow compilation and rerun. Do not add a null guard merely to hide this intentionally missing required dependency.

### Live debugging: add your own log

Open Debugging and observe that movement fails. Ask whether input is reaching the controller. Stop Play mode, open Player.cs and type this temporary block immediately after the `movement = new Vector3(input.x, 0f, input.y).normalized;` assignment in Update:

```csharp
if (movement != Vector3.zero)
{
    Debug.Log($"Input: {movement}; Speed: {speed}", this);
}
```

Save, allow Unity to compile, enter Play mode and briefly hold W with Game view focused. Use Console Collapse because the log runs each frame while movement input is held. Nonzero input and speed 0 point to Player > Player > Speed. Set it to 5 during Play mode and repeat the same input to see movement resume.

Continue diagnosing the coin and score faults in that run. Stop Play mode afterward to restore the saved Inspector faults, then manually remove the temporary log block and save Player.cs. Script edits persist after Play mode and affect every scene using Player; stopping Play does not remove them. The delivered script deliberately has no built-in movement logging shortcut.

## Reset and scene behaviour

Restart is the full gameplay reset: score 0, Demo reloaded, five main coins restored, additive scene removed, one manager. Next level retains score but recreates that level's coins; this intentionally permits collecting them again and is not a save/progression system. Bonus loading is guarded against duplicates until a single-scene transition unloads it. Stopping and restarting Play mode also resets all runtime state.

Level2 can be opened directly because it includes its own GameManager. BonusArea is an additive supplement and must not be played alone. The moving block demonstrates Rigidbody positioning; it is not a complete passenger-carrying platform controller.

## Build and asset origins

Build Profiles contains Demo, Level2 and BonusArea in that order. Use the Windows target and export to Builds/. Defaults are 1280 x 720 windowed, with background execution enabled for demonstrations alongside the Inspector. No standalone build was tested in this task.

Visuals are Unity primitives and simple course materials. Text uses Unity's supplied TextMeshPro Liberation Sans resources; retain their included licence files. No external artwork or audio was added.

## Verification on 2026-10-03

Scripts compiled and the scenes, prefab and button listeners were created through Unity. The rendered arena and overlay UI were inspected; the Console reported no warnings/errors at the check. Automated checks confirmed reset score/coin count, initial grounding, coin rotation and moving-platform motion before stopping at an input-simulation assertion. That assertion was not treated as proof of a gameplay bug or as a completed automated test suite.

Alex played the demo, reported that it works fine and requested no further testing. That report predates the later Move/Jump action-polling change. Further automated gameplay checks were stopped. Scene persistence, additive loading, every button and the intentional missing-reference fault were not all independently rehearsed to completion by the agent. No build test was performed.

For the later action-polling change, Unity recompilation completed successfully with no errors. Edit-mode inspection confirmed Assets/InputSystem_Actions.inputactions is assigned project-wide, Player/Move is Value/Vector2, Player/Jump is Button, and the compiled Player class has both cached InputAction fields. No Play mode, input simulation, reset rehearsal or build retest was run, respecting the earlier request. Backups and the Editor inspection report are under the teaching workspace's review_work/lecture02-actions-before-20261003-173932/.




## Coding conventions (2026-10-03)

All eight first-party scripts follow the repository [C# guidelines](../../CODING_GUIDELINES.md): Allman braces, one statement per line, private serialized Inspector configuration, grouped settings and separate runtime state. The existing Player.cs rename and its .meta identity were preserved. Field names remain unchanged so scene/prefab values survive access changes. GameManager.Score is the read-only runtime API used by ScoreUI; score remains the serialized backing field. Public UI methods remain for button listeners. The cached rb, score and trigger step count use ordinary private serialized fields for Inspector observation; no Inspector extension package is used.

Editor tooling must use SerializedObject to configure private fields. Earlier review_work generation scripts are historical records, not current reusable tools. The debugging scene's missing Score Text reference intentionally remains unvalidated by attributes so it can be diagnosed live. No Play mode, simulation or build tests were run for this refactor.

## Simplified controller and hierarchy (2026-10-03)

Player focuses on movement and jumping. The earlier direct WASD key calculations were superseded by polling the existing project-wide Move/Jump actions. Cache both references in Start, read Move as a Vector2, normalize its XZ direction and queue Jump in Update, then set velocity/apply the jump in FixedUpdate. Ground checking remains a downward ray. Existing bindings also supply arrows and gamepad input. Lecture 3 covers creating/configuring actions. Optional collision logs, the empty collision callback and automatic fall respawn remain removed. R/Restart remains the reset.

The main scenes now order roots as Main Camera, Player, Lighting, Environment, Coins, Physics Examples, UI, GameManager. Lighting contains lights; Environment contains the floor/walls/obstacles; Physics Examples contains the zone/marker, moving platform and ball; UI contains Canvas and EventSystem. GameManager remains a root for DontDestroyOnLoad. BonusArea has its coins grouped without adding a second camera/player. Parenting preserves world positions and existing component references.

NaughtyAttributes was removed through Package Manager. Open scene edits were backed up before hierarchy changes. No Play mode, simulation or build tests were run for this change.

Lecture slides aligned on 2026-10-03 with current Player input/physics, coin pickup, score UI and the three debugging faults. The 63-slide deck lives in the teaching workspace's 2026 folder. Labelled screenshot slots and classroom rehearsal remain. No additional gameplay testing performed.

2026-10-03 correction: slide 22 retained and replaced with the actual current Player.cs fields, Start and Update excerpt, using Player/Move and Player/Jump Input Actions. Current owner-edited source supersedes the earlier keyboard-polling assumption. Backed up the deck, preserved all other slides, rendered and inspected slide 22 in PowerPoint. Surrounding keyboard-polling slides still need alignment with this source change. No Unity tests run.

2026-10-03: Added slide 17, The existing Input Actions, with verified Player/Move and Player/Jump keyboard/gamepad bindings and a one-minute Editor walkthrough in speaker notes. Configuration details stay in Lecture 3. Deck now has 64 slides; previous slide 22 is now 23. Source backed up; new slide rendered and inspected in PowerPoint. Earlier surrounding keyboard-polling examples remain pending alignment.

## Score display separation — 2026-10-04

ScoreUI owns the score label, caches GameManager in Start and refreshes when the stored score changes. DemoUI retains restart, next-level and bonus controls/status. Both are on Canvas in Demo, Level2 and Debugging; normal scenes have the existing Score label assigned, while Debugging deliberately leaves ScoreUI.scoreText empty. BonusArea has no UI and was unchanged. Existing button listeners were preserved. Compiled types and saved references confirmed through Unity Editor; no Play-mode, simulation or build tests, per Alex’s instruction. The coin-trigger fault remains pending a separately agreed replacement. Slide references to the score component still need renaming from DemoUI to ScoreUI.
