using UnityEngine;

public static class ColorExtensions
{
    public static Color ColorWithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static bool IsEqualTo(this Color lhs, Color rhs, bool compareAlpha = false)
	{
		bool isEqual = false;

		if ((int)(lhs.r * 1000.0f) == (int)(rhs.r * 1000.0f))
		{
			if ((int)(lhs.g * 1000.0f) == (int)(rhs.g * 1000.0f))
			{
				if ((int)(lhs.b * 1000.0f) == (int)(rhs.b * 1000.0f))
				{
					if (!compareAlpha) {
						isEqual = true;
                    } else if ((int)(lhs.a * 1000.0f) == (int)(rhs.a * 1000.0f)) {
                        isEqual = true;
					}
				}
			}
		}

		return isEqual;
	}

}
