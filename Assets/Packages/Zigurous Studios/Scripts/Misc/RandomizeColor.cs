using UnityEngine;

public sealed class RandomizeColor : MonoBehaviour 
{
	[System.Serializable]
	public struct RandomMaterial
	{
		public Renderer renderer;
		public Material[] mats;

	}

	[System.Serializable]
	public struct RandomTint
	{
		public Material material;
		public Color[] colors;
		
	}

	public RandomMaterial[] matRandomizations;
	public RandomTint[] tintRandomizations;

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
		this.matRandomizations = null;
		this.tintRandomizations = null;
	}

	public void Randomize()
	{
		for (int i = 0; i < this.matRandomizations.Length; i++)
		{
			RandomMaterial matRandomization = this.matRandomizations[i];
			Renderer renderer = matRandomization.renderer;
			Material[] mats = matRandomization.mats;

			if (renderer == null) renderer = gameObject.GetComponent<Renderer>();
			if (renderer != null && mats.Length > 0) renderer.material = mats[Random.Range(0, mats.Length)];
		}

		for (int i = 0; i < this.tintRandomizations.Length; i++)
		{
			RandomTint tintRandomization = this.tintRandomizations[i];
			tintRandomization.material.color = tintRandomization.colors[Random.Range(0, tintRandomization.colors.Length)];
		}

		if (this.destroyAfterRandomizing) {
			Destroy(this, 1.0f);
		}
	}

}
