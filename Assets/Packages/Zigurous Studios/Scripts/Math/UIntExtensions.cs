using UnityEngine;
using System.Collections.Generic;

public static class UIntExtensions 
{
	public static IEnumerable<uint> GetFactors(this uint n)
	{
		for (uint x = 1; x*x <= n; x++)
		{
			if (n % x == 0) 
			{
				yield return x;
				
				if (x != (n / x)) {
					yield return n / x;
				}
			}
		}
	}

	public static bool IsEven(this uint n)
	{
		return n % 2 == 0;
	}

	public static bool IsOdd(this uint n)
	{
		return n % 2 != 0;
	}

}
