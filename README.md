# SimpleSmeeborg Documentation
Kodable Game Developer Test

[Instructions](https://docs.google.com/document/d/1H8M92RWBqTi0OufbuYja6BzQNnknbib8gbpJHlAts34/edit)

## Overview

This Unity application attempts to parse the contents of a .txt file based on the ASCII maze format provided in the instructions.
If successful, it renders a corresponding maze as a grid of simple sprites. Next, it uses the A* search algorithm to find a path from the
top left of the maze (0,0) to the bottom right (max, max). If one is found, the application animates a sprite following the path from start to finish. 

## Format

The application expects the ASCII maze to consist of a grid of cells. The general format of an individual cell is as follows:

```
+--+
|   
+--+
```

* The `--` characters together represent a wall on the north or south side of the cell
* The `|` character represents a wall on the east or west side of the cell
* The `+` characters represent the corners
* The ASCII characters are expected to have a uniform row length and a uniform column length (no standalone rooms protruding from the side of the maze)
* If the inputted ASCII does not match these expectations, the application will likely produce unspecified results

## Flow

### `MazeLoader` (MonoBehaviour)

* Holds a serialized reference to `maze.txt`, the provided example ASCII maze file
* Initiates the process of parsing the ASCII and creating the `Maze` instance
* Creates the `CellBehaviour` instances that visually represent the maze to the user
* Invokes `OnMazeInitialized`, listened for by `CameraAdjuster` and `FindPathAStar`

### `Maze`

* Passes the ASCII string to the `AsciiMaze` constructor
* Creates the 2D `Cell` matrix by requesting instances from `AsciiMaze`
* Serves valid `Cell` neighbors to `FindPathAStar` later

### `AsciiMaze`

* Creates a 2D `char` matrix from the inputted ASCII string
* Provides `Maze` with `Cell` instances
* Determines from which directions a `Cell` can be entered based on surrounding `char` values

### `Cell`

* Represents a room in the maze
* Holds a room's coordinates, world position and from which cardinal directions it is accessible

### `CellBehaviour` (MonoBehaviour)

* Responsible for the room's visuals
* Toggles on an appropriate icon if the cell is the start or finish
* Sets the `SpriteRenderer` Material's shader uniforms to display the room's walls, if any

### `FindPathAStar` 

* Attempts to find the shortest possible path through the maze
* Finds the costs of a room's neighbors as determined by the A* distance calculations
* Manages the open and closed lists of `PathNode` instances, used to track with routes have been tested yet
* If the finish is found, chains together a complete path by retracing steps through `PathNode` parent references

### `PathNode`

* Holds the current A* costs associated with a room 
* Stores the parent room that was selected before this room 

### `Character` (MonoBehaviour)

* Caches the completed path if `FindPathAStar` is successful
* Listens for a `UnityEvent` (invoked by `ShowSolutionButton`) to begin an animation
* Leverages the DOTween package to tween from room to room along the path

### `ShowSolutionButton` (MonoBehaviour)

* Invokes a `UnityEvent` when the user clicks the button, hiding the UI and triggering the `Character` instance's animation
* Hides the UI in the event that a path was not found

## Additional Scripts

### `BoolExtensions

* Provides a small method to convert the room access booleans to integers for the shader uniforms

### `CameraAdjuster` (MonoBehaviour)

* Sets the size of the `Camera` and the position of its `GameObject`, after the maze has been initialized

### `CellExtensions`

* A set of extension methods to check the directional relationship between two `Cell` instances

### `CellType` (enum)

* Used to identify the start and finish rooms

## Improvements

* **UI:** Provide more information to the user through UI, including pathfinding error messages, maze data, etc 
* **Feedback:** Improve the character animation so it's more interesting, design and build a maze completion state, add sound effects that coincide with the character's actions
* **Input Flexibility:** Refactor the input ASCII system so the text file can be received from an abstract source via an interface. Create input interface implementations that allow for the ASCII to be received through an HTTP GET request, read from a local file, loaded from an Addressable asset, etc
* **Format Options:** Expand the ASCII parsing system to accomodate different maze formats
* **Robustness/Error Handling:** Put checks in place to catch small typos, whitespace or other mistakes in the ASCII and design a way to either alert the end user of these discrepancies or handle them silently
* **Maze Generation:** Implement procedural maze generation algorithms to improve testability and level design capabilities. This could be an editor-only feature for the team, a user-facing feature, or both
* **Optimization:** Investigate various optimizations, such as: preferable alternatives to the LINQ usage in the A* class, object pooling for CellBehaviour instances and/or loading the CellBehaviour prefab via Addressables, create a class utilizing a `HashSet<Material>` in an accessible location so `CellBehaviour` instances can share identical `Material` instances between them, when possible
