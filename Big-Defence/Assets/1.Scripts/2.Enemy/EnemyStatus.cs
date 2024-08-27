using System;
using UnityEngine;

[Serializable]
public class EnemyStatus
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int HP {  get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
}
