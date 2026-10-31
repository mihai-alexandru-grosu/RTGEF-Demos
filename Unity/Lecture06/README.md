# RTGEF Unity Lecture06

Open this folder with Unity **6000.3.25f1**. Packages: URP 17.3.0, AI Navigation 2.0.14, Input System 1.20.0, uGUI 2.0.0 and Pipeline 0.8.0-exp.1.

Open `Assets/Scenes/Demo.unity` and press Play.

- **F1** / Terrain button: click-to-move navigation, collectible ScriptableObject data, terrain layers, trees and details. Buttons select destinations and toggle the carving crate or bridge link. Disabling the link also hides the bridge; enabling it or resetting restores it.
- **F2** / Lighting button: baked area light and probes, mixed sunlight, realtime point/spot lights, global/local Volumes. Buttons toggle lights, global effects and sphere motion.
- **R** / Reset scene: restore the current workshop.
- **Toggle saved text size**: demonstrate PlayerPrefs. **Clear demo preference** removes this demo's saved setting.

Stop Play to restore scene Inspector edits. TerrainData and other asset edits persist; use Undo for those. Build scenes are Demo (F1) and LightingWorkshop (F2). ProfilingWorkshop is retained for later, hidden from navigation and disabled in the build list.
