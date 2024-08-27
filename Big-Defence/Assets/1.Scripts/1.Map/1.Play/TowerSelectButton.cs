using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectButton : MonoBehaviour
{
    [SerializeField] Button LargeTower;
    [SerializeField] Button MediumTower;
    [SerializeField] Button SmallTower;

    [SerializeField] MapController MapController;

    // Start is called before the first frame update
    void Start()
    {
        LargeTower.onClick.AddListener(() => MapController.StartBuild(1));
        MediumTower.onClick.AddListener(() => MapController.StartBuild(2));
        SmallTower.onClick.AddListener(() => MapController.StartBuild(3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
