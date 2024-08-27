// ReadOnlyAttribute.cs
using UnityEditor;
using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute
{
    // �� Ŭ����. ���� PropertyAttribute�� ��ӹ޾� ���ο� �Ӽ��� �����մϴ�.
}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // GUI�� ��Ȱ��ȭ�Ͽ� �б� �������� ����ϴ�.
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true; // GUI�� �ٽ� Ȱ��ȭ�մϴ�.
    }
}
