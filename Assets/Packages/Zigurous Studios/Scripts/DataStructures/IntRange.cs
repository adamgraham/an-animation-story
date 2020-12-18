using UnityEngine;

[System.Serializable]
public struct IntRange
{
    public int min;
    public int max;

    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public int RandomValue()
    {
        return UnityEngine.Random.Range(this.min, this.max + 1);
    }

    public int Clamp(int value)
    {
        return UnityEngine.Mathf.Clamp(value, this.min, this.max);
    }

    public int delta
    {
        get {
            return max - min;
        }
    }
    
}
