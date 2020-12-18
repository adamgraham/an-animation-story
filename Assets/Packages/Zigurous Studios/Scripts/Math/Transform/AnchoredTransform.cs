using UnityEngine;

[RequireComponent(typeof(Transform))]
public sealed class AnchoredTransform : MonoBehaviour
{
	public enum Anchor { TopLeft, TopCenter, TopRight,
						 CenterLeft, Center, CenterRight,
						 BottomLeft, BottomCenter, BottomRight }

	public Anchor anchor = Anchor.Center;
	
	public bool local = true;
	
	[Header("Attachment")]

	public AnchoredTransform attachment;
	public AnchoredTransform.Anchor attachmentAnchor;
	public Vector3 attachmentOffset;

	private bool _positionInvalidated;

	private void OnDestroy()
	{
		this.attachment = null;
	}

	private void Update()
	{
		InvalidatePosition();
	}

	private void LateUpdate()
	{
		UpdatePositionIfNeeded();
	}

	public void InvalidatePosition()
	{
		_positionInvalidated = true;
	}

	public void UpdatePositionIfNeeded()
	{
		if (_positionInvalidated) {
			UpdatePosition();
		}
	}

	private void UpdatePosition()
	{
		if (this.attachment == null) {
			return;
		}

		this.attachment.UpdatePosition();

		if (this.local && this.transform.parent != null && this.attachment.transform.IsChildOf(this.transform.parent)) 
		{
			Vector3 attachmentPosition = this.attachment.GetLocalAnchorPosition(this.attachmentAnchor);
			Vector3 offset = GetLocalAnchorOffset(this.attachmentOffset);
			this.transform.localPosition = attachmentPosition - offset;
		}
		else 
		{
			Vector3 attachmentPosition = this.attachment.GetAnchorPosition(this.attachmentAnchor);
			Vector3 offset = GetAnchorOffset(this.attachmentOffset);
			this.transform.position = attachmentPosition - offset;
		}

		_positionInvalidated = false;
	}

	// private void OnDrawGizmos()
	// {
	// 	Gizmos.color = Color.red;
	// 	Gizmos.DrawSphere(GetAnchorPosition(), 0.05f);
	// }

	public void AttachTo(AnchoredTransform otherTransform, Vector3 additionalOffset = new Vector3())
	{
		this.attachment = otherTransform;
		this.attachmentAnchor = otherTransform.anchor;
		this.attachmentOffset = additionalOffset;
	}

	public void AttachTo(AnchoredTransform otherTransform, Anchor otherTransformAnchor, Vector3 additionalOffset = new Vector3())
	{
		this.attachment = otherTransform;
		this.attachmentAnchor = otherTransformAnchor;
		this.attachmentOffset = additionalOffset;
	}

	public void Detach()
	{
		this.attachment = null;
		this.attachmentAnchor = Anchor.Center;
		this.attachmentOffset = Vector3.zero;
	}

	public Vector3 GetAnchorPosition()
	{
		return GetAnchorPosition(this.anchor);
	}

	public Vector3 GetAnchorPosition(Anchor anchor)
	{
		return this.transform.position + GetAnchorOffset(anchor);
	}

	public Vector3 GetAnchorOffset()
	{
		return GetAnchorOffset(this.anchor);
	}

	public Vector3 GetAnchorOffset(Vector3 additionalOffset)
	{
		return GetAnchorOffset(this.anchor, additionalOffset);
	}

	public Vector3 GetAnchorOffset(Anchor anchor, Vector3 additionalOffset = new Vector3())
	{
		switch (anchor)
		{
			case Anchor.TopLeft:
				return WorldTopOffset(additionalOffset) + WorldLeftOffset(additionalOffset);

			case Anchor.TopCenter:
				return WorldTopOffset(additionalOffset);

			case Anchor.TopRight:
				return WorldTopOffset(additionalOffset) + WorldRightOffset(additionalOffset);

			case Anchor.CenterLeft:
				return WorldLeftOffset(additionalOffset);

			case Anchor.Center:
				return WorldCenterOffset(additionalOffset);

			case Anchor.CenterRight:
				return WorldRightOffset(additionalOffset);

			case Anchor.BottomLeft:
				return WorldBottomOffset(additionalOffset) + WorldLeftOffset(additionalOffset);

			case Anchor.BottomCenter:
				return WorldBottomOffset(additionalOffset);

			case Anchor.BottomRight:
				return WorldBottomOffset(additionalOffset) + WorldRightOffset(additionalOffset);

			default:
				return WorldCenterOffset(additionalOffset);
		}
	}

	private Vector3 WorldTopOffset(Vector3 additionalOffset)
	{
		Vector3 offset = Vector3.zero;
		offset.x = (0.5f + additionalOffset.y) * this.transform.up.x * this.transform.localScale.y;
		offset.y = (0.5f + additionalOffset.y) * this.transform.up.y * this.transform.localScale.y;
		offset.z = (0.5f + additionalOffset.y) * this.transform.up.z * this.transform.localScale.y;
		return offset;
	}

	private Vector3 WorldBottomOffset(Vector3 additionalOffset)
	{
		Vector3 offset = Vector3.zero;
		offset.x = (-0.5f + additionalOffset.y) * this.transform.up.x * this.transform.localScale.y;
		offset.y = (-0.5f + additionalOffset.y) * this.transform.up.y * this.transform.localScale.y;
		offset.z = (-0.5f + additionalOffset.y) * this.transform.up.z * this.transform.localScale.y;
		return offset;
	}

	private Vector3 WorldCenterOffset(Vector3 additionalOffset)
	{
		return additionalOffset;
	}

	private Vector3 WorldLeftOffset(Vector3 additionalOffset)
	{
		Vector3 offset = Vector3.zero;
		offset.x = (-0.5f + additionalOffset.x) * this.transform.right.x * this.transform.localScale.x;
		offset.y = (-0.5f + additionalOffset.x) * this.transform.right.y * this.transform.localScale.x;
		offset.z = (-0.5f + additionalOffset.x) * this.transform.right.z * this.transform.localScale.x;
		return offset;
	}

	private Vector3 WorldRightOffset(Vector3 additionalOffset)
	{
		Vector3 offset = Vector3.zero;
		offset.x = (0.5f + additionalOffset.x) * this.transform.right.x * this.transform.localScale.x;
		offset.y = (0.5f + additionalOffset.x) * this.transform.right.y * this.transform.localScale.x;
		offset.z = (0.5f + additionalOffset.x) * this.transform.right.z * this.transform.localScale.x;
		return offset;
	}

	public Vector3 GetLocalAnchorPosition()
	{
		return GetLocalAnchorPosition(this.anchor);
	}

	public Vector3 GetLocalAnchorPosition(Anchor anchor)
	{
		return this.transform.localPosition + GetLocalAnchorOffset(anchor);
	}

	public Vector3 GetLocalAnchorOffset()
	{
		return GetLocalAnchorOffset(this.anchor);
	}

	public Vector3 GetLocalAnchorOffset(Vector3 additionalOffset)
	{
		return GetLocalAnchorOffset(this.anchor, additionalOffset);
	}

	public Vector3 GetLocalAnchorOffset(Anchor anchor, Vector3 additionalOffset = new Vector3())
	{
		switch (anchor)
		{
			case Anchor.TopLeft:
				return LocalTopOffset(additionalOffset) + LocalLeftOffset(additionalOffset);

			case Anchor.TopCenter:
				return LocalTopOffset(additionalOffset);

			case Anchor.TopRight:
				return LocalTopOffset(additionalOffset) + LocalRightOffset(additionalOffset);

			case Anchor.CenterLeft:
				return LocalLeftOffset(additionalOffset);

			case Anchor.Center:
				return LocalCenterOffset(additionalOffset);

			case Anchor.CenterRight:
				return LocalRightOffset(additionalOffset);

			case Anchor.BottomLeft:
				return LocalBottomOffset(additionalOffset) + LocalLeftOffset(additionalOffset);

			case Anchor.BottomCenter:
				return LocalBottomOffset(additionalOffset);

			case Anchor.BottomRight:
				return LocalBottomOffset(additionalOffset) + LocalRightOffset(additionalOffset);

			default:
				return LocalCenterOffset(additionalOffset);
		}
	}

	private Vector3 LocalTopOffset(Vector3 additionalOffset)
	{
		Vector3 offset = Vector3.zero;
		offset.y = (0.5f + additionalOffset.y) * this.transform.localScale.y;
		return offset;
	}

	private Vector3 LocalBottomOffset(Vector3 additionalOffset)
	{
		Vector3 offset = Vector3.zero;
		offset.y = (-0.5f + additionalOffset.y) * this.transform.localScale.y;
		return offset;
	}

	private Vector3 LocalCenterOffset(Vector3 additionalOffset)
	{
		return additionalOffset;
	}

	private Vector3 LocalLeftOffset(Vector3 additionalOffset)
	{
		Vector3 offset = Vector3.zero;
		offset.x = (-0.5f + additionalOffset.x) * this.transform.localScale.x;
		return offset;
	}

	private Vector3 LocalRightOffset(Vector3 additionalOffset)
	{
		Vector3 offset = Vector3.zero;
		offset.x = (0.5f + additionalOffset.x) * this.transform.localScale.x;
		return offset;
	}

}
