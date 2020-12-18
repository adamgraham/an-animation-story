using UnityEngine;
using UnityEditor;

public sealed class GradientHorizonSunSkyboxEditor : MaterialEditor
{
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        if (this.isVisible)
        {
            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Background Parameters");

            EditorGUILayout.Space();

            ColorProperty(GetMaterialProperty(this.targets, "_SkyColor1"), "Top Color");
            FloatProperty(GetMaterialProperty(this.targets, "_SkyExponent1"), "Exponential Factor");

            EditorGUILayout.Space();

            ColorProperty(GetMaterialProperty(this.targets, "_SkyColor2"), "Horizon Color");

            EditorGUILayout.Space();

            ColorProperty(GetMaterialProperty(this.targets, "_SkyColor3"), "Bottom Color");
            FloatProperty(GetMaterialProperty(this.targets, "_SkyExponent2"), "Exponential Factor");

            EditorGUILayout.Space();

            FloatProperty(GetMaterialProperty(this.targets, "_SkyIntensity"), "Intensity");

            EditorGUILayout.Space();

            GUILayout.Label("Sun Parameters");

            EditorGUILayout.Space();

            ColorProperty(GetMaterialProperty(this.targets, "_SunColor"), "Color");
            FloatProperty(GetMaterialProperty(this.targets, "_SunIntensity"), "Intensity");

            EditorGUILayout.Space();

            FloatProperty(GetMaterialProperty(this.targets, "_SunAlpha"), "Alpha");
            FloatProperty(GetMaterialProperty(this.targets, "_SunBeta"), "Beta");

            EditorGUILayout.Space();

            var azimuth = GetMaterialProperty(this.targets, "_SunAzimuth");
            var altitude = GetMaterialProperty(this.targets, "_SunAltitude");

            if (azimuth.hasMixedValue || altitude.hasMixedValue)
            {
                EditorGUILayout.HelpBox("Editing angles is disabled because they have mixed values.", MessageType.Warning);
            }
            else
            {
                FloatProperty(azimuth, "Azimuth");
                FloatProperty(altitude, "Altitude");
            }

            if (EditorGUI.EndChangeCheck())
            {
                var azimuthRadians = azimuth.floatValue * Mathf.Deg2Rad;
                var altitudeRadians = altitude.floatValue * Mathf.Deg2Rad;
                
                var upVector = new Vector4(
                    Mathf.Cos(altitudeRadians) * Mathf.Sin(azimuthRadians),
                    Mathf.Sin(altitudeRadians),
                    Mathf.Cos(altitudeRadians) * Mathf.Cos(azimuthRadians),
                    0.0f
                );

                GetMaterialProperty(this.targets, "_SunVector").vectorValue = upVector;
                PropertiesChanged();
            }
        }
    }

}
