using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffectScript : MonoBehaviour
{
    public float lifetime;

    void Start()
    {
        Invoke("Destruct", lifetime);
    }


       

    private void Destruct()
    {
        Destroy(gameObject);
    }

}
