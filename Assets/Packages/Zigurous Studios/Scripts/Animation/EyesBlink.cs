using UnityEngine;

public sealed class EyesBlink : MonoBehaviour 
{
	public Renderer[] renderers;
	public Material eyesOpen;
	public Material eyesClosed;

	[Range(0.0f, 1.0f)]
	public float blinkChance = 0.0035f;
	public FloatRange blinkDuration = new FloatRange(0.075f, 0.125f);

	private bool _blinking;

	private void OnDestroy()
	{
		this.renderers = null;
		this.eyesOpen = null;
		this.eyesClosed = null;
	}

	private void FixedUpdate()
	{
		if (Random.value <= this.blinkChance) {
			Blink();
		}
	}

	public void Blink()
	{
		if (!_blinking)
		{
			_blinking = true;

			CloseEyes();
			CancelInvoke("BlinkComplete");
			Invoke("BlinkComplete", this.blinkDuration.RandomValue());
		}
	}

	public void OpenEyes()
	{
		if (this.renderers == null) {
			return;
		}

		for (int i = 0; i < this.renderers.Length; i++) {
			this.renderers[i].material = this.eyesOpen;
		}
	}

	public void CloseEyes()
	{
		if (this.renderers == null) {
			return;
		}

		for (int i = 0; i < this.renderers.Length; i++) {
			this.renderers[i].material = this.eyesClosed;
		}
	}

	private void BlinkComplete()
	{
		OpenEyes();

		_blinking = false;
	}

}
