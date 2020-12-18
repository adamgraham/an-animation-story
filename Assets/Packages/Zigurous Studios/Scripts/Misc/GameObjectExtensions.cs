using UnityEngine;
using System;
using System.Reflection;

public static class GameObjectExtensions
{
    public static T AddComponent<T>(this GameObject gameObject, T component) where T : Component
    {
        return gameObject.AddComponent<T>().Copy(component) as T;
    }

    private static T Copy<T>(this Component component, T other) where T : Component
    {
        Type type = component.GetType();
        if (type != other.GetType()) return null; // type mismatch

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] propertyInfos = type.GetProperties(flags);
        for (int i = 0; i < propertyInfos.Length; i++)
        {
            PropertyInfo propertyInfo = propertyInfos[i];

            if (propertyInfo.CanWrite)
            {
                try {
                    propertyInfo.SetValue(component, propertyInfo.GetValue(other, null), null);
                } catch {
                    // do nothing
                }
            }
        }

        FieldInfo[] fieldInfos = type.GetFields(flags);
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            FieldInfo fieldInfo = fieldInfos[i];
            fieldInfo.SetValue(component, fieldInfo.GetValue(other));
        }

        return component as T;
    }

}
