using System;
using UnityEditor;
using UnityEngine;

public class ConditionalHeaderAttribute : PropertyAttribute
{
    public string Header { get; private set; }
    public string ConditionFieldName { get; private set; }
    public float SpaceAfterHeader { get; private set; }

    public ConditionalHeaderAttribute(string header, string conditionFieldName, float spaceAfterHeader = 0)
    {
        Header = header;
        ConditionFieldName = conditionFieldName;
        SpaceAfterHeader = spaceAfterHeader;
    }
}

[CustomPropertyDrawer(typeof(ConditionalHeaderAttribute))]
public class ConditionalHeaderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHeaderAttribute condHeader = (ConditionalHeaderAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(condHeader.ConditionFieldName);

        if (conditionProperty != null && conditionProperty.boolValue)
        {
            EditorGUI.LabelField(position, condHeader.Header, EditorStyles.boldLabel);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + condHeader.SpaceAfterHeader;
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHeaderAttribute condHeader = (ConditionalHeaderAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(condHeader.ConditionFieldName);

        float height = base.GetPropertyHeight(property, label);
        if (conditionProperty != null && conditionProperty.boolValue)
        {
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + condHeader.SpaceAfterHeader;
        }

        return height;
    }
}