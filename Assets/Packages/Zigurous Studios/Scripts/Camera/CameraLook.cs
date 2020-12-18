using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraLook : MonoBehaviour 
{
	public Transform focus;

	public float smoothTime = 0.25f;
	public float maxSpeed = 100.0f;

	private Camera _camera;
	private Vector3 _velocity;
	
	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	private void OnDestroy()
	{
		_camera = null;

		this.focus = null;
	}

	private void FixedUpdate()
	{
		if (this.focus == null || _camera == null) {
			return;
		}

		Vector3 currentRotation = _camera.transform.eulerAngles;
		_camera.transform.LookAt(this.focus);
		Vector3 targetRotation = _camera.transform.eulerAngles;
		_camera.transform.eulerAngles = currentRotation;
		Vector3 rotation = Vector3.SmoothDamp(_camera.transform.eulerAngles, targetRotation, ref _velocity, this.smoothTime, this.maxSpeed);
		_camera.transform.eulerAngles = rotation;
	}

}
