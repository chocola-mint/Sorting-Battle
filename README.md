# Sorting-Battle

Sorting Battle is an open-source competitive puzzle game similar to Tetris Attack. Its original purpose is to design an end-to-end reinforcement learning (RL) environment where the trained models can have practical use in the game.

The RL models are trained using the [Sorting Battle Gym](https://github.com/jerry20091103/Sorting-Battle-Python), which aims to achieve feature parity with the [SortGame Core](/Assets/Scripts/Game/Core/). This is achieved through code tracing and implementing the same unit tests.

## Repository Walkthrough
Here are the most important directories in the [Assets](/Assets/) directory:
* [Scripts](/Assets/Scripts/): Contains the C# source code for the entire game.
    * [Game](/Assets/Scripts/Game/): Contains gameplay code. Note that internally the game is referred to as "SortGame".
         * [Core](/Assets/Scripts/Game/Core/): Contains core game logic. This part of the codebase is intentionally designed to avoid using Unity-specific data structures. It does still use Unity's Random state, but that can be replicated elsewhere easily.
    * [Utils](/Assets/Scripts/Utils/): Contains utility code used by the gameplay code. Uses the SortGame namespace.
    * [ChocoUtils](/Assets/Scripts/ChocoUtil/): Fork of CHM's [personal Unity utility repository](https://github.com/chocola-mint/Sorting-Battle/issues). Its main purpose here is to provide coroutine utilities.
* [Tests](/Assets/Tests/): Contains unit tests for the entire game. At the moment, only editor tests are implemented because they run very fast compared to play mode tests. (less than 0.5 seconds)
    * [GameTests](/Assets/Tests/GameTests/): Contains tests for [SortGame Core](/Assets/Scripts/Game/Core/). Similar tests are implemented in the [Sorting Battle Gym](https://github.com/jerry20091103/Sorting-Battle-Python) as well.
    * [UtilsTests](/Assets/Tests/UtilsTests/): Contains tests for [SortGame Utils](/Assets/Scripts/Utils/).
* [Prefabs](/Assets/Prefabs/): Contains reusable GameObjects and some ScriptableObject wrappers of said GameObjects. (For example, each [PlayerType](/Assets/Scripts/Game/Controllers/PlayerType.cs) wraps a GameController prefab, so they're put together)
* [Settings](/Assets/Settings/): Contains game setting assets, such as input settings and other ScriptableObjects.
* [Scenes](/Assets/Scenes/): Contains game scenes and scene-related ScriptableObjects.
* [ONNX](/Assets/ONNX/): Contains ONNX neural network assets.
* [Materials](/Assets/Materials/): Contains all assets related to materials (including shaders and textures).
* [Sprites](/Assets/Sprites/): Contains game sprites. Might be removed in the future (refactored into the Prefab folders).
* [External](/Assets/External/): Contains external assets. Their licenses/credits are attributed in their respective directories.

## Contributing

It is recommended that you try out the [open issues](https://github.com/chocola-mint/Sorting-Battle/issues) first.

In general, please try to mimick the coding style in the existing codebase, and remember to use the Test Runner window to detect regressions before submitting pull requests.

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
