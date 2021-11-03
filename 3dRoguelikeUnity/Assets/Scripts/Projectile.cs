using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Projectile : MonoBehaviour
{
    public int damage;
    public bool collided = false;
    public float lifetime = 7f;
    public string enemyName;


    public bool friendly;

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
        if (coll.transform.CompareTag("Shootable"))
        {
            if (!collided)
            {
                Debug.Log("should be doing damage");
                collided = true;

                
                //coll.transform.GetComponent<EnemySegment>().RelayDamage(damage);
            }
        }
        else if(coll.transform.root.CompareTag("Player"))
        {
            //take damage
        }
    }

}
