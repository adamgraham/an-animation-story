using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraAveragingFollow : MonoBehaviour 
{
	public Transform[] focuses;

	public Vector3 offset = Vector3.zero;

	public float smoothTime = 0.5f;
	public float maxSpeed = 50.0f;

	public bool maintainHeight = true;

	private Camera _camera;
	private Vector3 _velocity;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	private void OnDestroy()
	{
		_camera = null;

		this.focuses = null;
	}

	private void FixedUpdate()
	{
		if (this.focuses == null || this.focuses.Length <= 0 || _camera == null) {
			return;
		}

		Vector3 targetPosition = GetCameraTargetPosition(GetCenterFocusPosition());
		Vector3 position = Vector3.SmoothDamp(_camera.transform.position, targetPosition, ref _velocity, this.smoothTime, this.maxSpeed);

		if (this.maintainHeight) {
			position.y = _camera.transform.position.y;
		}

		_camera.transform.position = position;
	}

	private Vector3 GetCenterFocusPosition()
	{
		Vector3 center = Vector3.zero;

		for (int i = 0; i < this.focuses.Length; i++) {
			center += this.focuses[i].position;
		}

		return center / this.focuses.Length;
	}

	private Vector3 GetCameraTargetPosition(Vector3 endPoint)
	{
		Vector3 target;

		if (_camera.orthographic)
		{
			Vector3 point = _camera.WorldToViewportPoint(endPoint);
			Vector3 delta = endPoint - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			target = _camera.transform.position + delta + this.offset;
		} 
		else
		{
			target = endPoint + this.offset;
		}

		return target;
	}

}
