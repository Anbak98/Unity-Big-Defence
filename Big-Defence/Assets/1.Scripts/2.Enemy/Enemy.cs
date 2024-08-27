using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int HP { get; private set;  }
    [field: SerializeField] public int Damage { get; private set; }

    public bool DecreaseHP(int damage)
    {
        HP -= damage;

        if (HP < 0) 
        { 
            HP = 0;
            return false;
        }

        return true;
    }

    public void Attack()
    { 
        
    }
}
