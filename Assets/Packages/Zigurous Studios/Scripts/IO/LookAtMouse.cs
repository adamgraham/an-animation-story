using UnityEngine;

public sealed class LookAtMouse : MonoBehaviour
{
	public bool hideMouse;

	private void Update()
	{
		Cursor.visible = !this.hideMouse;

		Vector3 objectPos = Camera.main.WorldToScreenPoint(this.transform.position);
		Vector3 direction = Input.mousePosition - objectPos;
		
		float rotation = (-Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90.0f;
		Vector3 euler = new Vector3(0.0f, rotation, 0.0f);
		this.transform.rotation = Quaternion.Euler(euler);
	}

}
