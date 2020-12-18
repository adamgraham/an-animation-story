using UnityEngine;
using System;

[System.Serializable]
public struct Coordinates2D : IComparable<Coordinates2D>, IEquatable<Coordinates2D>
{
	public int x;
	public int y;

	public static Coordinates2D zero = new Coordinates2D(0, 0);

	public Coordinates2D(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public double DistanceTo(Coordinates2D other)
	{
		int dx = this.x - other.x;
		int dy = this.y - other.y;

		return Math.Sqrt((double)((dx * dx) + (dy * dy)));
	}

	public int CompareTo(Coordinates2D obj)
	{
		Coordinates2D a = this;
		Coordinates2D b = obj;

		if ((a.x == b.x) && (a.y == b.y)) {
            return 0;
		}

        if ((a.x < b.x) || ((a.x == b.x) && (a.y < b.y))) {
            return -1;
		}

        return 1;
	}

    public bool Equals(Coordinates2D obj)
    {
        return this.GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        unchecked // overflow is fine, just wrap
        {
            int offset = 32749;
            int width = offset * offset;
            int x = this.x + offset;
            int y = this.y + offset;
            return x * width + y;
        }
    }

    public override string ToString()
    {
        return "(" + this.x.ToString() + ", " + this.y.ToString() + ")";
    }

    public Vector2 ToVector2()
    {
        return new Vector2(this.x, this.y);
    }

}
