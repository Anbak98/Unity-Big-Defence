using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBuildableTile : MonoBehaviour
{
    [SerializeField] ValidEffectTilePool validEffectTilePool;
    [SerializeField] InvalidEffectTilePool invalidEffectTilePool;

    GameObject[] validEffectTile;
    GameObject[] invalidEffectTile;

    private int effectObjectNum = 25;

    /*

    public void SetTileXY(Vector2 tileXY)
    {
        this.tileXY = tileXY;
    }

    public void SetUnitSize(int unitSize)
    {
        this.unitSize = unitSize;
    }

    public void SetWidth(int width)
    {
        this.width = width;
    }

    public void SetHeight(int height)
    {
        this.height = height;
    }*/

    public void Awake()
    {
        this.validEffectTile = new GameObject[effectObjectNum];
        this.invalidEffectTile = new GameObject[effectObjectNum];
    }

    public void Start()
    {
        for (int i = 0; i < effectObjectNum; i++)
        {
            this.validEffectTile[i] = validEffectTilePool.GetPooledObject();
            this.validEffectTile[i].SetActive(true);

            this.invalidEffectTile[i] = invalidEffectTilePool.GetPooledObject();
            this.invalidEffectTile[i].SetActive(true);
        }

        foreach (GameObject obj in this.validEffectTile)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in this.invalidEffectTile)
        {
            obj.SetActive(false);
        }
    }

    public void UnsetEffectTile()
    {
        foreach (GameObject obj in validEffectTile)
        {
            this.validEffectTilePool.ReturnToPool(obj);
        }

        foreach (GameObject obj in invalidEffectTile)
        {
            this.invalidEffectTilePool.ReturnToPool(obj);
        }
    }

    public void DetectTileUnderBuilding(Vector2 position, MapController mapControll, int? buildingWidth = 1, int? buildingHeight = 1)
    {
        for (int i = 0; i < buildingWidth; i++)
        {
            for (int j = 0; j < buildingHeight; j++)
            {
                Vector2 tilePosition = position + new Vector2(i, j);

                if (mapControll.IsValidTileForBuild(tilePosition))
                {
                    this.invalidEffectTile[i * buildingWidth.Value + j].SetActive(false);
                    this.validEffectTile[i * buildingWidth.Value + j].SetActive(true);
                    this.validEffectTile[i * buildingWidth.Value + j].transform.position = tilePosition;
                }
                else
                {
                    this.validEffectTile[i * buildingWidth.Value + j].SetActive(false);
                    this.invalidEffectTile[i * buildingWidth.Value + j].SetActive(true);
                    this.invalidEffectTile[i * buildingWidth.Value + j].transform.position = tilePosition;
                }
            }
        }
    }
}
