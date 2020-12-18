using UnityEngine;

public sealed class MatchTransform : MonoBehaviour 
{
	public Transform transformToMatch;
	
	public bool matchPosition;
	public bool matchRotation;
	public bool matchScale;

	private void OnDestroy()
	{
		this.transformToMatch = null;
	}

	private void LateUpdate()
	{
		if (this.transformToMatch == null) {
			return;
		}

		if (this.matchPosition) {
			this.transform.position = this.transformToMatch.position;
		}

		if (this.matchRotation) {
			this.transform.eulerAngles = this.transformToMatch.eulerAngles;
		}

		if (this.matchScale) {
			this.transform.localScale = this.transformToMatch.localScale;
		}
	}

}
