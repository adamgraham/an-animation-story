using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class MouseWheelOrthographicZoom : MonoBehaviour 
{
	public FloatRange zoomLevel = new FloatRange(2.0f, 8.0f);
	public float zoomSpeed = 1.0f;

	private Camera _camera;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	private void Start()
	{
		_camera.orthographicSize = this.zoomLevel.Clamp(_camera.orthographicSize);
	}

	private void Update()
	{
		float scrollDelta = Input.GetAxis("Mouse ScrollWheel") * -this.zoomSpeed;
		_camera.orthographicSize = this.zoomLevel.Clamp(_camera.orthographicSize + scrollDelta);
	}

	private void OnDestroy()
	{
		_camera = null;
	}

}
