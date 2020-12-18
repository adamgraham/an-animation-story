[System.Serializable]
public struct Bool3 
{
	public static Bool3 zero = new Bool3(false, false, false);
	public static Bool3 one = new Bool3(true, true, true);
	public static Bool3 right = new Bool3(true, false, false);
	public static Bool3 up = new Bool3(false, true, false);
	public static Bool3 forward = new Bool3(false, false, true);

	public bool x;
	public bool y;
	public bool z;

	public Bool3(bool x, bool y, bool z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

}
