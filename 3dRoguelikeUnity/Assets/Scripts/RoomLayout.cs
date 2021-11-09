using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayout : MonoBehaviour
{
    public GameObject enemy;
    public GameObject container;

    private List<Transform> spawns = new List<Transform>();


    void Awake()
    {


        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            spawns.Add(child);
        }
        container = transform.root.GetChild(0).gameObject;
    }

    public void SpawnEnemies(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Invoke("DoSpawn", 0.8f);
        }
    }



    private void DoSpawn()
    {

        Transform foundSpawn = spawns[Random.Range(0, spawns.Count)];
        if (spawns.Contains(foundSpawn))
        {
            spawns.Remove(foundSpawn);
            GameObject spawnedObj = Instantiate(enemy, new Vector3(foundSpawn.position.x, foundSpawn.position.y, foundSpawn.position.z), Quaternion.identity, container.transform);
            spawnedObj.transform.eulerAngles = new Vector3(spawnedObj.transform.eulerAngles.x - 90, spawnedObj.transform.eulerAngles.y, spawnedObj.transform.eulerAngles.z);
        }
        else
        {
            DoSpawn();
        }
    }
}
