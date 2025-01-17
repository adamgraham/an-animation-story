﻿using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(UInt3))]
public class UInt3PropertyDrawer : PropertyDrawer 
{
    private SerializedProperty _x;
    private SerializedProperty _y;
    private SerializedProperty _z;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (_x == null) { _x = property.FindPropertyRelative("x"); }
        if (_y == null) { _y = property.FindPropertyRelative("y"); }
        if (_z == null) { _z = property.FindPropertyRelative("z"); }
        

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
        float third = position.width / 3.0f;
        position.width /= 3.0f;

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 13.0f;


        // X
        EditorGUI.BeginProperty(position, label, _x);
        {
            EditorGUI.BeginChangeCheck();
            uint value = (uint)EditorGUI.IntField(position, "X", _x.intValue);
            
            if (EditorGUI.EndChangeCheck()) {
                _x.intValue = (int)value;
            }
        }
        EditorGUI.EndProperty();
        position.x += third;

        // Y
        EditorGUI.BeginProperty(position, label, _y);
        {
            EditorGUI.BeginChangeCheck();
            uint value = (uint)EditorGUI.IntField(position, "Y", _y.intValue);

            if (EditorGUI.EndChangeCheck()) {
                _y.intValue = (int)value;
            }
        }
        EditorGUI.EndProperty();
        position.x += third;

        // Z
        EditorGUI.BeginProperty(position, label, _z);
        {
            EditorGUI.BeginChangeCheck();
            uint value = (uint)EditorGUI.IntField(position, "Z", _z.intValue);

            if (EditorGUI.EndChangeCheck()) {
                _z.intValue = (int)value;
            }
        }
        EditorGUI.EndProperty();


        // Reset editor properties
        EditorGUI.indentLevel = indent;
        EditorGUIUtility.labelWidth = labelWidth;
    }

}
