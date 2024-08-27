using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStateLoad : MonoBehaviour, IMapState
{
    [SerializeField] private MapController mapController;

    public void EnterState()
    {
        mapController.Load();
        mapController.GenerateMap();
    }

    public void ExitState()
    {
    }

    public void InputAction()
    {
    }
}
