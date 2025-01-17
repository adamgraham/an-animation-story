﻿using UnityEngine;
using UnityEngine.Rendering;

public static class MaterialExtensions
{
	public enum RenderingMode { Opaque, Cutout, Fade, Transparent }

	public static RenderingMode GetRenderingMode(this Material material)
	{
		int renderingMode = (int)material.GetFloat("_Mode");

		switch (renderingMode)
		{
			case 1:
				return RenderingMode.Cutout;
			case 2:
				return RenderingMode.Fade;
			case 3:
				return RenderingMode.Transparent;
			default:
				return RenderingMode.Opaque;
		}
	}

	public static void SetRenderingMode(this Material material, RenderingMode renderingMode)
	{
		switch (renderingMode)
		{
			case RenderingMode.Opaque:
				material.SetFloat("_Mode", 0);
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = -1;
				break;

			case RenderingMode.Cutout:
				material.SetFloat("_Mode", 1);
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				material.SetInt("_ZWrite", 1);
				material.EnableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 2450;
				break;

			case RenderingMode.Fade:
				material.SetFloat("_Mode", 2);
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.EnableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				break;

			case RenderingMode.Transparent:
				material.SetFloat("_Mode", 3);
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				break;
		}
	}

}
