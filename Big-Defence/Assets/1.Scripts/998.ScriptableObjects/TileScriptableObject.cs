using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TilePrefabs", menuName = "Custom/Tile Prefabs", order = 1)]
public class TileScriptableObject : ScriptableObject
{
    //[Header("value")]
    //[ReadOnly][SerializeField] private int pixelPerUnit = 64;
    //[ReadOnly][SerializeField] private int pixelPerGrid = 64;
    //[ReadOnly][SerializeField] private int unitPerGrid = 1;

    [field: Header("Tiles")]
    [field: SerializeField] public List<GameObject> PrefabList { get; private set; }

    public (int, int) CalculateWidthAndHeight(int tileCode)
    {
        SpriteRenderer spriteRenderer = PrefabList[tileCode].GetComponent<SpriteRenderer>();

        int ppu = (int)spriteRenderer.sprite.pixelsPerUnit;
        int w = spriteRenderer.sprite.texture.width / ppu;
        int h = spriteRenderer.sprite.texture.height / ppu;

        return (w, h);
    }
}
