# Sorting-Battle

## Core Architecture

Diagram: [SortGame Architecture](/SortGame%20Architecture.pdf).

Relevant files can be found [here](/Assets/Scripts/Game/).

To trace the code, start from [GameBoardState](/Assets/Scripts/Game/Core/GameBoardState.cs), which represents a playable game instance's state. (Entirely separated from Unity's components)

The important classes are:
* [GameState](/Assets/Scripts/Game/Core/GameState.cs): Represents a game mode. This is an abstract base class that can be extended to handle any game mode. The base class contains a tick-based event scheduler that can run anywhere. For a sample implementation, see [Endless1PGameState](/Assets/Scripts/Game/Core/Endless1PGameState.cs).
* [GameBoardState](/Assets/Scripts/Game/Core/GameBoardState.cs): Represents a board. A game can have multiple boards.
* [GameGridState](/Assets/Scripts/Game/Core/GameGridState.cs): Represents a grid. Exposes some public methods for manipulating the grid, sometimes returning lists of swaps needed to execute each command. A board only has one grid.
* [GameControllerState](/Assets/Scripts/Game/Core/GameControllerState.cs): Represents a player's inputs to the board. Has public methods that represent the actions a player (or agent) can take, like selecting and swapping.
* [GameScoreState](/Assets/Scripts/Game/Core/GameScoreState.cs): Represents a board's current scoring state. This includes info such as the current score and combo. GameScoreState also implements the scoring algorithm.

## License

* Unless specified otherwise in its own directory, all source code under the **[Assets](/Assets/)** folder is licensed under the **[GNU General Public License v3.0](/LICENSE)**. "Source code" includes:
    * Text files written in programming languages: **.cs**, **.js**, etc.
    * Shader graphs.
    * Other Unity-specific assets: Scene data, prefab data, ScriptableObject data.
* Notable exceptions:
    * The [TextMesh Pro](/Assets/TextMesh%20Pro/) directory uses the [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license) instead. Unfortunately by design we are forced to include the directory.
    * [ChocoUtil](/Assets/Scripts/ChocoUtil/) is a hard-fork of CHM's personal Unity utilities available [here](https://github.com/chocola-mint/ChocoUtil), and inherits the original MIT license.
    * This project uses a number of [external assets](/Assets/External/) (all of which uses some variant of the Creative Commons license that permits redistribution). Their licenses are attached in their own directories.
    * This project has dependencies over other open-source projects. For details, refer to the [packages](/Packages/packages-lock.json) metadata.