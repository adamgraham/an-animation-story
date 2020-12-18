using UnityEngine;

public sealed class Spin : MonoBehaviour 
{
	public enum Axis { None, X, Y, Z, XY, XZ, YZ, XYZ }
	public Axis axis = Axis.Y;
	
	public float speed;

	private Vector3 _rotation;

	private void Update() 
	{
		SetRotationVector();

		this.transform.eulerAngles += _rotation * Time.deltaTime;
	}

	private void SetRotationVector()
	{
		_rotation.x = 0.0f;
		_rotation.y = 0.0f;
		_rotation.z = 0.0f;

		switch (this.axis)
		{
			case Axis.X:
			case Axis.XY:
			case Axis.XZ:
			case Axis.XYZ:
				_rotation.x = this.speed;
				break;

			default:
				break;
		}

		switch (this.axis)
		{
			case Axis.Y:
			case Axis.YZ:
			case Axis.XY:
			case Axis.XYZ:
				_rotation.y = this.speed;
				break;

			default:
				break;
		}

		switch (this.axis)
		{
			case Axis.Z:
			case Axis.YZ:
			case Axis.XZ:
			case Axis.XYZ:
				_rotation.z = this.speed;
				break;

			default:
				break;
		}
	}

}
