using UnityEngine;

[System.Serializable]
public struct UIntRange
{
    public uint min;
    public uint max;

    public UIntRange(uint min, uint max)
    {
        this.min = min;
        this.max = max;
    }

    public uint RandomValue()
    {
        return (uint)UnityEngine.Random.Range((int)this.min, (int)this.max + 1);
    }

    public uint Clamp(uint value)
    {
        return (uint)UnityEngine.Mathf.Clamp((int)value, (int)this.min, (int)this.max);
    }
    
}
