using UnityEngine;

[System.Serializable]
public struct FloatRange
{
    public float min;
    public float max;

    public FloatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float RandomValue()
    {
        return UnityEngine.Random.Range(this.min, this.max);
    }

    public float Clamp(float value)
    {
        return UnityEngine.Mathf.Clamp(value, this.min, this.max);
    }

}
