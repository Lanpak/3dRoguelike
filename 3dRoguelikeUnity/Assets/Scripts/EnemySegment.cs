using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySegment : MonoBehaviour
{
    public GameObject master;

    public void RelayDamage(int damage)
    {
        master.GetComponent<Enemy>().TakeDamage(damage);
    }
}
