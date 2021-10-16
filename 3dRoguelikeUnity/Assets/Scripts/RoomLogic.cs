using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    private bool triggered = false;
    public GameObject[] doors = new GameObject[4];
    public GameObject room;


    private bool doorsOpen = true;

    private void OnTriggerEnter(Collider coll)
    {
        Debug.Log("collision");

        if(coll.gameObject.layer == 11 && !triggered && room.GetComponent<MapModelSelector>().type == 0)
        {
            triggered = true;
            Debug.Log("Player entered room!");
            MoveDoors(true);
            //ready enemies
        }
    }

    public void EnemyDied()
    {

    }

    private void MoveDoors(bool closing)
    {

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i] != null)
            {
                doors[i].SetActive(closing);
            }
        }

    }

}
