using UnityEngine;

public sealed class RandomizeInstantiate : MonoBehaviour 
{
	[System.Serializable]
	public struct RandomInstantiate
	{
		public GameObject prefab;

		[Range(0.0f, 1.0f)]
		public float spawnChance;
		public int spawnAmount;
		public bool parented;

	}

	public RandomInstantiate[] randomizations;

	public bool randomizeOnStart = true;
	public bool destroyAfterRandomizing = true;

	private void Start()
	{
		if (this.randomizeOnStart) {
			Randomize();
		}
	}

	private void OnDestroy()
	{
		this.randomizations = null;
	}

	public void Randomize()
	{
		for (int i = 0; i < this.randomizations.Length; i++)
		{
			RandomInstantiate randomization = this.randomizations[i];

			int amount = 0;
			while (amount++ < randomization.spawnAmount)
			{
				if (Random.value <= randomization.spawnChance)
				{
					GameObject newGameObject = Instantiate(randomization.prefab, transform.position, Quaternion.identity) as GameObject;
					
					if (randomization.parented) {
						newGameObject.transform.parent = transform;
					}
				}
			}
		}

		if (this.destroyAfterRandomizing) {
			Destroy(this, 1.0f);
		}
	}

}
