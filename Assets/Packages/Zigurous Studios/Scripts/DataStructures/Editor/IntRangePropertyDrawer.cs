using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IntRange))]
public class IntRangePropertyDrawer : PropertyDrawer 
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
            int value = EditorGUI.IntField(position, "Min", _min.intValue);
            
            if (EditorGUI.EndChangeCheck()) {
                _min.intValue = value;
            }
        }
        EditorGUI.EndProperty();
        position.x += half;

        // Max
		EditorGUIUtility.labelWidth = 28.0f;
        EditorGUI.BeginProperty(position, label, _max);
        {
            EditorGUI.BeginChangeCheck();
            int value = EditorGUI.IntField(position, "Max", _max.intValue);

            if (EditorGUI.EndChangeCheck()) {
                _max.intValue = value;
            }
        }
        EditorGUI.EndProperty();


        // Reset editor properties
        EditorGUI.indentLevel = indent;
        EditorGUIUtility.labelWidth = labelWidth;
    }

}
