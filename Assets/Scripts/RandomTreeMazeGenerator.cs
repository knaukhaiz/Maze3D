using UnityEngine;
using System.Collections;

public class RandomTreeMazeGenerator : TreeMazeGenerator {

	public RandomTreeMazeGenerator(int row, int column):base(row,column){
		
	}
	
	protected override int GetCellInRange(int max)
	{
		return Random.Range (0, max+1);
	}
}
