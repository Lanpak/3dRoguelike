using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySegment : MonoBehaviour
{
    public GameObject master;

    public void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Projectile") && !coll.gameObject.GetComponent<Projectile>().collided)
        {
            int damage = coll.gameObject.GetComponent<Projectile>().damage;
            

            master.GetComponent<Enemy>().TakeDamage(damage);
        }
        
    }
}
