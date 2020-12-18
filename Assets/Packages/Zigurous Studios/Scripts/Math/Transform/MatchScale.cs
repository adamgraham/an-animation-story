using UnityEngine;

public sealed class MatchScale : MonoBehaviour 
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

		Vector3 scale = this.transform.localScale;

		if (this.match.x) {
			scale.x = this.transformToMatch.localScale.x;
		}

		if (this.match.y) {
			scale.y = this.transformToMatch.localScale.y;
		}

		if (this.match.z) {
			scale.z = this.transformToMatch.localScale.z;
		}

		this.transform.localScale = scale;
	}

}
