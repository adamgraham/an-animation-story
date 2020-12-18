using UnityEngine;
using System.Collections;

public sealed class GameObjectGridSystem : MonoBehaviour 
{
	[Header("Grid Settings")]

	public GameObjectGrid gridPrefab;

	public UInt3 amountOfGrids;
	
	public Vector3 padding = Vector3.zero;

	public int generationsPerFrame = int.MaxValue;

	public bool autoDestroyRemovedGrids = true;

	private int _gridsGenerated;
	private bool _generating;

	[Header("Optional Settings")]

	public Transform activationSource;
	public int maxActiveGrids = int.MaxValue;

	private GameObjectGrid[] _gridSystem;
	private GameObjectGrid[] gridSystem
	{
		get 
		{
			if (_gridSystem == null) {
				_gridSystem = new GameObjectGrid[this.amountOfGrids.x * this.amountOfGrids.y * this.amountOfGrids.z];
			}

			return _gridSystem;
		}
	}

	private void Start()
	{
		Regenerate();
	}

	private void Update()
	{
		if (_generating) {
			return;
		}

		GameObjectGrid[] allGrids = (GameObjectGrid[])this.gridSystem.Clone();
		
		if (this.activationSource != null) 
		{
			allGrids.Sort(delegate(GameObjectGrid lhs, GameObjectGrid rhs) {
				if (lhs == null && rhs == null) return 0;
				else if (lhs == null) return -1;
				else if (rhs == null) return 1;
				else {
					float lhsDistance = Vector3.Distance(lhs.gameObject.transform.position, this.activationSource.position);
					float rhsDistance = Vector3.Distance(rhs.gameObject.transform.position, this.activationSource.position);
					return lhsDistance.CompareTo(rhsDistance);
				}
			});
		}

		for (int i = 0; i < allGrids.Length; i++) 
		{
			GameObjectGrid grid = allGrids[i];
			
			if (grid != null) {
				grid.gameObject.SetActive(i < this.maxActiveGrids || this.activationSource == null);
			}
		}
	}

	public void Regenerate()
	{
		RemoveAllGrids();

		if (this.gridPrefab == null || this.amountOfGrids.volume == 0) {
			return;
		}

		StartCoroutine(Generate());
	}

	private IEnumerator Generate()
	{
		_generating = true;
		_gridsGenerated = 0;

		Vector3 position = Vector3.zero;
		Vector3 size = new Vector3(this.amountOfGrids.x, this.amountOfGrids.y, this.amountOfGrids.z);
		Vector3 gridSize = new Vector3(this.gridPrefab.gridSize.x, this.gridPrefab.gridSize.y, this.gridPrefab.gridSize.z);

		float gridX = (gridSize.x * this.gridPrefab.occupantSize.x) + ((gridSize.x - 1.0f) * this.gridPrefab.padding.x);
		float gridY = (gridSize.y * this.gridPrefab.occupantSize.y) + ((gridSize.y - 1.0f) * this.gridPrefab.padding.y);
		float gridZ = (gridSize.z * this.gridPrefab.occupantSize.z) + ((gridSize.z - 1.0f) * this.gridPrefab.padding.z);

		float gridHalfX = (gridX * 0.5f);
		float gridHalfY = (gridY * 0.5f);
		float gridHalfZ = (gridZ * 0.5f);

		float maxX = size.x * gridHalfX;
		float maxY = size.y * gridHalfY;
		float maxZ = size.z * gridHalfZ;

		float minX = -maxX;
		float minY = -maxY;
		float minZ = -maxZ;

		float offsetX = (((size.x - 1.0f) * this.padding.x) * -0.5f) + gridHalfX;
		float offsetY = (((size.y - 1.0f) * this.padding.y) * -0.5f) + gridHalfY;
		float offsetZ = (((size.z - 1.0f) * this.padding.z) * -0.5f) + gridHalfZ;
		
		int volume = (int)this.amountOfGrids.volume;
		int currentIteration = 0;
		int generationsPerFrame = (int)((float)volume * Time.maximumDeltaTime);
		generationsPerFrame = generationsPerFrame > 0 ? generationsPerFrame : int.MaxValue;

		for (uint x = 0; x < this.amountOfGrids.x; x++)
		{
			float fx = (float)x;

			for (uint y = 0; y < this.amountOfGrids.y; y++)
			{
				float fy = (float)y;

				for (uint z = 0; z < this.amountOfGrids.z; z++) 
				{
					float fz = (float)z;

					position.x = Mathf.Lerp(minX, maxX, fx / size.x) + (this.padding.x * fx) + offsetX;
					position.y = Mathf.Lerp(minY, maxY, fy / size.y) + (this.padding.y * fy) + offsetY;
					position.z = Mathf.Lerp(minZ, maxZ, fz / size.z) + (this.padding.z * fz) + offsetZ;

					GameObjectGrid grid = Instantiate(this.gridPrefab.gameObject, this.transform).GetComponent<GameObjectGrid>();
					grid.gameObject.SetActive(true);
					grid.gameObject.transform.localPosition = position;
					grid.generationsPerFrame = this.generationsPerFrame / volume;
					grid.autoGenerateOnStart = false;
					grid.Regenerate(OnGridGenerated);
					
					StoreGrid(grid, x, y, z);

					if (++currentIteration % generationsPerFrame == 0) {
						yield return new WaitForEndOfFrame();
					}
				}
			}
		}
	}

	private void OnGridGenerated(bool success)
	{
		if (success) {
			_gridsGenerated++;
		}

		if (_gridsGenerated >= this.amountOfGrids.volume) {
			_generating = false;
		}
	}

	public GameObjectGrid GetGrid(uint x, uint y, uint z)
	{
		uint index = x + this.amountOfGrids.x * (y + this.amountOfGrids.y * z);

		if (index >= this.gridSystem.Length) {
			return null;
		}
		
		return this.gridSystem[index];
	}

	public void StoreGrid(GameObjectGrid grid, uint x, uint y, uint z)
	{
		uint index = x + this.amountOfGrids.x * (y + this.amountOfGrids.y * z);

		if (index >= this.gridSystem.Length) {
			return;
		}

		RemoveGrid(x, y, z);

		this.gridSystem[index] = grid;
	}

	public void RemoveGrid(uint x, uint y, uint z, bool? destroy = null)
	{
		GameObjectGrid grid = GetGrid(x, y, z);

		if (grid == null) {
			return;
		}

		if (destroy ?? this.autoDestroyRemovedGrids) {
			Destroy(grid.gameObject);
		}

		uint index = x + this.amountOfGrids.x * (y + this.amountOfGrids.y * z);
		this.gridSystem[index] = null;
	}

	public void RemoveAllGrids(bool? destroyGrids = null)
	{
		if (destroyGrids ?? this.autoDestroyRemovedGrids) {
			DestroyAllGrids();
		}

		_gridSystem = null;
	}

	private void DestroyAllGrids()
	{
		for (int i = 0; i < this.gridSystem.Length; i++)
		{
			GameObjectGrid grid = this.gridSystem[i];

			if (grid != null) {
				Destroy(grid.gameObject);
			}
		}
	}

}
