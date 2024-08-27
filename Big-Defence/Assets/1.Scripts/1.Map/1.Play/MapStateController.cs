using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MAP_STATE
{
    Init, Load, Save, Default, Build
}

public class MapStateController : MonoBehaviour
{
    //public static MapManager Instance;

    //[field: Header("Map")]
    //[field: SerializeField] public MapSaveData Map {  get; private set; }

    [Header("States")]
    [SerializeField] private MapStateLoad loadState;
    [SerializeField] private MapStateSave saveState;
    [SerializeField] private MapStateDefault defaultState;
    [SerializeField] private MapStateBuild buildState;

    private Dictionary<MAP_STATE, IMapState> mapStates;

    private IMapState nowState = null;

    void Awake()
    {
        mapStates = new Dictionary<MAP_STATE, IMapState>
        {
            { MAP_STATE.Load, loadState },
            { MAP_STATE.Save, saveState },
            { MAP_STATE.Default, defaultState },
            { MAP_STATE.Build, buildState }
        };

        //if (Instance != null || Instance == this)
        //{
        //    return;
        //}
        //Instance = this;
    }

    private void Start()
    {
        ChangeMapState(MAP_STATE.Load);
        ChangeMapState(MAP_STATE.Default);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (nowState == mapStates[MAP_STATE.Default]) ChangeMapState(MAP_STATE.Build);
            else if (nowState == mapStates[MAP_STATE.Build]) ChangeMapState(MAP_STATE.Default);
        }

        nowState.InputAction();        
    }

    public void ChangeMapState(MAP_STATE mapState)
    {
        nowState?.ExitState();
        nowState = mapStates[mapState];
        nowState.EnterState();
    }
}

