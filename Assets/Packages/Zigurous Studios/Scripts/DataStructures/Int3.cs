[System.Serializable]
public struct Int3 
{
	public static Int3 zero = new Int3(0, 0, 0);
	public static Int3 one = new Int3(1, 1, 1);
	public static Int3 right = new Int3(1, 0, 0);
	public static Int3 left = new Int3(-1, 0, 0);
	public static Int3 up = new Int3(0, 1, 0);
	public static Int3 down = new Int3(0, -1, 0);
	public static Int3 forward = new Int3(0, 0, 1);
	public static Int3 backward = new Int3(0, 0, -1);
	
	public int x;
	public int y;
	public int z;

	public Int3(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

}
