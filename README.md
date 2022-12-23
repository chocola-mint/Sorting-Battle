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