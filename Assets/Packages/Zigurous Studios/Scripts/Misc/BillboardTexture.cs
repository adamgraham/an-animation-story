using UnityEngine;

public sealed class BillboardTexture : MonoBehaviour 
{
	public Camera targetCamera;
	public Bool3 constraints;

	private void LateUpdate()
	{
		Vector3 originalRotation = this.transform.eulerAngles;

		if (this.targetCamera != null) {
			this.transform.LookAt(this.targetCamera.transform);
		} else {
			this.transform.LookAt(Camera.main.transform);
		}

		Vector3 constrainedRotation = this.transform.eulerAngles;

		if (this.constraints.x) {
			constrainedRotation.x = originalRotation.x;
		}

		if (this.constraints.y) {
			constrainedRotation.y = originalRotation.y;
		}

		if (this.constraints.z) {
			constrainedRotation.z = originalRotation.z;
		}

		this.transform.eulerAngles = constrainedRotation;
	}

}
