using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;
using System.Collections.Generic;

public class MazeSpawner : MonoBehaviour {
	
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject EnemyPrefab = null;
	public GameObject FinishPrefab = null;

	private BasicMazeGenerator mMazeGenerator = null;
	public NavMeshSurface _navMeshSurface;
	int enemyProbabilityStart;
	int enemyProbabilityEnd;

	public List<GameObject> cellsList = new List<GameObject>();
	public List<int> uniqueEnemyIndexes;

	int MaxEnemies = 0;

	void Start () {

		MaxEnemies = (Rows / 2);
		enemyProbabilityStart = Rows;
		enemyProbabilityEnd = (Rows * Columns) - Rows;
		uniqueEnemyIndexes = GenerateUniqueRandomNumbers(enemyProbabilityStart, enemyProbabilityEnd, MaxEnemies);
		GameObject floorObject;
		mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
		mMazeGenerator.GenerateMaze ();
		int currentCellIndex = 0;
		cellsList.Clear();
		for (int row = 0; row < Rows; row++) {
			for(int column = 0; column < Columns; column++){
				float x = column*(CellWidth+(AddGaps?.2f:0));
				float z = row*(CellHeight+(AddGaps?.2f:0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
				GameObject tmp;
				floorObject = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
				cellsList.Add(floorObject);
				currentCellIndex++;

				floorObject.transform.parent = transform;
				if (cell.WallRight){
					tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,90,0)) as GameObject;// right
					tmp.transform.parent = transform;
				}
				if(cell.WallFront){
					tmp = Instantiate(Wall,new Vector3(x,0,z+CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,0,0)) as GameObject;// front
					tmp.transform.parent = transform;
				}
				if(cell.WallLeft){
					tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,270,0)) as GameObject;// left
					tmp.transform.parent = transform;
				}
				if(cell.WallBack){
					tmp = Instantiate(Wall,new Vector3(x,0,z-CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,180,0)) as GameObject;// back
					tmp.transform.parent = transform;
				}
				if(row == Rows-1 && column == Columns - 1){
					tmp = Instantiate(FinishPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
					tmp.transform.parent = transform;
					_navMeshSurface.BuildNavMesh();
				}
			}
		}
		SpawnEnemies();
		if(Pillar != null){
			for (int row = 0; row < Rows+1; row++) {
				for (int column = 0; column < Columns+1; column++) {
					float x = column*(CellWidth+(AddGaps?.2f:0));
					float z = row*(CellHeight+(AddGaps?.2f:0));
					GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}
	}

	List<int> GenerateUniqueRandomNumbers(int start, int end, int count)
	{
		if (count > end - start + 1)
		{
			return null;
		}

		HashSet<int> uniqueNumbers = new HashSet<int>();
		List<int> uniqueRandomNumbers = new List<int>();

		while (uniqueNumbers.Count < count)
		{
			int randomNumber = Random.Range(start, end + 1);

			if (!uniqueNumbers.Contains(randomNumber))
			{
				uniqueNumbers.Add(randomNumber);
				uniqueRandomNumbers.Add(randomNumber);
			}
		}

		return uniqueRandomNumbers;
	}

	void SpawnEnemies()
    {
		for(int i = 0; i < uniqueEnemyIndexes.Count; i++)
        {
			GameObject enemyObject = Instantiate(EnemyPrefab, cellsList[uniqueEnemyIndexes[i]].transform.position, Quaternion.identity) as GameObject;
			AssignPositions(cellsList[uniqueEnemyIndexes[i]], enemyObject);
			enemyObject.transform.parent = transform;
		}
	}		

	void AssignPositions(GameObject FloorCollider, GameObject Enemy)
    {
		Collider collider = FloorCollider.GetComponent<Collider>();
		if (collider != null)
		{
			Bounds bounds = collider.bounds;
			Vector3 center = bounds.center;
			Vector3 extents = bounds.extents;

			float minX = center.x - extents.x;
			float minY = center.y - extents.y;
			float minZ = center.z - extents.z;

			float maxX = center.x + extents.x;
			float maxY = center.y + extents.y;
			float maxZ = center.z + extents.z;

			Enemy enemyObject = Enemy.GetComponent<Enemy>();
			enemyObject.startPoint = new Vector3(minX, enemyObject.transform.localPosition.y, minZ);
			enemyObject.endPoint = new Vector3(maxX, enemyObject.transform.localPosition.y, maxZ);
			enemyObject.IsPathAssigned = true;
		}
	}
}
