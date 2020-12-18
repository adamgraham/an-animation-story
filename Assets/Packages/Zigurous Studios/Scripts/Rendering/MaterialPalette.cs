using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Custom/Material Palette")]
public sealed class MaterialPalette : ScriptableObject
{
	public Material[] palette;

	public Material GetRandomMaterial()
	{
		return this.palette[Random.Range(0, this.palette.Length)];
	}

	public Color GetRandomColor()
	{
		return GetRandomMaterial().color;
	}

}
