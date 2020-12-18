using UnityEngine;
using DG.Tweening;

public sealed class YoYoScale : MonoBehaviour 
{
	public Vector3Range delta;
	public FloatRange duration;

	public Ease ease = Ease.InOutQuad;

	public bool autoStartAndStop = true;

	private Vector3 _startingScale;
	private float _duration;

	private void Start()
	{
		_startingScale = this.transform.localScale;

		if (this.autoStartAndStop) {
			StartYoYo();
		}
	}

	private void OnEnable()
	{
		if (this.autoStartAndStop) {
			StartYoYo();
		}
	}

	private void OnDisable()
	{
		if (this.autoStartAndStop) {
			StopYoYo();
		}
	}

	private void OnDestroy()
	{
		StopYoYo();
	}

	public void StopYoYo()
	{
		this.transform.DOKill();
	}

	public void StartYoYo()
	{
		_duration = this.duration.RandomValue();

		if (_duration > 0.0f)
		{
			this.transform.DOKill();
			this.transform.DOScale(_startingScale + this.delta.min, _duration * 0.5f).
				SetEase(this.ease).OnComplete(OnYoYoComplete);
		}
	}

	private void OnYoYoComplete()
	{
		this.transform.DOKill();
		this.transform.DOScale(_startingScale + this.delta.max, _duration * 0.5f).
			SetEase(this.ease).OnComplete(StartYoYo);
	}

}
