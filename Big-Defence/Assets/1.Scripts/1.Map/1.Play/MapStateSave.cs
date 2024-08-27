using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStateSave : MonoBehaviour, IMapState
{
    [SerializeField] private MapController mapController;

    public void EnterState()
    {
        mapController.Save();
    }

    public void ExitState()
    {
    }

    public void InputAction()
    {
    }
}
