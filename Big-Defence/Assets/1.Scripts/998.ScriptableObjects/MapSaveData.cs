using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MapSaveData", menuName = "Custom/SaveData/Map", order = 1)]
public class MapSaveData : ScriptableObject
{
    [ReadOnly][SerializeField]
    private bool isInitialized = false;

    [field: ConditionalShow("isInitialized")][field: SerializeField] public int TopLength { get; private set; }
    [field: ConditionalShow("isInitialized")][field: SerializeField] public int BottomLength { get; private set; }
    [field: ConditionalShow("isInitialized")][field: SerializeField] public int LeftLength { get; private set; }
    [field: ConditionalShow("isInitialized")][field: SerializeField] public int RightLength { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    [SerializeField] private int[] gridCodeSaveData;
    [SerializeField] private int[] tileCodeSaveData;
    [SerializeField] private int[] buildingCodeSaveData;

    public int[,] TileCode { get; set; }
    public int[,] GridCode { get; set; }
    public int[,] BuildingCode { get; set; }

    public void InitializeByMapController()
    {
        Width = LeftLength + RightLength + 1;
        Height = TopLength + BottomLength + 1;

        if(!isInitialized)
        {
            tileCodeSaveData = new int[Width * Height];
            gridCodeSaveData = new int[Width * Height];
            buildingCodeSaveData = new int[Width * Height];
            isInitialized = true;
        }

        if (AnyCodeIsNull())
        {
            TileCode = new int[Width, Height];
            GridCode = new int[Width, Height];
            BuildingCode = new int[Width, Height];
        }

        if(TileCode.Length != tileCodeSaveData.Length)
        {
            Debug.LogError($"This map size not correct {TileCode.Length} , {tileCodeSaveData.Length}");
        }
    }

    public void Load()
    {
        if (AnySaveIsNull()) return;
        if (AnyCodeIsNull()) return;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                TileCode[x, y] = tileCodeSaveData[x * Height + y];
                GridCode[x, y] = gridCodeSaveData[x * Height + y];
                BuildingCode[x, y] = buildingCodeSaveData[x * Height + y];
            }
        }
    }

    public void Save()
    {
        if(AnyCodeIsNull()) return;
        if(AnySaveIsNull()) return;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                tileCodeSaveData[x * Height + y] = TileCode[x, y];
                gridCodeSaveData[x * Height + y] = GridCode[x, y];
                buildingCodeSaveData[x * Height + y] = BuildingCode[x, y];
            }
        }
    }

    public void ResetByMapController()
    {
        tileCodeSaveData = null;
        gridCodeSaveData = null;
        buildingCodeSaveData = null;

        TileCode = null;
        GridCode = null;
        BuildingCode = null;
    }

    private bool AnyCodeIsNull() => TileCode is null || GridCode is null || BuildingCode is null;

    private bool AnySaveIsNull() => tileCodeSaveData is null || gridCodeSaveData is null || buildingCodeSaveData is null;

}
