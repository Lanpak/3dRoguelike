using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    private bool triggered = false;
    public GameObject[] doors = new GameObject[4];


    private bool doorsOpen = true;

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.transform.name == "Player" && !triggered)
        {
            triggered = true;
            Debug.Log("Player entered room!");
            MoveDoors(true);
            //ready enemies
        }
    }


    private void MoveDoors(bool closing)
    {
        
        foreach (GameObject door in doors)
        {
            //door.GetComponent<Animator>().Play("MoveDoors");
            door.SetActive(closing);

            //Invoke("PauseAnim", 0.5f);
        }
    }

}
