using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

public class HuntAndKillMazeAlgorithm : MazeAlgorithm {

	private int row, column, previousHuntWall, currentRow, currentColumn = 0;

	private bool courseComplete = false;

	public HuntAndKillMazeAlgorithm(MazeCell[,] mazeCells) : base(mazeCells) {
	}

	public override void CreateMaze () {
        HuntAndKill();
	}

    private void HuntAndKill()
    {
        mazeCells [currentRow, currentColumn].visited = true;

        while (!courseComplete)
        {
            Kill(); // Will run until it hits a dead end.
			Hunt(); // Finds the next unvisited cell with an adjacent visited cell. If it can't find any, it sets courseComplete to true.
		}
    }
	private int nextDirection()//choose between procedural and random
    {
        return (Random.Range(1, 5));
        //return (ProceduralNumberGenerator.GetNextNumber());
    }
	private void Kill()
	{
		int previousDirection = 0;
		while (RouteStillAvailable(currentRow, currentColumn))
		{
            int direction = nextDirection();

            if (direction == 1 && CellIsAvailable(currentRow - 1, currentColumn))
			{
				// North
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].northWall);
				DestroyWallIfItExists(mazeCells[currentRow - 1, currentColumn].southWall);
				currentRow--;
			}
			else if (direction == 2 && CellIsAvailable(currentRow + 1, currentColumn))
			{
				// South
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].southWall);
				DestroyWallIfItExists(mazeCells[currentRow + 1, currentColumn].northWall);
				currentRow++;
			}
			else if (direction == 3 && CellIsAvailable(currentRow, currentColumn + 1))
			{
				// east
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].eastWall);
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn + 1].westWall);
				currentColumn++;
			}
			else if (direction == 4 && CellIsAvailable(currentRow, currentColumn - 1))
			{
				// west
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].westWall);
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn - 1].eastWall);
				currentColumn--;
			}
			previousDirection = direction;
			mazeCells[currentRow, currentColumn].visited = true;
		}
        wallRemover(previousDirection);
    }

	//method to remove wall at each stop
    private void wallRemover(int previousDirection)
    {
		//previousDirection -> to keep track of path so we dont destroy a already destroyed wall
		//previousHuntWall -> sometime hunt phase will end without kill phase for those special condition ,we should know which wall was destroyed
		//cellChecker -> check whether the next cell is within maze size

        if (previousDirection != 2 && previousHuntWall != 1 && CellChecker(currentRow - 1, currentColumn))
		{
			// North
			DestroyWallIfItExists(mazeCells[currentRow, currentColumn].northWall);
			DestroyWallIfItExists(mazeCells[currentRow - 1, currentColumn].southWall);
			previousHuntWall = 0;
		}
        else if (previousDirection != 1 && previousHuntWall != 2 && CellChecker(currentRow + 1, currentColumn))
        {
            // South
            DestroyWallIfItExists(mazeCells[currentRow, currentColumn].southWall);
            DestroyWallIfItExists(mazeCells[currentRow + 1, currentColumn].northWall);
			previousHuntWall = 0;
		}
        else if (previousDirection != 4 && previousHuntWall != 3 && CellChecker(currentRow, currentColumn + 1) )
        {
            // east
            DestroyWallIfItExists(mazeCells[currentRow, currentColumn].eastWall);
            DestroyWallIfItExists(mazeCells[currentRow, currentColumn + 1].westWall);
			previousHuntWall = 0;
		}
        else if (previousDirection != 3 && previousHuntWall != 4 && CellChecker(currentRow, currentColumn - 1) )
        {
            // west
            DestroyWallIfItExists(mazeCells[currentRow, currentColumn].westWall);
            DestroyWallIfItExists(mazeCells[currentRow, currentColumn - 1].eastWall);
			previousHuntWall = 0;
		}
    }

	private bool CellChecker(int row, int column)
    {
        if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Hunt()
    {
        courseComplete = true; // Set it to this, and see if we can prove otherwise below!

        for (int r = row; r < mazeRows; r++)
        {
            for (int c = column; c < mazeColumns; c++)
            {
                if (!mazeCells[r, c].visited && CellHasAnAdjacentVisitedCell(r, c))
                {
                    courseComplete = false; // Yep, we found something so definitely do another Kill cycle.
                    currentRow = r;
                    row = r;//stores the last row it processed so it doesnt have to retry from start
                    currentColumn = c;
                    column = c;
                    DestroyAdjacentWall(currentRow, currentColumn);
                    mazeCells[currentRow, currentColumn].visited = true;
                    return; // Exit the function
                }
            }
            column = 0;
        }
    }

    private bool RouteStillAvailable(int row, int column) {
		int availableRoutes = 0;

		if (row > 0 && !mazeCells[row-1,column].visited) {
			availableRoutes++;
		}

		if (row < mazeRows - 1 && !mazeCells [row + 1, column].visited) {
			availableRoutes++;
		}

		if (column > 0 && !mazeCells[row,column-1].visited) {
			availableRoutes++;
		}

		if (column < mazeColumns-1 && !mazeCells[row,column+1].visited) {
			availableRoutes++;
		}

		return availableRoutes > 0;
	}

	private bool CellIsAvailable(int row, int column) {
		if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells [row, column].visited) {
			return true;
		} else {
			return false;
		}
	}

	private void DestroyWallIfItExists(GameObject wall) {
		if (wall != null) {
			GameObject.Destroy (wall);
		}
	}

	private bool CellHasAnAdjacentVisitedCell(int row, int column) {
		int visitedCells = 0;

		// Look 1 row up (north) if we're on row 1 or greater
		if (row > 0 && mazeCells [row - 1, column].visited) {
			visitedCells++;
		}

		// Look one row down (south) if we're the second-to-last row (or less)
		if (row < (mazeRows-2) && mazeCells [row + 1, column].visited) {
			visitedCells++;
		}

		// Look one row left (west) if we're column 1 or greater
		if (column > 0 && mazeCells [row, column - 1].visited) {
			visitedCells++;
		}

		// Look one row right (east) if we're the second-to-last column (or less)
		if (column < (mazeColumns-2) && mazeCells [row, column + 1].visited) {
			visitedCells++;
		}

		// return true if there are any adjacent visited cells to this one
		return visitedCells > 0;
	}

	private void DestroyAdjacentWall(int row, int column) {
		bool wallDestroyed = false;

		while (!wallDestroyed) {
			int direction = nextDirection();

			if (direction == 1 && row > 0 && mazeCells [row - 1, column].visited) {
				DestroyWallIfItExists (mazeCells [row, column].northWall);
				DestroyWallIfItExists (mazeCells [row - 1, column].southWall);
				wallDestroyed = true;
				previousHuntWall = 1;
			}
			else if (direction == 2 && row < (mazeRows-2) && mazeCells [row + 1, column].visited) {
				DestroyWallIfItExists (mazeCells [row, column].southWall);
				DestroyWallIfItExists (mazeCells [row + 1, column].northWall);
				wallDestroyed = true;
				previousHuntWall = 2;
			} 
			else if (direction == 3 && column > 0 && mazeCells [row, column-1].visited) {
				DestroyWallIfItExists (mazeCells [row, column].westWall);
				DestroyWallIfItExists (mazeCells [row, column-1].eastWall);
				wallDestroyed = true;
				previousHuntWall = 3;
			}
			else if (direction == 4 && column < (mazeColumns-2) && mazeCells [row, column+1].visited) {
				DestroyWallIfItExists (mazeCells [row, column].eastWall);
				DestroyWallIfItExists (mazeCells [row, column+1].westWall);
				wallDestroyed = true;
				previousHuntWall = 4;
			}
		}

	}

}
