using UnityEngine;

[System.Serializable]
public struct Vector3Range
{
    public Vector3 min;
    public Vector3 max;

    public Vector3 RandomValue()
    {
        float x = UnityEngine.Random.Range(this.min.x, this.max.x);
        float y = UnityEngine.Random.Range(this.min.y, this.max.y);
        float z = UnityEngine.Random.Range(this.min.z, this.max.z);
        
        return new Vector3(x, y, z);
    }

    public Vector3 Clamp(Vector3 value)
    {
        return value.Clamped(this.min, this.max);
    }
    
}
