using UnityEngine;

public static class TransformExtensions 
{
	public static bool HasBeenInvalidated(this Transform transform)
	{
		if (transform.parent != null) {
			return transform.parent.HasBeenInvalidated();
		} else {
			return transform.hasChanged;
		}
	}

	public static Transform[] AllChildren(this Transform transform)
	{
		int count = transform.childCount;
		Transform[] children = new Transform[count];

		for (int i = 0; i < count; i++) {
			children[i] = transform.GetChild(i);
		}

		return children;
	}

	public static void AllChildrenSetActive(this Transform transform, bool active)
	{
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(active);
		}
	}

	public static Vector3 CenterPositionOfChildren(this Transform transform)
	{
		Vector3 center = Vector3.zero;
		Transform[] children = transform.AllChildren();

		for (int i = 0; i < children.Length; i++) {
			center += children[i].position;
		}

		return center / transform.childCount;
	}

}
