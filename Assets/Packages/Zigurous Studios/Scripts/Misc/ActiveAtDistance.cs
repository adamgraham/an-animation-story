using UnityEngine;

public sealed class ActiveAtDistance : MonoBehaviour 
{
	public GameObject activatingObject;
	public Transform target;
	public float minDistance;

	private void Update()
	{
		if (this.target == null || this.activatingObject == null) {
			return;
		}
		
		float distance = Vector3.Distance(this.activatingObject.transform.position, this.target.position);
		this.activatingObject.SetActive(distance <= this.minDistance);
	}

}
