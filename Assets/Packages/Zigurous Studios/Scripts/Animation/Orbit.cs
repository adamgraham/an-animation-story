using UnityEngine;

public sealed class Orbit : MonoBehaviour 
{
	public enum RotationDirection { None, Clockwise, CounterClockwise }
	public RotationDirection direction = RotationDirection.Clockwise;

	public float speed;
	public float radius = 1.0f;

	[Range(0.0f, 360.0f)]
	public float startAngle;

	private Vector3 _position;
	private float _currentAngle;

	private float _directionMultiplier
	{
		get
		{
			switch (this.direction)
			{
				case RotationDirection.Clockwise:
					return -1.0f;
				case RotationDirection.CounterClockwise:
					return 1.0f;
				default:
					return 0.0f;
			}
		}
	}

	private void Start()
	{
		_currentAngle = this.startAngle;
		_position = this.transform.localPosition;
	}

	private void Update()
	{
		_currentAngle += this.speed * _directionMultiplier * Time.deltaTime;

		float radians = _currentAngle * Mathf.Deg2Rad;

		_position.x = (Mathf.Cos(radians) * this.radius);
		_position.y = this.transform.localPosition.y;
		_position.z = (Mathf.Sin(radians) * this.radius);
		
		this.transform.localPosition = _position;
	}

}
