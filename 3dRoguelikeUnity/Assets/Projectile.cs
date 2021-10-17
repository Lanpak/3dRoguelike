using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private bool collided = false;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.root.CompareTag("Shootable") && !collided)
        {
            collided = true;
            coll.transform.root.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

}
