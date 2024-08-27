using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MapEditor : MonoBehaviour
{
    [SerializeField] public MapController mapController;

    [ReadOnly][SerializeField] public GameObject tileObj;
    [ReadOnly][SerializeField] public int tileCode;

    GameObject tileGroupParent;
    readonly string tileGroupName = "TileGroup";

    private Vector3 lastMousePosition;

    private void OnEnable()
    {
        mapController.Initialize();

        foreach (GameObject editorObj in GameObject.FindGameObjectsWithTag("EditorOnly"))
        {
            DestroyImmediate(editorObj);
        }

        tileGroupParent = new GameObject
        {
            name = tileGroupName,
            tag = "EditorOnly"
        };

        tileGroupParent.transform.parent = transform;

        mapController.Load();
        mapController.GenerateMap(true);
    }

    // 에디터에서 변경 시 자동으로 업데이트
    void OnValidate()
    {
        mapController.Initialize();
    }

    void OnGUI()
    {
        Event currentEvent = Event.current;

        // 마우스 우클릭을 누른 상태에서 드래그하면 카메라를 이동
        if (currentEvent.button == 1 && currentEvent.isMouse)
        {
            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    lastMousePosition = currentEvent.mousePosition;
                    break;

                case EventType.MouseDrag:
                    Vector3 delta = (Vector3)currentEvent.mousePosition - lastMousePosition;
                    Camera.main.transform.position -= new Vector3(delta.x, -delta.y) * 0.01f;
                    lastMousePosition = currentEvent.mousePosition;
                    SceneView.RepaintAll();  // 씬 뷰 업데이트
                    break;
            }
        }

        Vector3 mousePos = currentEvent.mousePosition;
        mousePos.y = Screen.height - mousePos.y;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 mousePosGame = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 mousePosTile = new()
        {
            x = (int)Math.Round(mousePosGame.x),
            y = (int)Math.Round(mousePosGame.y)
        };

        if (currentEvent.button == 0 && currentEvent.isMouse) // 좌클릭
        {
            switch (currentEvent.type)
            {
                case EventType.MouseDown:

                    if (mapController.IsTileInGrid(mousePosTile))
                    {
                        InstantiateTileObj(tileCode);
                        mapController.TileFollowPosition(tileObj, mousePosTile);
                        mapController.TileOnPosition(tileObj, tileCode, mousePosTile);
                    }
                    break;

                case EventType.MouseDrag:
                    break;
            }
            
        }

        if (GUILayout.Button("Initialize map"))
        {
            mapController.Initialize();

            foreach (GameObject editorObj in GameObject.FindGameObjectsWithTag("EditorOnly"))
            {
                DestroyImmediate(editorObj);
            }

            tileGroupParent = new GameObject
            {
                name = tileGroupName,
                tag = "EditorOnly"
            };

            tileGroupParent.transform.parent = transform;

            mapController.Load();
            mapController.GenerateMap(true);
        }

        if (GUILayout.Button("Load map"))
        {
            foreach (GameObject editorObj in GameObject.FindGameObjectsWithTag("EditorOnly"))
            {
                DestroyImmediate(editorObj);
            }

            tileGroupParent = new GameObject
            {
                name = tileGroupName,
                tag = "EditorOnly"
            };

            tileGroupParent.transform.parent = transform;

            mapController.Load();
            mapController.GenerateMap(true);
        }

        if (GUILayout.Button("Save map"))
        {
            mapController.Save();
        }
    }

    public void InstantiateTileObj(int tileIndex)
    {
        tileObj = Instantiate(mapController.GetTileObject(tileIndex));
        tileObj.transform.position = transform.position;
        tileObj.transform.parent = tileGroupParent.transform;
    }

    public Sprite GetTileSprite(int index)
    {
        return mapController.GetTileSprite(index);
    }
}
