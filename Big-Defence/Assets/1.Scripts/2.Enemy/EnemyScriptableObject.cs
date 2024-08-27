using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPrefabs", menuName = "Custom/Enemy Prefab", order = 2)]
public class EnemyScriptableObject : ScriptableObject
{
    [field: SerializeField] public List<EnemyStatus> PrefabList {  get; private set; }
}
