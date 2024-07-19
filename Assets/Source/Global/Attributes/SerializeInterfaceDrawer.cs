#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(SerializeInterfaceAttribute))]
public class SerializeInterfaceDrawer : PropertyDrawer
{
    private const string Error = "";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (IsValidField() == false)
        {
            EditorGUI.HelpBox(position, Error, MessageType.Error);
            return;
        }

        Type type = (attribute as SerializeInterfaceAttribute)?.Type;

        UpdatePropertyValue(property, type);
        UpdateDropIcon(position, type);

        property.objectReferenceValue = EditorGUI.ObjectField(
            position,
            label,
            property.objectReferenceValue,
            typeof(GameObject),
            true);
    }

    private bool IsValidField()
    {
        return fieldInfo.FieldType == typeof(GameObject) ||
               typeof(IEnumerable<GameObject>).IsAssignableFrom(fieldInfo.FieldType);
    }

    private bool IsInvalidObject(Object @object, Type required)
    {
        if (@object is GameObject gameObject)
            return gameObject.GetComponent(required) == null;

        return true;
    }

    private void UpdatePropertyValue(SerializedProperty property, Type required)
    {
        if (property.objectReferenceValue == null)
            return;

        if (IsInvalidObject(property.objectReferenceValue, required))
            property.objectReferenceValue = null;
    }

    private void UpdateDropIcon(Rect position, Type required)
    {
        if (position.Contains(Event.current.mousePosition) != false)
            return;

        foreach (Object reference in DragAndDrop.objectReferences)
        {
            if (IsInvalidObject(reference, required))
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
        }
    }
}
#endif