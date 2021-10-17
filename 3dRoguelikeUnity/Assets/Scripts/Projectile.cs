using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private bool collided = false;
    public float lifetime = 7f;


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
        Debug.Log(coll.collider.gameObject.transform.root);
        
        if (coll.collider.gameObject.transform.root.CompareTag("Shootable") && !collided)
        {
            Debug.Log("should be doing damage");
            collided = true;
            coll.transform.root.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

}
