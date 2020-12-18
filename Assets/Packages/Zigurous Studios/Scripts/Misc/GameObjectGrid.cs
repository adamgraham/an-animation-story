using UnityEngine;
using System.Collections;

public sealed class GameObjectGrid : MonoBehaviour 
{
	public delegate void Callback(bool success);

	public GameObject prefab;

	public UInt3 gridSize;
	public Vector3 occupantSize = Vector3.one;
	public Vector3 padding = Vector3.zero;

	public int generationsPerFrame = int.MaxValue;

	public bool autoGenerateOnStart = true;
	public bool autoDestroyRemovedOccupants = true;

	private GameObject[] _grid;
	private GameObject[] grid
	{
		get 
		{
			if (_grid == null) {
				_grid = new GameObject[this.gridSize.x * this.gridSize.y * this.gridSize.z];
			}

			return _grid;
		}
	}

	private void Start()
	{
		if (this.autoGenerateOnStart) {
			Regenerate();
		}
	}

	public void Regenerate(Callback onComplete = null)
	{
		RemoveAllOccupants();

		if (this.prefab == null || this.gridSize.volume == 0) 
		{
			if (onComplete != null) {
				onComplete(false);
			}

			return;
		}
		
		StartCoroutine(Generate(onComplete));
	}

	private IEnumerator Generate(Callback onComplete = null)
	{
		Vector3 position = Vector3.zero;
		Vector3 size = new Vector3(this.gridSize.x, this.gridSize.y, this.gridSize.z);

		float occupantHalfX = (this.occupantSize.x * 0.5f);
		float occupantHalfY = (this.occupantSize.y * 0.5f);
		float occupantHalfZ = (this.occupantSize.z * 0.5f);

		float maxX = size.x * occupantHalfX;
		float maxY = size.y * occupantHalfY;
		float maxZ = size.z * occupantHalfZ;

		float minX = -maxX;
		float minY = -maxY;
		float minZ = -maxZ;

		float offsetX = (((size.x - 1.0f) * this.padding.x) * -0.5f) + occupantHalfX;
		float offsetY = (((size.y - 1.0f) * this.padding.y) * -0.5f) + occupantHalfY;
		float offsetZ = (((size.z - 1.0f) * this.padding.z) * -0.5f) + occupantHalfZ;

		int currentIteration = 0;
		int generationsPerFrame = this.generationsPerFrame > 0 ? this.generationsPerFrame : 1;

		for (uint x = 0; x < this.gridSize.x; x++)
		{
			float fx = (float)x;

			for (uint y = 0; y < this.gridSize.y; y++)
			{
				float fy = (float)y;

				for (uint z = 0; z < this.gridSize.z; z++) 
				{
					float fz = (float)z;

					position.x = Mathf.Lerp(minX, maxX, fx / size.x) + (this.padding.x * fx) + offsetX;
					position.y = Mathf.Lerp(minY, maxY, fy / size.y) + (this.padding.y * fy) + offsetY;
					position.z = Mathf.Lerp(minZ, maxZ, fz / size.z) + (this.padding.z * fz) + offsetZ;

					GameObject gameObject = Instantiate(this.prefab, this.transform);
					gameObject.SetActive(true);
					gameObject.transform.localPosition = position;

					StoreOccupant(gameObject, x, y, z);

					if (++currentIteration % generationsPerFrame == 0) {
						yield return new WaitForEndOfFrame();
					}
				}
			}
		}

		if (onComplete != null) {
			onComplete(true);
		}
	}

	public GameObject GetOccupant(uint x, uint y, uint z)
	{
		uint index = x + this.gridSize.x * (y + this.gridSize.y * z);

		if (index >= this.grid.Length) {
			return null;
		}
		
		return this.grid[index];
	}

	public void StoreOccupant(GameObject occupant, uint x, uint y, uint z)
	{
		uint index = x + this.gridSize.x * (y + this.gridSize.y * z);
		
		if (index >= this.grid.Length) {
			return;
		}

		RemoveOccupant(x, y, z);

		this.grid[index] = occupant;
	}

	public void RemoveOccupant(uint x, uint y, uint z, bool? destroy = null)
	{
		GameObject occupant = GetOccupant(x, y, z);

		if (occupant == null) {
			return;
		}

		if (destroy ?? this.autoDestroyRemovedOccupants) {
			Destroy(occupant);
		}

		uint index = x + this.gridSize.x * (y + this.gridSize.y * z);
		this.grid[index] = null;
	}

	public void RemoveAllOccupants(bool? destroyOccupants = null)
	{
		if (destroyOccupants ?? this.autoDestroyRemovedOccupants) {
			DestroyAllOccupants();
		}

		_grid = null;
	}

	private void DestroyAllOccupants()
	{
		for (int i = 0; i < this.grid.Length; i++)
		{
			GameObject occupant = this.grid[i];

			if (occupant != null) {
				Destroy(occupant);
			}
		}
	}

}
