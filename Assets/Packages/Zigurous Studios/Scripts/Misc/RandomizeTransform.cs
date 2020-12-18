using UnityEngine;
using System.Collections;

public sealed class RandomizeTransform : MonoBehaviour
{
	public enum Axis { None, X, Y, Z, XY, XZ, YZ, XYZ }

	[System.Serializable]
	public struct RandomRotation
	{
		public Transform transform;
		public Axis axis;
		public bool uniform;

		[Range(-360.0f, 360.0f)]
		public float min;
		[Range(-360.0f, 360.0f)]
		public float max;

	}

	[System.Serializable]
	public struct RandomScale
	{
		public Transform transform;
		public Axis axis;
		public bool uniform;

		public float min;
		public float max;

	}

	[System.Serializable]
	public struct RandomTranslation
	{
		public Transform transform;
		public Axis axis;
		public bool uniform;
		
		public float min;
		public float max;

	}

	public RandomRotation[] rotations;
	public RandomScale[] scales;
	public RandomTranslation[] translations;

	[HideInInspector]
	public Transform defaultTransform;
	[HideInInspector]
	public bool useDefaultTransformIfNull = true;

	public bool randomizeOnStart = true;
	public bool destroyAfterRandomizing = true;

	private void Start()
	{
		if (this.defaultTransform == null) {
			this.defaultTransform = transform;
		}

		if (this.randomizeOnStart) {
			Randomize();
		}
	}

	private void OnDestroy()
	{
		this.rotations = null;
		this.scales = null;
		this.translations = null;
		this.defaultTransform = null;
	}

	public void Randomize()
	{
		RandomizeRotation();
		RandomizeScale();
		RandomizeTranslation();

		if (this.destroyAfterRandomizing) {
			Destroy(this, 1.0f);
		}
	}

	private void RandomizeRotation()
	{
		for (int i = 0; i < this.rotations.Length; i++)
		{
			RandomRotation rotationRandomization = this.rotations[i];

			if (this.useDefaultTransformIfNull && rotationRandomization.transform == null) {
				rotationRandomization.transform = this.defaultTransform;
			}

			if (rotationRandomization.transform == null) {
				continue;
			}

			Vector3 rotation = rotationRandomization.transform.localEulerAngles;
			float rotationValue = Random.Range(rotationRandomization.min, rotationRandomization.max);
			Axis axis = rotationRandomization.axis;

			// x
			if (axis == Axis.X || axis == Axis.XZ || axis == Axis.XYZ) rotation.x = rotationValue;

			// y
			if (!rotationRandomization.uniform) rotationValue = Random.Range(rotationRandomization.min, rotationRandomization.max);
			if (axis == Axis.Y || axis == Axis.YZ || axis == Axis.XYZ) rotation.y = rotationValue;

			// z
			if (!rotationRandomization.uniform) rotationValue = Random.Range(rotationRandomization.min, rotationRandomization.max);
			if (axis == Axis.Z || axis == Axis.XZ || axis == Axis.XYZ) rotation.z = rotationValue;

			rotationRandomization.transform.localEulerAngles = rotation;
		}
	}

	private void RandomizeScale()
	{
		for (int i = 0; i < this.scales.Length; i++)
		{
			RandomScale scaleRandomization = this.scales[i];

			if (this.useDefaultTransformIfNull && scaleRandomization.transform == null) {
				scaleRandomization.transform = this.defaultTransform;
			}

			if (scaleRandomization.transform == null) {
				continue;
			}

			Vector3 scale = scaleRandomization.transform.localScale;
			float scaleValue = Random.Range(scaleRandomization.min, scaleRandomization.max);
			Axis axis = scaleRandomization.axis;

			// x
			if (axis == Axis.X || axis == Axis.XZ || axis == Axis.XYZ) scale.x *= scaleValue;

			// y
			if (!scaleRandomization.uniform) scaleValue = Random.Range(scaleRandomization.min, scaleRandomization.max);
			if (axis == Axis.Y || axis == Axis.YZ || axis == Axis.XYZ) scale.y *= scaleValue;

			// z
			if (!scaleRandomization.uniform) scaleValue = Random.Range(scaleRandomization.min, scaleRandomization.max);
			if (axis == Axis.Z || axis == Axis.XZ || axis == Axis.XYZ) scale.z *= scaleValue;

			scaleRandomization.transform.localScale = scale;
		}
	}

	private void RandomizeTranslation()
	{
		for (int i = 0; i < this.translations.Length; i++)
		{
			RandomTranslation translationRandomization = this.translations[i];

			if (this.useDefaultTransformIfNull && translationRandomization.transform == null) {
				translationRandomization.transform = this.defaultTransform;
			}

			if (translationRandomization.transform == null) {
				continue;
			}

			Vector3 translation = Vector3.zero;
			float translationValue = Random.Range(translationRandomization.min, translationRandomization.max);
			Axis axis = translationRandomization.axis;

			// x
			if (axis == Axis.X || axis == Axis.XZ || axis == Axis.XYZ) translation.x += translationValue;

			// y
			if (!translationRandomization.uniform) translationValue = Random.Range(translationRandomization.min, translationRandomization.max);
			if (axis == Axis.Y || axis == Axis.YZ || axis == Axis.XYZ) translation.y += translationValue;

			// z
			if (!translationRandomization.uniform) translationValue = Random.Range(translationRandomization.min, translationRandomization.max);
			if (axis == Axis.Z || axis == Axis.XZ || axis == Axis.XYZ) translation.z += translationValue;

			Rigidbody rigidBody = translationRandomization.transform.gameObject.GetComponent<Rigidbody>();

			if (rigidBody != null)
			{
				if (rigidBody.isKinematic)
				{
					// the object needs to be non-kinematic such that it can be pushed out of place if overlapping with another object
					// however, it needs to only be non-kinematic for atleast one frame so Unity's physics code runs and moves the object

					rigidBody.isKinematic = false;
					StartCoroutine(ReapplyKinematicRigidbody(rigidBody));
				}

				translationRandomization.transform.Translate(translation);
			}
			else
			{
				translationRandomization.transform.position = translationRandomization.transform.position + translation;
			}
		}
	}

	private IEnumerator ReapplyKinematicRigidbody(Rigidbody rigidBody)
	{
		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();

		if (rigidBody != null) {
			rigidBody.isKinematic = true;
		}
	}

}
