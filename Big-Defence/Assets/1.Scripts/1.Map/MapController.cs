using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

[Serializable]
public class MapController : MonoBehaviour
{   
    [field: SerializeField] public MapSaveData Map { get; private set; }

    [SerializeField] private BuildingGenerator buildingGenerator;
    [SerializeField] private TileGenerator tileGenerator;
    [SerializeField] private DetectBuildableTile detectBuildableTile;

    private CursorPosition cursorPosition;

    private GameObject[,] tileObjectGroup;
    private GameObject[,] gridObjectGroup;
    private GameObject[,] buildingObjectGroup;

    private void Awake()
    {
        if (!this.Map)
        {
            Debug.LogError("Do not assignd Map to MapController");
        }
        this.Initialize();
    }

    #region Map
    public void Initialize()
    {
        Map.InitializeByMapController();

        cursorPosition = new();

        tileObjectGroup = new GameObject[Map.Width, Map.Height];
        gridObjectGroup = new GameObject[Map.Width, Map.Height];
        buildingObjectGroup = new GameObject[Map.Width, Map.Height];
    }

    public void GenerateMap(bool? editorMode = false)
    {
        if (gridObjectGroup is not null)
        {
            foreach (GameObject gridTile in gridObjectGroup)
            {
                DestroyImmediate(gridTile);
            }
            gridObjectGroup = null;
        }

        if (tileObjectGroup is not null)
        {
            foreach (GameObject gridTile in tileObjectGroup)
            {
                DestroyImmediate(gridTile);
            }
            tileObjectGroup = null;
        }

        if (buildingObjectGroup is not null)
        {
            foreach (GameObject gridTile in buildingObjectGroup)
            {
                DestroyImmediate(gridTile);
            }
            buildingObjectGroup = null;
        }
        this.gridObjectGroup = tileGenerator.GenerateMapGridGroup(Map.GridCode, editorMode);
        this.tileObjectGroup = tileGenerator.GenerateMapTileGroup(Map.TileCode, editorMode);
        if(!editorMode.Value) this.buildingObjectGroup = buildingGenerator.GenerateMapBuildingGroup(Map.BuildingCode);
    }

    public void Save() => Map.Save();

    public void Load() => Map.Load(); 

    public void ResetMap()
    {
        Map.ResetByMapController();

        if (gridObjectGroup is not null)
        {
            foreach (GameObject gridTile in gridObjectGroup)
            {
                DestroyImmediate(gridTile);
            }
            gridObjectGroup = null;
        }

        if (tileObjectGroup is not null)
        {
            foreach (GameObject gridTile in tileObjectGroup)
            {
                DestroyImmediate(gridTile);
            }
            tileObjectGroup = null;
        }

        if (buildingObjectGroup is not null)
        {
            foreach (GameObject gridTile in buildingObjectGroup)
            {
                DestroyImmediate(gridTile);
            }
            buildingObjectGroup = null;
        }
    }

    public void SwitchGridMap()
    {
        if (gridObjectGroup is null)
        {
            Debug.Log("Can't switch activation about null grid Map");
            return;
        }

        foreach (GameObject gridTile in gridObjectGroup)
        {
            gridTile.SetActive(!gridTile.activeSelf);
        }
    }

    public void TurnOnGridMap()
    {
        if (gridObjectGroup is null)
        {
            Debug.Log("Can't turn on activation about null grid Map");
            return;
        }

        foreach (GameObject gridTile in gridObjectGroup)
        {
            gridTile.SetActive(true);
        }
    }

    public void TurnOffGridMap()
    {
        if (gridObjectGroup is null)
        {
            Debug.Log("Can't turn off activation about null grid Map");
            return;
        }

        foreach (GameObject gridTile in gridObjectGroup)
        {
            gridTile.SetActive(false);
        }
    }
    #endregion

    #region Building

    GameObject waitingBuilding;
    Coroutine buildRoutine;

    public void StartBuild(int buildingCode)
    {
        if (waitingBuilding)
        {
            Destroy(waitingBuilding);
        }

        if(buildRoutine != null)
        {
            StopCoroutine(buildRoutine);
        }

        detectBuildableTile.UnsetEffectTile();

        waitingBuilding = buildingGenerator.InstantiateBuildingObject(buildingCode, cursorPosition.tilePosOnMouse);

        buildRoutine = StartCoroutine(BuildRoutine(waitingBuilding, buildingCode));
    }

    public IEnumerator BuildRoutine(GameObject building, int buildingCode)
    {
        SpriteRenderer spriteRenderer = building.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "BuildingSearchingBuildableTile";
        int ppu = (int)spriteRenderer.sprite.pixelsPerUnit;
        int w = spriteRenderer.sprite.texture.width / ppu;
        int h = spriteRenderer.sprite.texture.height / ppu;

        while (true)
        {
            Vector2 position = cursorPosition.tilePosOnMouse;

            BuildingFollowMouse(building, position, w, h);

            // 좌클릭 감지
            if (Input.GetMouseButtonDown(0))
            {
                if (BuildOnPosition(building, buildingCode, position, w, h))
                {
                    spriteRenderer.sortingLayerName = "Building";
                    detectBuildableTile.UnsetEffectTile();
                    waitingBuilding = null;
                    break;
                }
            }
            // 우클릭 감지
            else if (Input.GetMouseButtonDown(1))
            {
                Destroy(waitingBuilding);
                detectBuildableTile.UnsetEffectTile();
                break;
            }

            // 기본 동작
            yield return null;
        }
    }

    public void BuildingFollowMouse(GameObject building, Vector2 cursorPosition, int buildingWidth, int buildingHeight)
    {
        Vector2 position = cursorPosition;
        int w = buildingWidth;
        int h = buildingHeight;

        Vector2 center = position + new Vector2(w / 2, h / 2);
        if (w > 1) center -= new Vector2(0.5F, 0);
        if (h > 1) center -= new Vector2(0, 0.5F);

        building.transform.position = center;

        detectBuildableTile.DetectTileUnderBuilding(position, this, w, h);
    }

    public bool BuildOnPosition(GameObject building, int buildingCode, Vector2 cursorPosition, int buildingWidth, int buildingHeight)
    {
        Vector2 position = cursorPosition;
        int w = buildingWidth;
        int h = buildingHeight;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (!IsValidTileForBuild(position + new Vector2(i, j)))
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Map.BuildingCode[(int)position.x + i, (int)position.y + j] = buildingCode;
                buildingObjectGroup[(int)position.x + i, (int)position.y + j] = building;
            }
        }

        return true;
    }

    public bool IsValidTileForBuild(Vector2 tileXY)
    {
        int x = (int)tileXY.x;
        int y = (int)tileXY.y;

        if (x + 1 > this.Map.Width || y + 1 > this.Map.Height)
        {
            return false;
        }

        if (x < 0 || y < 0)
        {
            return false;
        }

        if (Map.TileCode == null || Map.BuildingCode == null)
        {
            Debug.Log("Map.IsValidTileForBuild() : There is no variable 'tileCode'");
            return false;
        }

        if (Map.TileCode[x, y] != 0 || Map.BuildingCode[x,y] != 0)
        {
            return false;
        }

        //Debug.Log(x + " " + y + " :" + tileCode[x, y]);

        return true;
    }

    public bool IsTileInGrid(Vector2 tileXY)
    {
        int x = (int)tileXY.x;
        int y = (int)tileXY.y;

        if (x + 1 > this.Map.Width || y + 1 > this.Map.Height)
        {
            return false;
        }

        if (x < 0 || y < 0)
        {
            return false;
        }

        return true;
    }

    public void DestroyGrid()
    {
        if (gridObjectGroup != null)
        {
            foreach(GameObject gridTile in gridObjectGroup)
            {
                Destroy(gridTile);
            }
        }
    }

    #endregion

    #region MapEditor (Tile)
    public void TileFollowPosition(GameObject tileObj, Vector2 position)
    {
        tileObj.transform.position = position;
    }

    public bool TileOnPosition(GameObject tileObj, int tileCode, Vector2 position)
    {                
        if (Map.TileCode[(int)position.x, (int)position.y] != 0)
        {
            DestroyImmediate(tileObjectGroup[(int)position.x, (int)position.y]);
        }

        Map.TileCode[(int)position.x, (int)position.y] = tileCode;
        tileObjectGroup[(int)position.x, (int)position.y] = tileObj;
            
        return true;
    }

    public Sprite GetTileSprite(int tileCode)
    {
        return tileGenerator.GetSprite(tileCode);
    }

    public GameObject GetTileObject(int tileCode)
    {
        return tileGenerator.GetObj(tileCode);
    }

    public void DeleteMap()
    {

    }
    #endregion

    #region PathFinding
    public int[,] FindPath(Vector2Int start, Vector2Int goal)
    {
        int[,] grid = Map.TileCode;

        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int[,] gridNav = new int[rows, cols];

        // 오픈 리스트와 클로즈 리스트 생성
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        // 시작 노드 추가
        Node startNode = new Node(start, null, 0, GetHeuristic(start, goal));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // 오픈 리스트에서 F 값이 가장 작은 노드를 선택
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F < currentNode.F)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // 목표 지점에 도달하면 경로를 생성
            if (currentNode.Position == goal)
            {
                Node temp = currentNode;
                while (temp != null)
                {
                    gridNav[temp.Position.x, temp.Position.y] = 1; // 경로를 1로 표시
                    temp = temp.Parent;
                }
                break;
            }

            // 인접한 노드들을 확인
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentNode.Position + direction;

                // 그리드 범위 내인지 확인
                if (neighborPos.x >= 0 && neighborPos.x < rows && neighborPos.y >= 0 && neighborPos.y < cols)
                {
                    // 이동할 수 있는지 확인 (0은 이동 가능, 1은 이동 불가능)
                    if (grid[neighborPos.x, neighborPos.y] == 1 || closedList.Contains(new Node(neighborPos)))
                    {
                        continue;
                    }

                    int gCost = currentNode.G + 1; // G 값 증가
                    Node neighborNode = new Node(neighborPos, currentNode, gCost, GetHeuristic(neighborPos, goal));

                    if (!openList.Exists(node => node.Position == neighborPos && node.G <= gCost))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        return gridNav;
    }

    // 휴리스틱 계산 (맨해튼 거리)
    private int GetHeuristic(Vector2Int pos, Vector2Int goal)
    {
        return Mathf.Abs(pos.x - goal.x) + Mathf.Abs(pos.y - goal.y);
    }

    // 노드 클래스
    private class Node
    {
        public Vector2Int Position { get; }
        public Node Parent { get; }
        public int G { get; } // 시작점으로부터의 비용
        public int H { get; } // 목표점까지의 휴리스틱
        public int F { get { return G + H; } } // G + H

        public Node(Vector2Int position, Node parent = null, int g = 0, int h = 0)
        {
            Position = position;
            Parent = parent;
            G = g;
            H = h;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node)
            {
                Node other = (Node)obj;
                return Position.Equals(other.Position);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
    #endregion
}
