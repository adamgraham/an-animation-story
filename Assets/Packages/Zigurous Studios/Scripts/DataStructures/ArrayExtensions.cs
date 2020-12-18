using System;

public static class ArrayExtensions
{
	public static void ForEach<T>(this T[] array, Action<T> action)
	{
		for (int i = 0; i < array.Length; i++) {
			action(array[i]);
		}
	}

	public static int IndexOf<T>(this T[] array, T element, uint startIndex = 0) where T : IComparable<T> 
	{
		if (element == null) {
			return -1;
		}

		for (int i = (int)startIndex; i < array.Length; i++)
		{
			if (element.CompareTo(array[i]) == 0) {
				return i;
			}
		}

		return -1;
	}

	public static bool Contains<T>(this T[] array, T element, uint startIndex = 0) where T : IComparable<T> 
	{
		return array.IndexOf(element, startIndex) >= 0;
	}

	public static bool IsEmpty<T>(this T[] array)
	{
		return array.Length <= 0;
	}

	public static T[] Filter<T>(this T[] array, Predicate<T> match)
	{
		return Array.FindAll(array, match);
	}

	public static void Sort<T>(this T[] array, Comparison<T> comparison)
	{
		Array.Sort(array, comparison);
	}

	public static void Reverse<T>(this T[] array)
	{
		Array.Reverse(array);
	}

	public static T[] Flatten<T>(this T[,] array)
	{
		int width = array.GetLength(0);
		int height = array.GetLength(1);

		T[] flat = new T[width * height];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				T element = array[x, y];
				flat[x * width + y] = element;
			}
		}

		return flat;
	}

	public static T[] Flatten<T>(this T[,,] array)
	{
		int width = array.GetLength(0);
		int height = array.GetLength(1);
		int depth = array.GetLength(2);

		T[] flat = new T[width * height * depth];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < depth; z++)
				{
					T element = array[x, y, z];
					flat[x + width * (y + height * z)] = element;
				}
			}
		}

		return flat;
	}

	public static T RandomElement<T>(this T[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

}
