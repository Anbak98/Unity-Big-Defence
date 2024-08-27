using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;


public class BuildingGenerator : MonoBehaviour
{
    [SerializeField] private BuildingScriptableObject buildingData;

    public GameObject InstantiateBuildingObject(int buildingCode, Vector3? position = null)
    {
        if (!buildingData.PrefabList[buildingCode])
        {
            Debug.LogError("Wrong building code");
            return null;
        }

        if (position.HasValue)
        {
            return Instantiate(buildingData.PrefabList[buildingCode], position.Value, Quaternion.identity);
        }

        return Instantiate(buildingData.PrefabList[buildingCode]);
    }
    public GameObject[,] GenerateMapBuildingGroup(int[,] buildingCode)
    { 

        int w = buildingCode.GetLength(0);
        int h = buildingCode.GetLength(1);

        GameObject buildingParent = new("BuildingGroup");
        buildingParent.transform.parent = transform;

        GameObject[,] buildingObjectGroup = new GameObject[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                int code = buildingCode[x, y];
                if (code == 0) { continue; }

                Vector3 position = new(x, y, 0);
                GameObject buildingObject = Instantiate(buildingData.PrefabList[code], position, Quaternion.identity);
                buildingObject.name = $"Tile({buildingCode})({x},{y})";
                buildingObject.transform.parent = transform;
            }
        }

        return buildingObjectGroup;
    }

    /*public void OnStart(Vector3 xy, int buildingCode, int width, int height)
    {
        _buildingCode = buildingCode;
        _width = width;
        _height = height;
    
        TurnOffDetect();

        SetDetectBuildTile(_width, _height);

        TurnOnDetect();

        BuildSettingOnMouseStart(_buildingCode, xy, _width, _height);
    }

    public void OnUpdate(Vector3 xy)
    {
        if (detect)
        {
            xy = xy - new Vector3(_width / 2, _height / 2);

            BuildSettingOnMouseUpdate(xy, _width, _height);
            DoDetect(xy);

            if (Input.GetMouseButton(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    BuildOnMouseLeftClick(_buildingCode, xy, _width, _height);
                }
            }

            if (Input.GetMouseButton(1))
            {
                TurnOffDetect();
            }
        }
    }

    public void BuildSettingOnMouseStart(int buildingCode, Vector3 xy, int width, int height)
    {
        Vector3 _xy = xy + new Vector3(width / 2, height / 2);
        if (width > 1) _xy -= new Vector3(0.5F, 0, 0);
        if (height > 1) _xy -= new Vector3(0, 0.5F, 0);
        this.building = Instantiate(this.buildingData.buildings[buildingCode], _xy, Quaternion.identity);
        this.building.transform.parent = transform;
        this.building.SetActive(false);
    }

    public void BuildSettingOnMouseUpdate(Vector3 pos, int width, int height)
    {
        Vector3 _xy = pos + new Vector3(width / 2, height / 2);
        if (width > 1) _xy -= new Vector3(0.5F, 0, 0);
        if (height > 1) _xy -= new Vector3(0, 0.5F, 0);
        this.building.SetActive(true);
        this.building.transform.position = _xy;
    }

    public void BuildOnMouseLeftClick(int buildingCode, Vector3 pos, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!this._map.IsValidTileForBuild(pos + new Vector3(i, j)))
                {
                    return;
                }
            }
        }

        Vector3 _xy = pos + new Vector3(width / 2, height / 2);

        if (width > 1) _xy -= new Vector3(0.5F, 0, 0);
        if (height > 1) _xy -= new Vector3(0, 0.5F, 0);

        this.buildings.Add(this.building);
        this.building = Instantiate(this.buildingData.buildings[buildingCode], _xy, Quaternion.identity);
        building.transform.parent = transform;
        this._map.BuildOnTile(this.building, pos, width, height);
    }*/

    /*public void DoDetect(Vector3 xy)
    {
        this.detectBuildTile.DetectValidTile(xy);
    }

    public void SetDetectBuildTile(int width, int height)
    {
        this.detectBuildTile.SetMap(_map);
        this.detectBuildTile.SetWidth(width);
        this.detectBuildTile.SetHeight(height);
        this.detectBuildTile.SetUnitSize(unitSize);
    }

    public void TurnOnDetect()
    {
        this.detectBuildTile.SetEffectTile();
        this.detect = true;
    }

    public void TurnOffDetect()
    {
        if (building == null) return;
        this.detectBuildTile.UnsetEffectTile();
        Destroy(this.building);
        this.detect = false;
    }

    public bool GetDetect()
    {
        return this.detect;
    }

    public void SetMap(MapScriptableObject map)
    {
        this._map = map;
    }

    public void  SetDetect(bool detect)
    {
        this.detect = detect;
    }

    public void BuildSettingButton(int buildingCode, int width, int height)
    {
        OnStart(_map.GetTileXYOnMouse(), buildingCode, width, height);
    }
*/
}
