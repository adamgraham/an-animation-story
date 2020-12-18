using UnityEngine;
using DG.Tweening;

public sealed class YoYoTranslate : MonoBehaviour
{
	public Vector3Range delta;
	public FloatRange duration;

	public Ease ease = Ease.InOutQuad;

	public bool autoStartAndStop = true;
	public bool localMovement = true;

	private Vector3 _startingPosition;
	private float _duration;

	private void Start()
	{
		_startingPosition = this.localMovement ? this.transform.localPosition : this.transform.position;

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

			if (this.localMovement)
			{
				this.transform.DOLocalMove(_startingPosition + this.delta.min, _duration * 0.5f).
					SetEase(this.ease).OnComplete(OnYoYoComplete);
			}
			else
			{
				this.transform.DOMove(_startingPosition + this.delta.min, _duration * 0.5f).
					SetEase(this.ease).OnComplete(OnYoYoComplete);
			}
		}
	}

	private void OnYoYoComplete()
	{
		this.transform.DOKill();

		if (this.localMovement)
		{
			this.transform.DOLocalMove(_startingPosition + this.delta.max, _duration * 0.5f).
				SetEase(this.ease).OnComplete(StartYoYo);
		}
		else
		{
			this.transform.DOMove(_startingPosition + this.delta.max, _duration * 0.5f).
				SetEase(this.ease).OnComplete(StartYoYo);
		}
	}

}
