using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStateInit : MonoBehaviour, IMapState
{
    [SerializeField] private MapController mapController;

    public void EnterState()
    {
        mapController.Initialize();
    }

    public void ExitState()
    {
    }

    public void InputAction()
    {
    }
}
