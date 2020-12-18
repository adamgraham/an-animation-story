using UnityEngine;

public static class VectorExtensions 
{
	public static Vector3 AbsoluteValue(this Vector3 vector)
	{
		vector.x = Mathf.Abs(vector.x);
		vector.y = Mathf.Abs(vector.y);
		vector.z = Mathf.Abs(vector.z);

		return vector;
	}

	public static Vector3 Clamped(this Vector3 vector, Vector3 min, Vector3 max)
	{
		vector.x = Mathf.Clamp(vector.x, min.x, max.x);
		vector.y = Mathf.Clamp(vector.y, min.y, max.y);
		vector.z = Mathf.Clamp(vector.z, min.z, max.z);

		return vector;
	}

	public static Vector3 Floored(this Vector3 vector)
	{
		vector.x = Mathf.Floor(vector.x);
		vector.y = Mathf.Floor(vector.y);
		vector.z = Mathf.Floor(vector.z);

		return vector;
	}

	public static Vector3 Rounded(this Vector3 vector)
	{
		vector.x = Mathf.Round(vector.x);
		vector.y = Mathf.Round(vector.y);
		vector.z = Mathf.Round(vector.z);

		return vector;
	}

	public static Vector3 Ceiled(this Vector3 vector)
	{
		vector.x = Mathf.Ceil(vector.x);
		vector.y = Mathf.Ceil(vector.y);
		vector.z = Mathf.Ceil(vector.z);

		return vector;
	}

	public static bool IsZero(this Vector3 vector)
	{
		return vector.x.IsZero() && 
			   vector.y.IsZero() && 
			   vector.z.IsZero();
	}

	public static bool IsZero(this Vector3 vector, float epsilon)
	{
		return vector.x.IsZero(epsilon) && 
			   vector.y.IsZero(epsilon) && 
			   vector.z.IsZero(epsilon);
	}

	public static bool IsNotZero(this Vector3 vector)
	{
		return !vector.IsZero();
	}

	public static bool IsNotZero(this Vector3 vector, float epsilon)
	{
		return !vector.IsZero(epsilon);
	}

	public static bool IsEqualTo(this Vector3 lhs, Vector3 rhs)
	{
		return lhs.x.IsEqualTo(rhs.x) && 
			   lhs.y.IsEqualTo(rhs.y) && 
			   lhs.z.IsEqualTo(rhs.z);
	}

	public static bool IsEqualTo(this Vector3 lhs, Vector3 rhs, float epsilon)
	{
		return lhs.x.IsEqualTo(rhs.x, epsilon) && 
			   lhs.y.IsEqualTo(rhs.y, epsilon) && 
			   lhs.z.IsEqualTo(rhs.z, epsilon);
	}

	public static bool IsNotEqualTo(Vector3 lhs, Vector3 rhs)
	{
		return !lhs.IsEqualTo(rhs);
	}

	public static bool IsNotEqualTo(Vector3 lhs, Vector3 rhs, float epsilon)
	{
		return !lhs.IsEqualTo(rhs, epsilon);
	}

}
