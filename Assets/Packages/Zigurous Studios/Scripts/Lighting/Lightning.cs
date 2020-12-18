using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(Light))]
public sealed class Lightning : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	new Light light;

	[Header("General")]

	[Range(0.0f, 1.0f)]
	public float lightningChance = 0.0025f;
	public bool global;
	private bool _flashing;
	private static bool _globalFlashing;
	private static List<Lightning> _globalList;

	[Header("Transform")]

	public bool maintainRotation = true;
	[Range(0.0f, 360.0f)]
	public float rotationMin = 0.0f;
	[Range(0.0f, 360.0f)]
	public float rotationMax = 360.0f;

	[Header("Intensity")]

	[Range(0.0f, 8.0f)]
	public float intensityMin = 0.0f;
	[Range(0.0f, 8.0f)]
	public float intensityMax = 4.0f;
	[Range(0.0f, 8.0f)]
	public float intensityFluctuationMin = 0.0f;
	[Range(0.0f, 8.0f)]
	public float intensityFluctuationMax = 1.0f;

	private float _fillLightIntensity;

	[Header("Timing")]

	[Range(0.0f, 1.0f)]
	public float speedInMin = 0.95f;
	[Range(0.0f, 1.0f)]
	public float speedInMax = 0.95f;

	[Range(0.0f, 1.0f)]
	public float speedOutMin = 0.95f;
	[Range(0.0f, 1.0f)]
	public float speedOutMax = 0.95f;

	[Header("Ripling")]

	public int ripplesMin = 2;
	public int ripplesMax = 4;

	[Header("Audio")]

	public AudioClip[] audioClips;

	public float audioIntervalMin;
	public float audioIntervalMax;

	private bool _audioCooldown;

	private void Awake()
	{
		this.light = GetComponent<Light>();

		_fillLightIntensity = light.intensity;

		if (_globalList == null) {
			_globalList = new List<Lightning>();
		}

		_globalList.Add(this);
	}

	private void OnDestroy()
	{
		if (_globalList != null) {
			_globalList.Remove(this);
		}

		this.light = null;
		this.audioClips = null;
	}

	private void Update()
	{
		if (_flashing) {
			_globalFlashing = true;
		}
	}

	private void FixedUpdate() 
	{
		if (Random.value < this.lightningChance) {
			Flash();
		}
	}

	public void Flash()
	{
		if (_flashing || (this.global && _globalFlashing)) {
			return;
		}

		_flashing = true;
		_globalFlashing = true;

		if (!this.maintainRotation) 
		{
			this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 
													 Random.Range(this.rotationMin, this.rotationMax), 
													 this.transform.eulerAngles.z);
		}

		Sequence thunder = DOTween.Sequence();

		float intensity = Random.Range(this.intensityMin, this.intensityMax);
		float durationIn = 1.0f - Random.Range(this.speedInMin, this.speedInMax);
		float durationOut = 1.0f - Random.Range(this.speedOutMin, this.speedOutMax);

		int ripples = Random.Range(this.ripplesMin, this.ripplesMax);
		for (int i = 0; i < ripples; i++)
		{
			thunder.Append(this.light.DOIntensity(intensity, durationIn).SetEase(Ease.OutQuad));
			thunder.Append(this.light.DOIntensity(1.0f, durationOut).SetEase(Ease.InQuad));
			
			float fluctutation = Random.Range(this.intensityFluctuationMin, this.intensityFluctuationMax) * ((Random.Range(0, 2) == 1) ? 1.0f : -1.0f);
			intensity = Mathf.Clamp(intensity + fluctutation, this.intensityMin, this.intensityMax);
		}

		thunder.Append(this.light.DOIntensity(intensity, durationIn).SetEase(Ease.OutQuad));
		thunder.Append(this.light.DOIntensity(_fillLightIntensity, durationOut).SetEase(Ease.InQuad));
		thunder.Play().OnComplete(OnFlashComplete);

		PlayAudio();
	}

	private void OnFlashComplete()
	{
		_flashing = false;
		_globalFlashing = false;
	}

	private void PlayAudio()
	{
		if (_audioCooldown || this.audioClips == null || this.audioClips.Length <= 0) {
			return;
		}

		AudioSource.PlayClipAtPoint(this.audioClips[Random.Range(0, this.audioClips.Length)], Camera.main.transform.position);

		_audioCooldown = true;

		CancelInvoke("OnAudioIntervalComplete");
		Invoke("OnAudioIntervalComplete", Random.Range(this.audioIntervalMin, this.audioIntervalMax));
	}

	private void OnAudioIntervalComplete()
	{
		_audioCooldown = false;
	}

	public static void ToggleAllLightning(bool state)
	{
		if (_globalList == null) {
			return;
		}

		for (int i = 0; i < _globalList.Count; i++)
		{
			Lightning lightning = _globalList[i];
			
			if (lightning != null) {
				lightning.enabled = state;
			}
		}
	}

}
