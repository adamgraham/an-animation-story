using UnityEngine;

public sealed class MatchPosition : MonoBehaviour 
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
		
		Vector3 position = this.transform.position;

		if (this.match.x) {
			position.x = this.transformToMatch.position.x;
		}

		if (this.match.y) {
			position.y = this.transformToMatch.position.y;
		}

		if (this.match.z) {
			position.z = this.transformToMatch.position.z;
		}

		this.transform.position = position;
	}

}
