[System.Serializable]
public struct UInt3 
{
	public static UInt3 zero = new UInt3(0, 0, 0);
	public static UInt3 one = new UInt3(1, 1, 1);
	public static UInt3 right = new UInt3(1, 0, 0);
	public static UInt3 up = new UInt3(0, 1, 0);
	public static UInt3 forward = new UInt3(0, 0, 1);
	
	public uint x;
	public uint y;
	public uint z;

	public UInt3(uint x, uint y, uint z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public uint volume
	{
		get {
			return this.x * this.y * this.z;
		}
	}
	
}
