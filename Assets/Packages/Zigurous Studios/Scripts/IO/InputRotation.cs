using UnityEngine;

public sealed class InputRotation : MonoBehaviour 
{
	public string inputAxis = "Mouse X";
	public KeyCode requiredKeyDown = KeyCode.Mouse0;

	public enum Axis { X, Y, Z, X_NEG, Y_NEG, Z_NEG };
	public InputRotation.Axis axis = Axis.Y_NEG;

	public float speed = 250.0f;

	public bool localRotation = true;

	private Vector3 _rotation;

	private void Update()
	{
		if (this.requiredKeyDown != KeyCode.None)
		{
			if (Input.GetKey(this.requiredKeyDown)) {
				Rotate();
			}
		} 
		else 
		{
			Rotate();
		}
	}

	private void Rotate()
	{
		float axisDelta = Input.GetAxis(this.inputAxis);

		if (float.IsNaN(axisDelta)) {
			return;
		}

		if (this.localRotation)
		{
			_rotation.x = this.transform.localEulerAngles.x;
			_rotation.y = this.transform.localEulerAngles.y;
			_rotation.z = this.transform.localEulerAngles.z;
		} 
		else 
		{
			_rotation.x = this.transform.eulerAngles.x;
			_rotation.y = this.transform.eulerAngles.y;
			_rotation.z = this.transform.eulerAngles.z;
		}

		if (this.axis == Axis.X_NEG || this.axis == Axis.Y_NEG || this.axis == Axis.Z_NEG) {
			axisDelta *= -1.0f;
		}

		if (this.axis == Axis.X || this.axis == Axis.X_NEG) {
			_rotation.x += axisDelta * this.speed * Time.deltaTime;
		} else if (this.axis == Axis.Y || this.axis == Axis.Y_NEG) {
			_rotation.y += axisDelta * this.speed * Time.deltaTime;
		} else if (this.axis == Axis.Z || this.axis == Axis.Z_NEG) {
			_rotation.z += axisDelta * this.speed * Time.deltaTime;
		}

		if (this.localRotation) {
			this.transform.localEulerAngles = _rotation;
		} else {
			this.transform.eulerAngles = _rotation;
		}
	}

}
