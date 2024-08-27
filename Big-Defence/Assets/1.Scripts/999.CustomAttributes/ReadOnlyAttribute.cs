// ReadOnlyAttribute.cs
using UnityEditor;
using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute
{
    // 빈 클래스. 단지 PropertyAttribute를 상속받아 새로운 속성을 정의합니다.
}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // GUI를 비활성화하여 읽기 전용으로 만듭니다.
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true; // GUI를 다시 활성화합니다.
    }
}
