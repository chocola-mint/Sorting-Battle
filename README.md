# Sorting-Battle

![](/Media/logo.png)

Sorting Battle is an open-source competitive puzzle game similar to Tetris Attack. Its original purpose is to design an end-to-end reinforcement learning (RL) environment where the trained models can have practical use in the game.

The RL models are trained using the [Sorting Battle Gym](https://github.com/jerry20091103/Sorting-Battle-Python), which aims to achieve feature parity with the [SortGame Core](/Assets/Scripts/Game/Core/). This is achieved through code tracing and implementing the same unit tests.

Refer to our [presentation slides](https://docs.google.com/presentation/d/1EthJHoBGDKr_O8OhGpC6bvZwa7gPInu2nyNB6ULWm18/edit?usp=sharing) for an overview.

## Installation
You need the following:
* Unity 2021.3.x (If contributing, you'll need [the same Unity version](/ProjectSettings/ProjectVersion.txt) this repository is using.)
    * You can install Unity [here](https://unity.com/download).

## Repository Walkthrough
Here are the most important directories in the [Assets](/Assets/) directory:
* [Scripts](/Assets/Scripts/): Contains the C# source code for the entire game.
    * [Game](/Assets/Scripts/Game/): Contains gameplay code. Note that internally the game is referred to as "SortGame".
        * [Core](/Assets/Scripts/Game/Core/): Contains core game logic. This part of the codebase is intentionally designed to avoid using Unity-specific data structures. It does still use Unity's Random state, but that can be replicated elsewhere easily.
        * [UI](/Assets/Scripts/Game/UI/): Contains components used by the UI system, which powers various menus in the game.
        * [Sound](/Assets/Scripts/Game/Sound/): Contains audio components and ScriptableObjects that make it easier to manage and play audio assets.
    * [Utils](/Assets/Scripts/Utils/): Contains utility code used by the gameplay code. Uses the SortGame namespace.
    * [ChocoUtils](/Assets/Scripts/ChocoUtil/): Fork of CHM's [personal Unity utility repository](https://github.com/chocola-mint/ChocoUtil). Its main purpose here is to provide coroutine utilities.
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

## FAQ
### Why use RL? Wouldn't more traditional [search algorithms](https://en.wikipedia.org/wiki/Search_algorithm) perform better?
Our decision to use RL is mostly due to three reasons:
1. It is hard to define a good heuristic for *Sorting Battle*.
   * One class of search algorithms aims to optimize a heuristic function greedily using algorithms like [A*](https://en.wikipedia.org/wiki/A*_search_algorithm). However, we've found it difficult to define such a heuristic function here. 
   * For example, one might greedily try to clear all lines whenever possible, but clearing each line can change the rest of the board, removing other lines.
   * If we were to "rate" each board's state with a heuristic function instead: One might punish taller boards, but taller boards also allow longer vertical lines, so it's hard to say that taller boards are definitely not preferred.
2. Search algorithms have a hard time dealing with *real-time* games in general, compared to turn-based games like Chess or Go.
   * You usually find search algorithms in *planning* an optimal solution from the current state. However, in our game, informative game states are scattered sparsely across the time domain. This means search algorithms must compute a lot of simulation steps to get a good solution.
   * For stochastic algorithms like [Monte Carlo Tree Search (MCTS)](https://en.wikipedia.org/wiki/Monte_Carlo_tree_search), it may also take a lot of steps for each simulated game to end. The legal action space for each step can also be very large, especially if there are a lot of number blocks on the board, increasing the width of the search tree. And so, MTCS would have a hard time converging to a good solution too, not to mention the performance concerns that come with maintaining such a big search tree.
3. RL algorithms are easier to implement and maintain compared to sophisticated search algorithms that rely on specifically-tuned heuristics.

Our RL model can make decisions almost instantly without the memory overhead of a search tree. We believe that it's ultimately the best choice for our application.

However! Our C# framework can still support search algorithm-based implementations, theoretically. Such an implementation would have to inherit from [AIController](/Assets/Scripts/Game/Controllers/AIController.cs). Feel free to submit a pull request if you managed to design a good search algorithm to play our game!

### Why did you not use ML-Agents Toolkit? Wouldn't it be more convenient?

1. ML-Agents Toolkit requires RL developers to install Unity, which can be annoying if you don't usually use Unity.
2. ML-Agents Toolkit's training process is much more inefficient compared to a pure Python implementation.
   * The training process for ML-Agents Toolkit involves either using the Unity Editor *or* a separate build of the game (which can be headless in our case), and using a communication bridge to marshall data (input/output) from and to the Python runtime (which would contain the RL model).
   * Whatever the case, the entire Unity runtime must be included in the training process, which involves animating objects that the RL model would not care about.
   * And of course, interprocess communication across C# and Python can also hurt performance.
3. ML-Agents Toolkit leaves less space for customization.
   * This project started as a term project for a Machine Learning course, and we wanted finer control on the model's architecture. ML-Agents Toolkit's built-in models cannot be modified (although it does happen to have a PPO implementation), so we had to use Python one way or another.

But of course, now that this project has graduated into an open source project, this is no longer a hard requirement. See [Issue #2](https://github.com/chocola-mint/Sorting-Battle/issues/2).

## Contributing

It is recommended that you try out the [open issues](https://github.com/chocola-mint/Sorting-Battle/issues) first.

In general, please try to mimic the coding style in the existing codebase, and remember to use the [Test Runner window](https://docs.unity3d.com/Manual/testing-editortestsrunner.html) to detect regressions before submitting pull requests.

## License

* **Unless specified otherwise in its own directory**, everything under the **[Assets](/Assets/)** folder is licensed under the **MIT license**.
* Notable exceptions:
    * The [TextMesh Pro](/Assets/TextMesh%20Pro/) directory uses the [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license) instead. Unfortunately by design we are forced to include the directory.
    * [ChocoUtil](/Assets/Scripts/ChocoUtil/) is a hard-fork of CHM's personal Unity utilities available [here](https://github.com/chocola-mint/ChocoUtil), and inherits the original MIT license.
    * [SortGame.Core](/Assets/Scripts/Game/Core/) implements fundamental game logic and is specifically licensed under **GNU GPL V3**.
    * This project uses a number of [external assets](/Assets/External/) (all of which uses some variant of the Creative Commons license that permits redistribution). Their licenses are attached in their own directories.
    * This project has dependencies over other open-source projects. For details, refer to the [packages](/Packages/packages-lock.json) metadata.
