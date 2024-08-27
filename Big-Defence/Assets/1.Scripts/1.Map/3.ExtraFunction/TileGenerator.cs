using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private TileScriptableObject tileData;

    public GameObject InstantiateTile(int tileCode, Vector2? position)
    {
        if (!tileData.PrefabList[tileCode])
        {
            Debug.LogError("Wrong tile code");
            return null;
        }

        if (position.HasValue)
        {
            return Instantiate(tileData.PrefabList[tileCode], position.Value, Quaternion.identity);
        }

        return Instantiate(tileData.PrefabList[tileCode]);
    }

    public GameObject[,] GenerateMapGridGroup(int[,] gridCode, bool? editorMode = false)
    {
        int w = gridCode.GetLength(0);
        int h = gridCode.GetLength(1);
        
        GameObject gridTileParent = new("GridGroup");
        gridTileParent.transform.parent = transform;

        if (editorMode.Value)
        {
            gridTileParent.tag = "EditorOnly";
        }

        GameObject[,] gridTileObjectGroup = new GameObject[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector3 position = new(x , y , 0);
                GameObject gridTile = Instantiate(tileData.PrefabList[1], position, Quaternion.identity);
                gridTile.name = $"gridTile({x},{y})";
                gridTile.transform.parent = gridTileParent.transform;

                gridTileObjectGroup[x, y] = gridTile;
            }
        }

        return gridTileObjectGroup;
    }

    public GameObject[,] GenerateMapTileGroup(int[,] tileCode, bool? editorMode = false)
    {
        int w = tileCode.GetLength(0);
        int h = tileCode.GetLength(1);

        GameObject TileParent = new("TileGroup");
        TileParent.transform.parent = transform;

        if (editorMode.Value)
        {
            TileParent.tag = "EditorOnly";
        }

        GameObject[,] tileObjectGroup = new GameObject[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                int code = tileCode[x, y];
                if(code == 0) continue;

                Vector3 position = new(x , y, 0);
                GameObject tileObject = Instantiate(tileData.PrefabList[code], position, Quaternion.identity);
                tileObject.name = $"Tile({tileCode})({x},{y})";
                tileObject.transform.parent = TileParent.transform;

                tileObjectGroup[x, y] = tileObject;
            }
        }

        return tileObjectGroup;
    }

    public Sprite GetSprite(int tileCode)
    {
        if(tileData.PrefabList.Count <= tileCode)
        {
            return tileData.PrefabList[0].GetComponent<SpriteRenderer>().sprite;
        }
        if (!tileData.PrefabList[tileCode])
        {
            return tileData.PrefabList[0].GetComponent<SpriteRenderer>().sprite;
        }
        return tileData.PrefabList[tileCode].GetComponent<SpriteRenderer>().sprite;
    }

    public GameObject GetObj(int tileCode)
    {
        if (tileData.PrefabList.Count <= tileCode)
        {
            return tileData.PrefabList[0];
        }
        if (!tileData.PrefabList[tileCode])
        {
            return tileData.PrefabList[0];
        }
        return tileData.PrefabList[tileCode];
    }
}
