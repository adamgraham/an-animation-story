using UnityEngine;

public sealed class SkyboxRotator : MonoBehaviour
{
    public float speed = 1.0f;

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * this.speed);
    }

}
