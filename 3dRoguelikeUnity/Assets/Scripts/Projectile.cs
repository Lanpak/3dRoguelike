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




    public bool usingExplosive;
    private bool isShard;
    public int shardsOnExplosion;
    public float shardSize;
    public int shardForce;
    private bool exploded = false;
    


    private LevelManager manager;

    public bool friendly;

    void Start()
    {
        if (!isShard)
        {
            manager = GameObject.Find("Manager").GetComponent<LevelManager>();

            if (manager.playerUpgrades.Contains(2)) { usingExplosive = true; }


        }

        Invoke("DespawnObj", lifetime);
    }

    private void DespawnObj()
    {
        Destroy(gameObject);
    }


    
    private void Explode()
    {
        for (int i = 0; i < shardsOnExplosion; i++)
        {
            GameObject proj = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
            proj.transform.localScale = new Vector3(shardSize, shardSize, shardSize);
            proj.GetComponent<Projectile>().damage = damage;
            proj.GetComponent<Projectile>().isShard = true;
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.AddTorque(new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), ForceMode.Impulse);
            Transform rot = gameObject.transform;
            rot.rotation = Random.rotation;
            rb.AddForce(rot.forward * shardForce, ForceMode.Impulse);
        }

        
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
        else 
        {
            if (!isShard && usingExplosive && !exploded)
            {
                Explode();
                Destroy(gameObject);
            }
        }

        
    
    }

}
