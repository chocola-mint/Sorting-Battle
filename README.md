# Sorting-Battle

Sorting Battle is an open-source competitive puzzle game similar to Tetris Attack. Its original purpose is to design an end-to-end reinforcement learning (RL) environment where the trained models can have practical use in the game.

The RL models are trained using the [Sorting Battle Gym](https://github.com/jerry20091103/Sorting-Battle-Python), which aims to achieve feature parity with the [SortGame Core](/Assets/Scripts/Game/Core/). This is achieved through code tracing and implementing the same unit tests.

## License

* **Unless specified otherwise in its own directory**, all source code under the **[Assets](/Assets/)** folder is licensed under the **MIT license**. "Source code" includes:
    * Text files written in programming languages: **.cs**, **.js**, etc.
    * Shader graphs.
    * ONNX assets.
    * Other Unity-specific assets: Scene data, prefab data, ScriptableObject data.
* Notable exceptions:
    * The [TextMesh Pro](/Assets/TextMesh%20Pro/) directory uses the [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license) instead. Unfortunately by design we are forced to include the directory.
    * [ChocoUtil](/Assets/Scripts/ChocoUtil/) is a hard-fork of CHM's personal Unity utilities available [here](https://github.com/chocola-mint/ChocoUtil), and inherits the original MIT license.
    * [SortGame.Core](/Assets/Scripts/Game/Core/) implements fundamental game logic and is specifically licensed under **GNU GPL V3**.
    * This project uses a number of [external assets](/Assets/External/) (all of which uses some variant of the Creative Commons license that permits redistribution). Their licenses are attached in their own directories.
    * This project has dependencies over other open-source projects. For details, refer to the [packages](/Packages/packages-lock.json) metadata.