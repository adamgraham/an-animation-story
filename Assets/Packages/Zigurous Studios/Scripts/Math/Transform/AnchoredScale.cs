using UnityEngine;

[RequireComponent(typeof(AnchoredTransform))]
public sealed class AnchoredScale : MonoBehaviour 
{
	public AnchoredTransform scaleTo;
	public AnchoredTransform.Anchor scaleToAnchor;

	private AnchoredTransform _anchoredTransform;

	private void Awake()
	{
		_anchoredTransform = GetComponent<AnchoredTransform>();
	}

	private void OnDestroy()
	{
		this.scaleTo = null;
		
		_anchoredTransform = null;
	}

	private void LateUpdate()
	{
		if (this.scaleTo == null || _anchoredTransform == null) {
			return;
		}

		_anchoredTransform.UpdatePositionIfNeeded();
		this.scaleTo.UpdatePositionIfNeeded();

		Vector3 from = _anchoredTransform.GetAnchorPosition();
		Vector3 to = this.scaleTo.GetAnchorPosition(this.scaleToAnchor);
		Vector3 scale = to - from;
		
		this.transform.localScale = scale;
	}
	
}
