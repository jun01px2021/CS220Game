# Project Title: 2048! Multi-mode Variants

## Overview

This project is a command-line puzzle game based on the classic game **2048**.
The player moves number tiles on a square board by using the arrow keys or `W/A/S/D`.
When tiles slide, they may merge according to the rule of the selected game mode.

Unlike the original 2048, this project provides four different game modes:

1. **2's Mode** - original 2048-style game
2. **3's Mode** - tiles merge in groups of three
3. **Fibonacci Mode** - tiles merge according to the Fibonacci sequence
4. **Prime Mode** - tiles merge using prime-number rules

The goal is to reach the target number of the selected mode before the board becomes full and no more valid moves remain.

---

## How to Run

This project assumes that you can already install and run .NET!

### Step 1. Clone the Repository

Use your terminal to clone the repository to your local machine.

### Step 2. Check the Project Files

Once downloaded, the repository folder should contain these main files:

```text
CS220Game.fsproj
Board.fs
Program.fs
README.md
```

* `CS220Game.fsproj` is the F# project file.
* `Board.fs` contains the board engine, movement logic, tile spawning, and board printing.
* `Program.fs` contains the game modes, merge rules, score system, input handling, win/lose conditions, and main game loop.

### Step 3. Run the Game

Run the game by typing "dotnet run" in console.  

## What You Will See First

After running `dotnet run`, the terminal will show the game mode selection text, with the following:

```text
Multi-Mode 2048
Choose a game mode:
1. 2's Mode
2. 3's Mode
3. Fibonacci Mode
4. Prime Mode
Enter 1, 2, 3, or 4:
```

Then, you should enter one of the numbers `1`, `2`, `3`, or `4` and press Enter. The game will run the corresponding mode for you. For example, entering "1" will start **2's Mode**.

---

## Controls

During the game, use either the arrow keys or `W/A/S/D`.

```text
Up:    Up Arrow or W
Down:  Down Arrow or S
Left:  Left Arrow or A
Right: Right Arrow or D
Quit:  Q
```

If an invalid key is pressed, the game ignores it and waits for another input.

---

## What the Game Screen Looks Like

After choosing a mode, the game screen will look similar to this:

```text
2048 (2's)
Score: 0    Goal: 2048
Use arrow keys or WASD to move. Press Q to quit.

     .     .     .     .
     .     2     .     .
     .     .     .     .
     .     .     2     .
```

A dot `.` means the cell is empty.
A number means there is a tile in that cell.

After each valid move:

1. All tiles slide in the selected direction.
2. Valid merges happen.
3. The score is updated.
4. A new tile appears at a random empty position.
5. The updated board is printed again.

---

## Details and Logistics of Each Game Mode

## 1. 2's Mode

### Basic Information

```text
Game Name: 2048 (2's)
Board Size: 4 x 4
Goal: To have a tile with number "2048" on the board.
```

### Spawn Rule

At the beginning, two tiles are created.

During the game:

* Before a tile of value 32 appears:

  * `2` appears with 90% probability.
  * `4` appears with 10% probability.

* After a tile of value 32 or higher appears:

  * `2` appears with 60% probability.
  * `4` appears with 40% probability.

### Merge Rule

Two tiles merge if they have the same value.

Examples:

```text
2 + 2 -> 4
4 + 4 -> 8
8 + 8 -> 16
```

### Win Condition

The player wins when a tile with value `2048` or higher appears on the board.

### Lose Condition

The player loses when the board is full and no valid moves remain.

---

## 2. 3's Mode

### Basic Information

```text
Game Name: 2187 (3's)
Board Size: 6 x 6
Goal: To have a tile with number "2187" on the board.
```

### Spawn Rule

At the beginning, two tiles are created.

Before a tile of value 81 appears:

* `1` appears with 70% probability.
* `3` appears with 30% probability.

After a tile of value 81 or higher appears:

* `1` appears with 50% probability.
* `3` appears with 30% probability.
* `9` appears with 20% probability.
* There is a 40% chance that two tiles spawn instead of one.

### Merge Rule

Three identical adjacent tiles merge into one tile.

Examples:

```text
1 + 1 + 1 -> 3
3 + 3 + 3 -> 9
9 + 9 + 9 -> 27
```

### Win Condition

The player wins when a tile with value `2187` or higher appears on the board.

### Lose Condition

The player loses when the board is full and no valid triple merges remain.

---

## 3. Fibonacci Mode

### Basic Information

```text
Game Name: 2584 (Fibonacci)
Board Size: 4 x 4
Goal: To have a tile with number "2584" on the board.
```

### Spawn Rule

At the beginning, two tiles are created.

Before a tile of value 89 appears:

* `1` appears with 90% probability.
* `2` appears with 10% probability.

After a tile of value 89 or higher appears:

* `1` appears with 70% probability.
* `2` appears with 30% probability.

### Merge Rule

Two tiles merge if they are consecutive Fibonacci numbers.
The special case `1 + 1 -> 2` is also allowed so that the game can start naturally.

Examples:

```text
1 + 1 -> 2
1 + 2 -> 3
2 + 3 -> 5
3 + 5 -> 8
5 + 8 -> 13
```

### Win Condition

The player wins when a tile with value `2584` or higher appears on the board.

### Lose Condition

The player loses when the board is full and no valid Fibonacci merges remain.

---

## 4. Prime Mode

### Basic Information

```text
Game Name: 199 (Prime)
Board Size: 5 x 5
Goal: To have a tile with number "199" on the board.
```

### Spawn Rule

At the beginning, two tiles are created.

Before a tile greater than 20 appears:

* `2` appears with 70% probability.
* `3` appears with 30% probability.

After a tile greater than 20 appears:

* `2` appears with 50% probability.
* `3` appears with 30% probability.
* `5` appears with 20% probability.

### Merge Rule

Two tiles merge if either of the following conditions is true:

1. The two tiles have the same value.
2. The sum of the two tiles is a prime number.

The resulting tile is the sum of the two tiles.

Examples:

```text
2 + 2 -> 4
2 + 3 -> 5
3 + 4 -> 7
5 + 5 -> 10
10 + 13 -> 23
```

### Win Condition

The player wins when a tile with value `199` or higher appears on the board.

### Lose Condition

The player loses when the board is full and no valid same-number or prime-based merges remain.

---

## Scoring

Whenever tiles merge, the score increases by the value of the resulting merged tile.

For example:

```text
2 + 2 -> 4
```

adds `4` points to the score.

In 3's Mode:

```text
3 + 3 + 3 -> 9
```

adds `9` points to the score.

---

## Ending the Game

The game ends in one of three ways.

### 1. Win

If the player reaches the target number of the selected mode, the game prints a message like:

```text
You win! You reached 2048.
```

The target number depends on the selected mode.

```text
2's Mode:        2048
3's Mode:        2187
Fibonacci Mode:  2584
Prime Mode:      199
```

### 2. Game Over

If the board is full and no valid moves remain, the game prints:

```text
Game over! No valid moves remain.
```

### 3. Quit

The player can quit manually by pressing `Q`.

The game then prints:

```text
Quit game.
```

---

## Example Interaction

Suppose the user runs:

```bash
dotnet run
```

The game shows:

```text
Multi-Mode 2048
Choose a game mode:
1. 2's Mode
2. 3's Mode
3. Fibonacci Mode
4. Prime Mode
Enter 1, 2, 3, or 4:
```

The user enters:

```text
1
```

The game starts 2's Mode and prints a 4 x 4 board:

```text
2048 (2's)
Score: 0    Goal: 2048
Use arrow keys or WASD to move. Press Q to quit.

     .     .     .     .
     2     .     .     .
     .     .     2     .
     .     .     .     .
```

If the user presses the right arrow key, all tiles slide to the right.
If two matching tiles collide, they merge.
Then a new tile appears randomly, and the updated board is printed.

The game repeats this process until the player wins, loses, or presses `Q`.

---

## Final Remarks / Notes for Peer Evaluation!

1. The project runs using `dotnet run`.
2. The game mode selection screen appears.
3. Each of the four modes can be selected.
4. Tiles move correctly using arrow keys or `W/A/S/D`.
5. Tiles merge according to the selected mode's rule.
6. The score increases after merges.
7. New tiles spawn after valid moves.
8. The game detects win and lose conditions.
9. Pressing `Q` exits the game.

## LLM Usage

I completed this project with the help of ChatGPT, a widely known LLM. The main concept, planning, and game design decisions were my own. For example, I decided to create four variants of 2048 instead of only implementing the original version. I also designed the rules for each game mode, including the 2's mode, 3's mode, Fibonacci mode, and Prime mode.

I also planned the overall structure of the project, including the idea of separating the board-related logic into a separate `Board.fs` file. My goal was to define a common movement system that could be shared by all four game modes, while allowing each mode to have its own merge rule, spawn rule, board size, and win condition.

I used ChatGPT mainly as a programming assistant while implementing and polishing the project. Specifically, I used it to help with parts such as formatting the board output in the terminal, improving the game screen text, organizing some helper functions, and implementing the Fibonacci mode logic. I also used it to check whether the project structure and README instructions were clear enough for peer evaluation.

Overall, the design direction and major gameplay ideas were decided and implemented by me, while ChatGPT was used as a support tool for debugging, code organization, and documentation polishing.

