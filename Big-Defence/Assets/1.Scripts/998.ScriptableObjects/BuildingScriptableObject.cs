using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingPrefabs", menuName = "Custom/Building Prefabs", order = 1)]
public class BuildingScriptableObject : ScriptableObject
{
    //[Header("value")]
    //[ReadOnly][SerializeField] private int pixelPerUnit = 64;
    //[ReadOnly][SerializeField] private int pixelPerGrid = 64;
    //[ReadOnly][SerializeField] private int unitPerGrid = 1;

    [field: Header("Buildings")]
    [field: SerializeField] public List<GameObject> PrefabList { get; private set; }

    public (int, int) CalculateWidthAndHeight(int buildingCode)
    {
        SpriteRenderer spriteRenderer = PrefabList[buildingCode].GetComponent<SpriteRenderer>();

        int ppu = (int)spriteRenderer.sprite.pixelsPerUnit;
        int w = spriteRenderer.sprite.texture.width / ppu;
        int h = spriteRenderer.sprite.texture.height / ppu;

        return (w, h);
    }
}
