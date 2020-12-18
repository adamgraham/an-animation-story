using UnityEngine;

public sealed class MatchRotation : MonoBehaviour 
{
	public Transform transformToMatch;
	public Bool3 match = Bool3.one;

	private void OnDestroy()
	{
		this.transformToMatch = null;
	}

	private void LateUpdate()
	{
		if (this.transformToMatch == null) {
			return;
		}

		Vector3 rotation = this.transform.eulerAngles;

		if (this.match.x) {
			rotation.x = this.transformToMatch.eulerAngles.x;
		}

		if (this.match.y) {
			rotation.y = this.transformToMatch.eulerAngles.y;
		}

		if (this.match.z) {
			rotation.z = this.transformToMatch.eulerAngles.z;
		}

		this.transform.eulerAngles = rotation;
	}

}
