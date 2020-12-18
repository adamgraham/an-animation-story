using UnityEngine;

public static class FloatExtensions
{
	public static bool IsZero(this float value)
	{
		return (value > -float.Epsilon) && (value < float.Epsilon);
	}

	public static bool IsZero(this float value, float epsilon)
	{
		return (value > -epsilon) && (value < epsilon);
	}

	public static bool IsNotZero(this float value)
	{
		return !value.IsZero();
	}

	public static bool IsNotZero(this float value, float epsilon)
	{
		return !value.IsZero(epsilon);
	}

	public static bool IsEqualTo(this float lhs, float rhs)
	{
		return Mathf.Abs(lhs - rhs) < float.Epsilon;
	}

	public static bool IsEqualTo(this float lhs, float rhs, float epsilon)
	{
		return Mathf.Abs(lhs - rhs) < epsilon;
	}

	public static bool IsNotEqualTo(this float lhs, float rhs)
	{
		return !lhs.IsEqualTo(rhs);
	}

	public static bool IsNotEqualTo(this float lhs, float rhs, float epsilon)
	{
		return !lhs.IsEqualTo(rhs, epsilon);
	}

}
