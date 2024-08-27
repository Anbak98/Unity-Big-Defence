using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapEditor))]
public class PagedButtonUIEditor : Editor
{
    private MapEditor script;
    public Sprite buttonSprite; // �����Ϳ��� ������ �� �ִ� �̹��� �ʵ�
                                // ������ ��ư�� ���� ���̾ƿ� ����
    int buttonsPerRow = 5;
    int rows = 6;
    float buttonWidth = 60;
    float buttonHeight = 60; // ��ư�� ���̸� �̹��� ũ�⿡ �°� ����
    float spacing = 5;
    int currentPage = 0;
    int maxPage = 3;
    int totalButtons = 0;

    private void OnEnable()
    {
        script = (MapEditor)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Selected Tile", EditorStyles.boldLabel);

        buttonSprite = script.GetTileSprite(script.tileCode);

        if (buttonSprite != null)
        {
            // Sprite�� Texture2D�� ��ȯ�Ͽ� ��ư �̹����� ���
            Texture2D buttonTexture = SpriteToTexture2D(buttonSprite);
            GUIContent buttonContent = new GUIContent(buttonTexture);

            if (GUILayout.Button(buttonContent, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
            }
        }
        else
        {
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tile select menu", EditorStyles.boldLabel);

        totalButtons = rows * buttonsPerRow;

        GUILayout.BeginVertical();
        for (int row = 0; row < rows; row++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spacing);
            for (int col = 0; col < buttonsPerRow; col++)
            {
                int buttonIndex = row * buttonsPerRow + col + currentPage * totalButtons;
                if (buttonIndex < totalButtons + currentPage * totalButtons)
                {
                    buttonSprite = script.GetTileSprite(buttonIndex);

                    if (buttonSprite != null)
                    {
                        // Sprite�� Texture2D�� ��ȯ�Ͽ� ��ư �̹����� ���
                        Texture2D buttonTexture = SpriteToTexture2D(buttonSprite);
                        GUIContent buttonContent = new GUIContent(buttonTexture);

                        if (GUILayout.Button(buttonContent, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
                        {
                            script.tileCode = buttonIndex;
                            Debug.Log($"Button {buttonIndex} clicked.");
                            // ��ư Ŭ�� �� ������ ���⿡ �߰�
                        }
                    }
                    else
                    {
                        if (GUILayout.Button((buttonIndex + 1).ToString(), GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
                        {
                            Debug.Log($"Button {buttonIndex + 1} clicked.");
                            // ��ư Ŭ�� �� ������ ���⿡ �߰�
                        }
                    }
                }
                else
                {
                    GUILayout.Button("", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight));
                }

                GUILayout.Space(spacing);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(spacing);
        }
        GUILayout.EndVertical();

        // ������ ��ȯ ��ư �߰�
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Previous Page"))
        {
            if (currentPage > 0)
            {
                currentPage--;
            }
            else
            {
                currentPage = maxPage;
            }
        }

        GUILayout.Button(currentPage.ToString());

        if (GUILayout.Button("Next Page"))
        {
            if (currentPage < maxPage)
            {
                currentPage += 1;
            }
            else
            {
                currentPage = 0;
            }
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (GUILayout.Button("Initialize map"))
        {
            script.MapInit();
        }

        if (GUILayout.Button("Load map"))
        {
            script.MapLoad();
        }

        if (GUILayout.Button("Save map"))
        {
            script.MapSave();
        }
    }

    private Texture2D SpriteToTexture2D(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}
