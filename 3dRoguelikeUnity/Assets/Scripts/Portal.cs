using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Portal : MonoBehaviour
{
    public LevelManager manager;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("portal activated");
        if (other.gameObject.CompareTag("Player"))
        {
            manager.GoToNextLevel();


        }
    }

}
