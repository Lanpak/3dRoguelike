using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    private bool triggered = false;
    public GameObject[] doors = new GameObject[4];
    public GameObject room;
    public GameObject enemies;


    private bool doorsOpen = true;

    private void OnTriggerEnter(Collider coll)
    {
        Debug.Log("collision");

        if(coll.gameObject.layer == 11 && !triggered && room.GetComponent<MapModelSelector>().type == 0)
        {
            triggered = true;
            Debug.Log("Player entered room!");
            MoveDoors(true);
            ReadyEnemies();
        }
    }

    public void ReadyEnemies()
    {
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            enemies.transform.GetChild(i).GetComponent<Enemy>().isDisabled = false;
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
