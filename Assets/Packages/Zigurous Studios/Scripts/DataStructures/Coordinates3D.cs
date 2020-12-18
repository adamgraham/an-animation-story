using UnityEngine;
using System;

[System.Serializable]
public struct Coordinates3D : IComparable<Coordinates3D>, IEquatable<Coordinates3D>
{
	public int x;
	public int y;
	public int z;

	public static Coordinates3D zero = new Coordinates3D(0, 0, 0);

	public Coordinates3D(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public double DistanceTo(Coordinates3D other)
	{
		int dx = this.x - other.x;
		int dy = this.y - other.y;
		int dz = this.z - other.z;

		return Math.Sqrt(((dx * dx) + (dy * dy) + (dz * dz)));
	}

    public int CompareTo(Coordinates3D obj)
    {
        Coordinates3D a = this;
        Coordinates3D b = obj;

        if ((a.x == b.x) && (a.y == b.y) && (a.z == b.z)) {
            return 0;
        }

        if ((a.x < b.x) || ((a.x == b.x) && (a.y < b.y)) || ((a.x == b.x) && (a.y == b.y) && (a.z < b.z))) {
            return -1;
        }

        return 1;
    }

    public bool Equals(Coordinates3D obj)
    {
        return this.GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        unchecked // overflow is fine, just wrap
        {
            int offset = 32749;
            int size = offset * offset;
            int x = this.x + offset;
            int y = this.y + offset;
            return x + size * (y + size * z);
        }
    }

    public override string ToString()
    {
        return "(" + this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString() + ")";
    }

    public Vector3 ToVector3()
    {
        return new Vector3(this.x, this.y, this.z);
    }

}
