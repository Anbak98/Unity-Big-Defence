// ReadOnlyAttribute.cs
using UnityEditor;
using UnityEngine;

public class ConditionalShowAttribute : PropertyAttribute
{
    public string ConditionFieldName { get; private set; }

    public ConditionalShowAttribute(string conditionFieldName)
    {
        ConditionFieldName = conditionFieldName;
    }
}

[CustomPropertyDrawer(typeof(ConditionalShowAttribute))]
public class ConditionalShowDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalShowAttribute condAttribute = (ConditionalShowAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(condAttribute.ConditionFieldName);

        if (conditionProperty != null && !conditionProperty.boolValue)
        {
            GUI.enabled = true;
            EditorGUI.PropertyField(position, property, label);
        }
        else
        {
            GUI.enabled = false; // GUI�� ��Ȱ��ȭ�Ͽ� �б� �������� ����ϴ�.
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true; // GUI�� �ٽ� Ȱ��ȭ�մϴ�.
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalShowAttribute condAttribute = (ConditionalShowAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(condAttribute.ConditionFieldName);

        if (conditionProperty != null && !conditionProperty.boolValue)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        else
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
