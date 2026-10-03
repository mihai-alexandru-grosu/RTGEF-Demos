# RTGEF Unity Lecture05

Starting point for audio, particles, Shader Graph and decals. Lesson content has not been implemented.

## Open and run

Use **Unity 6000.3.25f1**. Open `Assets/Scenes/Demo.unity`, then enter Play mode. Expect a stationary blue cube on a grey ground plane. No gameplay controls, scripts or intentional faults have been added.

During Play mode, move the cube in the Inspector, then stop Play mode to restore its saved position `(0, 0.5, 0)`. This is the baseline reset procedure; lesson-specific reset steps will be added with the demo.

## Configuration

URP **17.3.0**, Input System **1.20.0** (new input only), Unity Pipeline **0.8.0-exp.1**. Remaining starter dependencies are recorded in `Packages/manifest.json` and `packages-lock.json`. The existing default Input Actions asset is retained.

The sole build scene is Demo. Use Windows in Build Profiles and export to `Builds/`; default output is 1280 x 720 windowed. No standalone build has been tested.

Assets are Unity starter-template assets, Unity primitives and three course materials. No third-party art or audio was added.

## Verification

2026-10-03: copied from the [3D template](../Templates/Basic3D/README.md), whose rendered Play mode and reset were checked in Unity with a clean Console. This copy imported successfully in Unity; its scene, camera, URP shader, starting cube position, build-scene entry and disconnected Cloud state were validated. Product name: RTGEF Unity Lecture05. No separate Play mode rehearsal of this copy or standalone build was performed. This is not yet a classroom-ready lecture demo.
