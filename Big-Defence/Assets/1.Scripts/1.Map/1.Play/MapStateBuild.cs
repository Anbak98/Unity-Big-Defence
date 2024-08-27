using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStateBuild : MonoBehaviour, IMapState
{
    [SerializeField] private GameObject BuildMenuUI;
    [SerializeField] private MapController mapController;

    public void EnterState()
    {
        BuildMenuUI.SetActive(true);
    }

    public void ExitState()
    {
        BuildMenuUI.SetActive(false);
    }

    public void InputAction()
    {

    }

    public void SelectBuildingButton(int buildingCode)
    {
        mapController.StartBuild(buildingCode);
    }

}
