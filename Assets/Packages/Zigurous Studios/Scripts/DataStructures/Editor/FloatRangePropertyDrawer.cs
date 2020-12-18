using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangePropertyDrawer : PropertyDrawer 
{
    private SerializedProperty _min;
    private SerializedProperty _max;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (_min == null) { _min = property.FindPropertyRelative("min"); }
        if (_max == null) { _max = property.FindPropertyRelative("max"); }


        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
		float half = position.width / 2.0f;
        position.width /= 2.0f;
		position.width -= 8.0f;

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float labelWidth = EditorGUIUtility.labelWidth;


        // Min
		EditorGUIUtility.labelWidth = 24.0f;
        EditorGUI.BeginProperty(position, label, _min);
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorGUI.FloatField(position, "Min", _min.floatValue);
            
            if (EditorGUI.EndChangeCheck()) {
                _min.floatValue = value;
            }
        }
        EditorGUI.EndProperty();
        position.x += half;

        // Max
		EditorGUIUtility.labelWidth = 28.0f;
        EditorGUI.BeginProperty(position, label, _max);
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorGUI.FloatField(position, "Max", _max.floatValue);

            if (EditorGUI.EndChangeCheck()) {
                _max.floatValue = value;
            }
        }
        EditorGUI.EndProperty();


        // Reset editor properties
        EditorGUI.indentLevel = indent;
        EditorGUIUtility.labelWidth = labelWidth;
    }

}
