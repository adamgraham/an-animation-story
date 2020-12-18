using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct AudioSourceExtended
{
	public AudioSource audioSource;

	[Range(0.0f, 1.0f)]
	public float defaultVolume;

	public void PlayAndFadeIn(float duration)
	{
		this.audioSource.Play();
		FadeIn(duration);
	}

	public void FadeIn(float duration, bool resetVolume = true)
	{
		if (resetVolume) {
			this.audioSource.volume = 0.0f;
		}
		
		this.audioSource.DOFade(this.defaultVolume, duration);
	}

	public void FadeOut(float duration)
	{
		this.audioSource.DOFade(0.0f, duration);
	}

}
