using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class MazeLoader : MonoBehaviour {
	public int mazeRows, mazeColumns;
	public GameObject wall, floor;
	public float size = 2f;
	public int startRow, startColumn,endRow,endColumn;

	private MazeCell[,] mazeCells;

    // Use this for initialization
    void Start()
    {
		if(mazeColumns>0 && mazeRows > 0)
        {
			if (mazeColumns < 30 && mazeRows < 30)//change maximum size here
			{
				if ((startRow <= mazeRows && startColumn <= mazeColumns && startRow > 0 && startColumn > 0) &&(endRow <= mazeRows && endColumn <= mazeColumns && endRow > 0 && endColumn >0) && (endRow!=startRow || endColumn != startColumn))
				{
					InitializeMaze();
                    MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
                    ma.CreateMaze();
                }
				else
				{
					print("I am not like yours to break easily");
				}
			}
            else
            {
				print("I cant handle such big like your mouth");
			}
        }
        else
        {
			print("too small like yours");
        }
    }
	

	private void InitializeMaze() {

		mazeCells = new MazeCell[mazeRows,mazeColumns];

		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {
				mazeCells [r, c] = new MazeCell ();
				if (r == startRow - 1 && c == startColumn - 1)//starting floor creation
				{
					mazeCells[r, c].floor = Instantiate(floor, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
					mazeCells[r, c].floor.name = "Floor " + r + "," + c;
					mazeCells[r, c].floor.GetComponent<Renderer>().material.color = new Color32(45, 255, 0, 255);				
					mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
				}
				else if(r == endRow - 1 && c == endColumn - 1)//end floor creation
                {
					//Color t = new  Color32(0,0,255,);
					mazeCells[r, c].floor = Instantiate(floor, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
					mazeCells[r, c].floor.name = "Floor " + r + "," + c;
					mazeCells[r, c].floor.GetComponent<Renderer>().material.color = new Color32(255, 50,0,255);
					mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
				}
				else
				{
					mazeCells[r, c].floor = Instantiate(floor, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
					mazeCells[r, c].floor.name = "Floor " + r + "," + c;
					mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
				}

				if (c == 0) {
					mazeCells[r,c].westWall = Instantiate (wall, new Vector3 (r*size, 0, (c*size) - (size/2f)), Quaternion.identity) as GameObject;
					mazeCells [r, c].westWall.name = "West Wall " + r + "," + c;
				}

				mazeCells [r, c].eastWall = Instantiate (wall, new Vector3 (r*size, 0, (c*size) + (size/2f)), Quaternion.identity) as GameObject;
				mazeCells [r, c].eastWall.name = "East Wall " + r + "," + c;

				if (r == 0) {
					mazeCells [r, c].northWall = Instantiate (wall, new Vector3 ((r*size) - (size/2f), 0, c*size), Quaternion.identity) as GameObject;
					mazeCells [r, c].northWall.name = "North Wall " + r + "," + c;
					mazeCells [r, c].northWall.transform.Rotate (Vector3.up * 90f);
				}

				mazeCells[r,c].southWall = Instantiate (wall, new Vector3 ((r*size) + (size/2f), 0, c*size), Quaternion.identity) as GameObject;
				mazeCells [r, c].southWall.name = "South Wall " + r + "," + c;
				mazeCells [r, c].southWall.transform.Rotate (Vector3.up * 90f);
			}
		}
	}
}
