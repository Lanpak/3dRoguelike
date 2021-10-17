using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private bool collided = false;
    public float lifetime = 20f;


    void Start()
    {
        Invoke("DespawnObj", lifetime);
    }

    private void DespawnObj()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.root.CompareTag("Shootable") && !collided)
        {
            collided = true;
            coll.transform.root.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

}
