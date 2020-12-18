using UnityEngine;

public sealed class HideMouse : MonoBehaviour 
{
	private void Start()
	{
		Cursor.visible = false;
		Destroy(this);
	}

}
