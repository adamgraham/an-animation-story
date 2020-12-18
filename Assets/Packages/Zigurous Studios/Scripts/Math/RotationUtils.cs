using UnityEngine;
using System.Collections;

public sealed class RotationUtils
{
	#region Term Guide

	//         Cardinal | North, East, South, West or Up, Right, Down, Left 
	//             Axis | [enum] X, Y, Z or a combination of those
	//        Direction | [enum] Right, Up, Left, Down or a combination of those
	// Direction Vector | [Vector] with values represented in the range -1 to 1
	//      Euler Angle | [Vector] with values represented as degrees
	//            Angle | [float] a value represented as either degrees or radians
	//          Degrees | [float] in the range 0 to 360  | Convert to radians with (PI / 360)
	//          Radians | [float] in the range -PI to PI | Convert to degrees with (360 / PI)

	#endregion

	#region Enums

	public enum Axis { None, X, X_NEG, Y, Y_NEG, Z, Z_NEG, XY, XY_NEG, XZ, XZ_NEG, YZ, YZ_NEG, XYZ, XYZ_NEG }
	public enum Direction { None, Left, Right, Up, UpLeft, UpRight, Down, DownLeft, DownRight, Forward, Backward, Horizontal, Vertical, Diagonal }

	#endregion

	#region Variables

	public const float DEG_TO_RAD = Mathf.Deg2Rad;
	public const float RAD_TO_DEG = Mathf.Rad2Deg;

	public const float ROTATION_ZERO = 0.0f;
	public const float ROTATION_RIGHT = 0.0f;
	public const float ROTATION_UP_RIGHT = 45.0f;
	public const float ROTATION_UP = 90.0f;
	public const float ROTATION_UP_LEFT = 135.0f;
	public const float ROTATION_LEFT = 180.0f;
	public const float ROTATION_DOWN_LEFT = 225.0f;
	public const float ROTATION_DOWN = 270.0f;
	public const float ROTATION_DOWN_RIGHT = 315.0f;

	public static readonly Vector3 DIRECTION_VECTOR_ZERO = Vector3.zero;
	public static readonly Vector3 DIRECTION_VECTOR_RIGHT = Vector3.right;
	public static readonly Vector3 DIRECTION_VECTOR_UP_RIGHT = Vector3.forward + Vector3.right;
	public static readonly Vector3 DIRECTION_VECTOR_UP = Vector3.forward;
	public static readonly Vector3 DIRECTION_VECTOR_UP_LEFT = Vector3.forward + Vector3.left;
	public static readonly Vector3 DIRECTION_VECTOR_LEFT = Vector3.left;
	public static readonly Vector3 DIRECTION_VECTOR_DOWN_LEFT = Vector3.back + Vector3.left;
	public static readonly Vector3 DIRECTION_VECTOR_DOWN = Vector3.back;
	public static readonly Vector3 DIRECTION_VECTOR_DOWN_RIGHT = Vector3.back + Vector3.right;

	public static readonly Vector3 AXIS_VECTOR_NONE = Vector3.zero;
	public static readonly Vector3 AXIS_VECTOR_X = Vector3.right;
	public static readonly Vector3 AXIS_VECTOR_X_NEG = Vector3.left;
	public static readonly Vector3 AXIS_VECTOR_Y = Vector3.up;
	public static readonly Vector3 AXIS_VECTOR_Y_NEG = Vector3.down;
	public static readonly Vector3 AXIS_VECTOR_Z = Vector3.forward;
	public static readonly Vector3 AXIS_VECTOR_Z_NEG = Vector3.back;
	public static readonly Vector3 AXIS_VECTOR_XY = Vector3.right + Vector3.up;
	public static readonly Vector3 AXIS_VECTOR_XY_NEG = Vector3.left + Vector3.down;
	public static readonly Vector3 AXIS_VECTOR_XZ = Vector3.right + Vector3.forward;
	public static readonly Vector3 AXIS_VECTOR_XZ_NEG = Vector3.left + Vector3.back;
	public static readonly Vector3 AXIS_VECTOR_YZ = Vector3.up + Vector3.forward;
	public static readonly Vector3 AXIS_VECTOR_YZ_NEG = Vector3.down + Vector3.back;
	public static readonly Vector3 AXIS_VECTOR_XYZ = Vector3.right + Vector3.up + Vector3.forward;
	public static readonly Vector3 AXIS_VECTOR_XYZ_NEG = Vector3.left + Vector3.down + Vector3.back;

	private static readonly Vector3[] DIRECTION_VECTORS = new Vector3[] 
	{
		DIRECTION_VECTOR_ZERO,			// ( 0,  0,  0) |   0 degrees
		DIRECTION_VECTOR_UP_RIGHT,		// ( 1,  0,  1) |  45 degrees
		DIRECTION_VECTOR_UP,			// ( 0,  0,  1) |  90 degrees
		DIRECTION_VECTOR_UP_LEFT,		// (-1,  0,  1) | 135 degrees
		DIRECTION_VECTOR_LEFT,			// (-1,  0,  0) | 180 degrees
		DIRECTION_VECTOR_DOWN_LEFT,		// (-1,  0, -1) | 225 degrees
		DIRECTION_VECTOR_DOWN,			// ( 0,  0, -1) | 270 degrees
		DIRECTION_VECTOR_DOWN_RIGHT,	// ( 1,  0, -1) | 315 degrees
		DIRECTION_VECTOR_RIGHT,			// ( 1,  0,  0) | 360 degrees
	};

	private static readonly Vector3[] AXIS_VECTORS = new Vector3[] 
	{ 
		AXIS_VECTOR_NONE,		// Axis.None	|	( 0,  0,  0)
		AXIS_VECTOR_X,			// Axis.X		|	( 1,  0,  0)
		AXIS_VECTOR_X_NEG,		// Axis.X_NEG	|	(-1,  0,  0)
		AXIS_VECTOR_Y,			// Axis.Y		|	( 0,  1,  0)
		AXIS_VECTOR_Y_NEG,		// Axis.Y_NEG	|	( 0, -1,  0)
		AXIS_VECTOR_Z,			// Axis.Z		|	( 0,  0,  1)
		AXIS_VECTOR_Z_NEG,		// Axis.Z_NEG	|	( 0,  0, -1)
		AXIS_VECTOR_XY,			// Axis.XY		|	( 1,  1,  0)
		AXIS_VECTOR_XY_NEG,		// Axis.XY_NEG	|	(-1, -1,  0)
		AXIS_VECTOR_XZ,			// Axis.XZ		|	( 1,  0,  1)
		AXIS_VECTOR_XZ_NEG,		// Axis.XZ_NEG	|	(-1,  0, -1)
		AXIS_VECTOR_YZ,			// Axis.YZ		|	( 0,  1,  1)
		AXIS_VECTOR_YZ_NEG,		// Axis.YZ_NEG	|	( 0, -1, -1)
		AXIS_VECTOR_XYZ,		// Axis.XYZ		|	( 1,  1,  1)
		AXIS_VECTOR_XYZ_NEG,	// Axis.XYZ_NEG	|	(-1, -1, -1)
	};

	private static Hashtable _directionAngles;
	private static Hashtable _angleDirectionEnums;
	private static Hashtable _angleDirectionVectors;

	private static Hashtable _axisEnums;
	private static Hashtable _axisVectors;

	#endregion

	#region Lazy Loading

	private static Hashtable directionAngles
	{
		get
		{
			if (_directionAngles == null)
			{
				_directionAngles = new Hashtable();
				_directionAngles[DIRECTION_VECTOR_ZERO] = ROTATION_ZERO;
				_directionAngles[DIRECTION_VECTOR_RIGHT] = ROTATION_RIGHT;
				_directionAngles[DIRECTION_VECTOR_UP_RIGHT] = ROTATION_UP_RIGHT;
				_directionAngles[DIRECTION_VECTOR_UP] = ROTATION_UP;
				_directionAngles[DIRECTION_VECTOR_UP_LEFT] = ROTATION_UP_LEFT;
				_directionAngles[DIRECTION_VECTOR_LEFT] = ROTATION_LEFT;
				_directionAngles[DIRECTION_VECTOR_DOWN_LEFT] = ROTATION_DOWN_LEFT;
				_directionAngles[DIRECTION_VECTOR_DOWN] = ROTATION_DOWN;
				_directionAngles[DIRECTION_VECTOR_DOWN_RIGHT] = ROTATION_DOWN_RIGHT;
			}

			return _directionAngles;
		}
	}

	private static Hashtable angleDirectionEnums
	{
		get
		{
			if (_angleDirectionEnums == null)
			{
				_angleDirectionEnums = new Hashtable();
				_angleDirectionEnums[ROTATION_ZERO] = Direction.Right;
				_angleDirectionEnums[ROTATION_RIGHT] = Direction.Right;
				_angleDirectionEnums[ROTATION_UP_RIGHT] = Direction.UpRight;
				_angleDirectionEnums[ROTATION_UP] = Direction.Up;
				_angleDirectionEnums[ROTATION_UP_LEFT] = Direction.UpLeft;
				_angleDirectionEnums[ROTATION_LEFT] = Direction.Left;
				_angleDirectionEnums[ROTATION_DOWN_LEFT] = Direction.DownLeft;
				_angleDirectionEnums[ROTATION_DOWN] = Direction.Down;
				_angleDirectionEnums[ROTATION_DOWN_RIGHT] = Direction.DownRight;
			}

			return _angleDirectionEnums;
		}
	}

	private static Hashtable angleDirectionVectors
	{
		get
		{
			if (_angleDirectionVectors == null)
			{
				_angleDirectionVectors = new Hashtable();
				_angleDirectionVectors[ROTATION_ZERO] = DIRECTION_VECTOR_ZERO;
				_angleDirectionVectors[ROTATION_RIGHT] = DIRECTION_VECTOR_RIGHT;
				_angleDirectionVectors[ROTATION_UP_RIGHT] = DIRECTION_VECTOR_UP_RIGHT;
				_angleDirectionVectors[ROTATION_UP] = DIRECTION_VECTOR_UP;
				_angleDirectionVectors[ROTATION_UP_LEFT] = DIRECTION_VECTOR_UP_LEFT;
				_angleDirectionVectors[ROTATION_LEFT] = DIRECTION_VECTOR_LEFT;
				_angleDirectionVectors[ROTATION_DOWN_LEFT] = DIRECTION_VECTOR_DOWN_LEFT;
				_angleDirectionVectors[ROTATION_DOWN] = DIRECTION_VECTOR_DOWN;
				_angleDirectionVectors[ROTATION_DOWN_RIGHT] = DIRECTION_VECTOR_DOWN_RIGHT;
			}

			return _angleDirectionVectors;
		}
	}

	private static Hashtable axisEnums
	{
		get
		{
			if (_axisEnums == null)
			{
				_axisEnums = new Hashtable();
				_axisEnums[AXIS_VECTOR_NONE] = Axis.None;
				_axisEnums[AXIS_VECTOR_X] = Axis.X;
				_axisEnums[AXIS_VECTOR_X_NEG] = Axis.X_NEG;
				_axisEnums[AXIS_VECTOR_Y] = Axis.Y;
				_axisEnums[AXIS_VECTOR_Y_NEG] = Axis.Y_NEG;
				_axisEnums[AXIS_VECTOR_Z] = Axis.Z;
				_axisEnums[AXIS_VECTOR_Z_NEG] = Axis.Z_NEG;
				_axisEnums[AXIS_VECTOR_XY] = Axis.XY;
				_axisEnums[AXIS_VECTOR_XY_NEG] = Axis.XY_NEG;
				_axisEnums[AXIS_VECTOR_XZ] = Axis.XZ;
				_axisEnums[AXIS_VECTOR_XZ_NEG] = Axis.XZ_NEG;
				_axisEnums[AXIS_VECTOR_YZ] = Axis.YZ;
				_axisEnums[AXIS_VECTOR_YZ_NEG] = Axis.YZ_NEG;
				_axisEnums[AXIS_VECTOR_XYZ] = Axis.XYZ;
				_axisEnums[AXIS_VECTOR_XYZ_NEG] = Axis.XYZ_NEG;
			}

			return _axisEnums;
		}
	}

	private static Hashtable axisVectors
	{
		get
		{
			if ( _axisVectors == null )
			{
				_axisVectors = new Hashtable();
				_axisVectors[Axis.None] = AXIS_VECTOR_NONE;
				_axisVectors[Axis.X] = AXIS_VECTOR_X;
				_axisVectors[Axis.X_NEG] = AXIS_VECTOR_X_NEG;
				_axisVectors[Axis.Y] = AXIS_VECTOR_Y;
				_axisVectors[Axis.Y_NEG] = AXIS_VECTOR_Y_NEG;
				_axisVectors[Axis.Z] = AXIS_VECTOR_Z;
				_axisVectors[Axis.Z_NEG] = AXIS_VECTOR_Z_NEG;
				_axisVectors[Axis.XY] = AXIS_VECTOR_XY;
				_axisVectors[Axis.XY_NEG] = AXIS_VECTOR_XY_NEG;
				_axisVectors[Axis.XZ] = AXIS_VECTOR_XZ;
				_axisVectors[Axis.XZ_NEG] = AXIS_VECTOR_XZ_NEG;
				_axisVectors[Axis.YZ] = AXIS_VECTOR_YZ;
				_axisVectors[Axis.YZ_NEG] = AXIS_VECTOR_YZ_NEG;
				_axisVectors[Axis.XYZ] = AXIS_VECTOR_XYZ;
				_axisVectors[Axis.XYZ_NEG] = AXIS_VECTOR_XYZ_NEG;
			}

			return _axisVectors;
		}
	}

	#endregion

	#region Utilities: Reverse Angle

	/** 
	 * Returns an angle + 180 (in degrees), which is normalized between 0 and 360. 
	 * (e.g.  90 degrees returns 270 degrees) 
	 * (e.g. 225 degrees returns  45 degrees) 
	 * (e.g. -45 degrees returns 135 degrees)
	 */
	public static float GetReverseAngle(float degrees)
	{
		return (degrees + 180.0f) % 360.0f;
	}

	#endregion

	#region Utilities: Angle Normalization

	/** 
	 * Returns an angle (in degrees) that is normalized to the range 0 to 360. 
	 * (e.g. 405 degrees returns 45 degrees) 
	 * (e.g. -45 degrees returns 315 degrees)
	 */
	public static float NormalizeDegrees0To360(float degrees)
	{
		degrees = degrees % 360.0f;
		if (degrees < 0.0f) degrees += 360.0f;
		return degrees;
	}

	/** 
	 * Outputs a vector in which each component is normalized to the range 0 to 360. 
	 * (e.g. 405 degrees returns 45 degrees) 
	 * (e.g. -45 degrees returns 315 degrees)
	 */
	public static void NormalizeDegrees0To360(Vector3 degrees, out Vector3 outVector)
	{
		outVector.x = NormalizeDegrees0To360(degrees.x);
		outVector.y = NormalizeDegrees0To360(degrees.y);
		outVector.z = NormalizeDegrees0To360(degrees.z);
	}

	#endregion

	#region Utilities: Coordinate System

	/** 
	 * Mirrors a X-axis rotation across the YZ plane. 
	 * This is a way of flipping between LHS and RHS coordinate systems.
	 */
	public static Quaternion MirrorXAxis(Quaternion quaternion)
	{
		return new Quaternion(-quaternion.x, quaternion.y, quaternion.z, -quaternion.w);
	}

	/** 
	 * Mirrors a Y-axis rotation across the XZ plane. 
	 * This is a way of flipping between LHS and RHS coordinate systems.
	 */
	public static Quaternion MirrorYAxis(Quaternion quaternion)
	{
		return new Quaternion(quaternion.x, -quaternion.y, quaternion.z, -quaternion.w);
	}

	/** 
	 * Mirrors a Z-axis rotation across the XY plane. 
	 * This is a way of flipping between LHS and RHS coordinate systems.
	 */
	public static Quaternion MirrorZAxis(Quaternion quaternion)
	{
		return new Quaternion(quaternion.x, quaternion.y, -quaternion.z, -quaternion.w);
	}

	#endregion

	#region Utilities: Direction Rounding

	/** 
	 * Rounds a direction vector to the nearest cardinal.
	 * NOTE: This includes diagonals.
	 */
	public static void RoundDirectionVector(Vector3 inDirection, out Vector3 outDirection)
	{
		float x = inDirection.x;
		float y = inDirection.y;
		float z = inDirection.z;
		float absX = Mathf.Abs(x);
		float absY = Mathf.Abs(y);
		float absZ = Mathf.Abs(z);
		float max = Mathf.Max(Mathf.Max(absX, absY), absZ);

		if (max.IsNotZero())
		{
			x /= max;
			y /= max;
			z /= max;
		}

		outDirection.x = (x.IsEqualTo(1.0f) || x.IsEqualTo(-1.0f)) ? x : 0.0f;
		outDirection.y = (y.IsEqualTo(1.0f) || y.IsEqualTo(-1.0f)) ? y : 0.0f;
		outDirection.z = (z.IsEqualTo(1.0f) || z.IsEqualTo(-1.0f)) ? z : 0.0f;
	}

	#endregion

	#region Utilities: Axis Rotations

	/** 
	 * Returns the euler angle X Axis rotation that corresponds to a direction vector. 
	 * NOTE: Direction vectors will be rounded to the nearest cardinal.
	 */
	public static void GetXAxisRotation(Vector3 direction, out Vector3 eulerAngle)
	{
		// To get the proper X Axis rotation, only the Y and Z axis can be set
		direction.x = 0.0f;
		RoundDirectionVector(direction, out direction);

		eulerAngle.x = -GetDirectionAngle(direction);
		eulerAngle.y = 0.0f;
		eulerAngle.z = 0.0f;
		eulerAngle = Quaternion.Euler(eulerAngle).eulerAngles;
	}

	/** 
	 * Returns the euler angle Y Axis rotation that corresponds to a direction vector. 
	 * NOTE: Direction vectors will be rounded to the nearest cardinal.
	 */
	public static void GetYAxisRotation(Vector3 direction, out Vector3 eulerAngle)
	{
		// To get the proper Y Axis rotation, only the X and Z axis can be set
		direction.y = 0.0f;
		RoundDirectionVector(direction, out direction);

		eulerAngle.x = 0.0f;
		eulerAngle.y = -GetDirectionAngle(direction);
		eulerAngle.z = 0.0f;
		eulerAngle = Quaternion.Euler(eulerAngle).eulerAngles;
	}

	/** 
	 * Returns the euler angle Z Axis rotation that corresponds to a direction vector. 
	 * NOTE: Direction vectors will be rounded to the nearest cardinal.
	 */
	public static void GetZAxisRotation(Vector3 direction, out Vector3 eulerAngle)
	{
		// To get the proper Z Axis rotation, only the X and Y axis can be set
		direction.z = 0.0f;
		RoundDirectionVector(direction, out direction);

		eulerAngle.x = 0.0f;
		eulerAngle.y = 0.0f;
		eulerAngle.z = -GetDirectionAngle(direction);
		eulerAngle = Quaternion.Euler(eulerAngle).eulerAngles;
	}

	#endregion

	#region Utilities: Angle To Direction | Direction to Angle

	/** 
	 * Returns the rotation angle (in degrees) of a direction vector. 
	 * (e.g. <0, 0, 1> returns 90 degrees)
	 */
	public static float GetDirectionAngle(Vector3 direction)
	{
		RoundDirectionVector(direction, out direction);
		return (float)directionAngles[direction];
	}

	/** 
	 * Returns a Direction enum that corresponds to an angle (in degrees). 
	 * (e.g.   0 degrees returns Direction.Right) 
	 * (e.g.  90 degrees returns Direction.Up) 
	 * (e.g. 180 degrees returns Direction.Left) 
	 * (e.g. 270 degrees returns Direction.Down)
	 */
	public static Direction GetAngleDirection(float degrees)
	{
		return (Direction)angleDirectionEnums[degrees];
	}

	/** 
	 * Returns a direction vector that corresponds to an angle (in degrees). 
	 * (e.g.   0 degrees returns < 1,  0,  0>) 
	 * (e.g.  90 degrees returns < 0,  0,  1>) 
	 * (e.g. 180 degrees returns <-1,  0,  0>) 
	 * (e.g. 270 degrees returns < 0,  0, -1>) 
	 */
	public static void GetAngleDirectionVector(float degrees, out Vector3 direction)
	{
		Vector3 temp = (Vector3)angleDirectionVectors[degrees];
		direction.x = temp.x;
		direction.y = temp.y;
		direction.z = temp.z;
	}

	#endregion

	#region Utilities: Axis To Direction | Direction to Axis

	/** 
	 * Outputs the direction vector that corresponds to an Axis. 
	 * (e.g.      Axis.X returns < 1,  0,  0>) 
	 * (e.g.     Axis.XZ returns < 1,  0,  1>) 
	 * (e.g. Axis.XZ_NEG returns <-1,  0, -1>)
	 */
	public static void GetAxisDirectionVector(Axis axis, out Vector3 direction)
	{
		Vector3 temp = (Vector3)axisVectors[axis];
		direction.x = temp.x;
		direction.y = temp.y;
		direction.z = temp.z;
	}

	/** 
	 * Returns the Axis that corresponds to a direction vector.
	 * NOTE: Direction vectors will be rounded to the nearest cardinal.
	 * (e.g. < 1,  0,  0> returns Axis.X) 
	 * (e.g. < 1,  0,  1> returns Axis.XZ) 
	 * (e.g. <-1,  0, -1> returns Axis.XZ_NEG)
	 */
	public static Axis GetDirectionVectorAxis(Vector3 direction)
	{
		RoundDirectionVector(direction, out direction);
		return (Axis)axisEnums[direction];
	}

	#endregion

}
