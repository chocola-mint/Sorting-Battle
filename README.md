# Sorting-Battle
 
## Game State Architecture

Diagram: [SortGame Architecture](/SortGame%20Architecture.pdf).

Relevant files can be found [here](/Assets/Scripts/Game/).

To trace the code, start from [GameBoardState](/Assets/Scripts/Game/GameBoardState.cs), which represents a playable game instance's state. (Entirely separated from Unity's components)

The important classes are:
* [GameBoardState](/Assets/Scripts/Game/GameBoardState.cs): Represents a board. A game can have multiple boards.
* [GameGridState](/Assets/Scripts/Game/GameGridState.cs): Represents a grid. Exposes some public methods for manipulating the grid, sometimes returning lists of swaps needed to execute each command. A board only has one grid.
* [GameControllerState](/Assets/Scripts/Game/GameControllerState.cs): Represents a player's inputs to the board. Has public methods that represent the actions a player (or agent) can take, like selecting and swapping.
* [GameScoreState](/Assets/Scripts/Game/GameScoreState.cs): Represents a board's current scoring state. This includes info such as the current score and combo. GameScoreState also implements the scoring algorithm.