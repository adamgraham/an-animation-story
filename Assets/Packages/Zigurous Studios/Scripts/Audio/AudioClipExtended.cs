using UnityEngine;

[System.Serializable]
public struct AudioClipExtended
{
	public AudioClip audioClip;

	[Range(0.0f, 1.0f)]
	public float volume;

	public void Play()
	{
		AudioUtils.PlayClipAtPoint(this, Vector3.zero);
	}

	public void Play(Vector3 position)
	{
		AudioUtils.PlayClipAtPoint(this, position);
	}

}
