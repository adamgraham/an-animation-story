using UnityEngine;
using DG.Tweening;

public sealed class YoYoRotate : MonoBehaviour
{
	public Vector3Range delta;
	public FloatRange duration;

	public Ease ease = Ease.InOutQuad;

	public bool autoStartAndStop = true;
	public bool localMovement = true;

	private Vector3 _startingRotation;
	private float _duration;

	private void Start()
	{
		_startingRotation = this.localMovement ? this.transform.localEulerAngles : this.transform.eulerAngles;

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
				this.transform.DOLocalRotate(_startingRotation + this.delta.min, _duration * 0.5f).
					SetEase(this.ease).OnComplete(OnYoYoComplete);
			}
			else
			{
				this.transform.DORotate(_startingRotation + this.delta.min, _duration * 0.5f).
					SetEase(this.ease).OnComplete(OnYoYoComplete);
			}
		}
	}

	private void OnYoYoComplete()
	{
		this.transform.DOKill();

		if (this.localMovement)
		{
			this.transform.DOLocalRotate(_startingRotation + this.delta.max, _duration * 0.5f).
				SetEase(this.ease).OnComplete(StartYoYo);
		}
		else
		{
			this.transform.DORotate(_startingRotation + this.delta.max, _duration * 0.5f).
				SetEase(this.ease).OnComplete(StartYoYo);
		}
	}

}
