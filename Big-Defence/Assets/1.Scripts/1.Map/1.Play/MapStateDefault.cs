using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStateDefault : MonoBehaviour,IMapState
{
    [SerializeField] private MapController mapController;

    public void EnterState()
    {
    }

    public void ExitState()
    {
    }

    public void InputAction()
    {
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            mapController.SwitchGridMap();
        }
    }
}
