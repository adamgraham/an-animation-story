using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Light))]
public sealed class LightFlicker : MonoBehaviour 
{
	[HideInInspector]
	[SerializeField]
	new Light light;

	[HideInInspector]
	public float baseIntensity;

	public float fluctuation = 0.25f;
	public float duration = 0.25f;

	private float _startingIntensity;
	private float _startingRange;
	private bool _flickering;

	private void Awake() 
	{
		this.light = GetComponent<Light>();
		this.baseIntensity = this.light.intensity;

		_startingIntensity = this.light.intensity;
		_startingRange = this.light.range;
	}

	private void OnDestroy()
	{
		this.light = null;
	}

	private void FixedUpdate() 
	{
		if (!_flickering) {
			Flicker();
		}
	}

	public void Flicker()
	{
		float fluctuation = Random.Range(-this.fluctuation, this.fluctuation);
		
		this.light.DOIntensity(this.baseIntensity + fluctuation, this.duration)
			.OnComplete(OnFlickerComplete);

		_flickering = true;
	}

	private void OnFlickerComplete()
	{
		_flickering = false;
	}

}
