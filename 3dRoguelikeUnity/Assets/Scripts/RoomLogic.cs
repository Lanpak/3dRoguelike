using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    public bool triggered = false;
    public GameObject[] doors = new GameObject[4];
    public GameObject room;
    public GameObject enemies;
    public GameObject player;
    public Vector2 position = Vector2.zero;
    
    [SerializeField] private int howManyLive;

    private LevelManager manager;
    private MusicManager music;

    private MapModelSelector mapman;

    void Start()
    {
        mapman = gameObject.transform.root.GetComponent<MapModelSelector>();
        manager = GameObject.Find("Manager").GetComponent<LevelManager>();
        music = GameObject.Find("MusicManager").GetComponent<MusicManager>();

        player = GameObject.Find("Player(Clone)");
        position = room.GetComponent<MapModelSelector>().pos;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.layer == 11)
        {
            player.GetComponent<PlayerManager>().MoveIntoRoom((int)position.x, (int)position.y);

            if (!triggered && room.GetComponent<MapModelSelector>().type == 0)
            {
                triggered = true;
                Debug.Log("Player entered room!");

                if(mapman.type == 2 && manager.floor == 3)
                {
                    StartBossRoom();
                    Debug.Log("StartBossRoom");
                }

                StartRoom();
            }
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
        howManyLive--;

        if(howManyLive == 0)
        {
            FinishRoom();
        }
    }

    private void FinishRoom()
    {
        MoveDoors(false);
    }
    
    private void StartRoom()
    {
        MoveDoors(true);
        ReadyEnemies();
        howManyLive = enemies.transform.childCount;
    }
    
    private void StartBossRoom()
    {
        MoveDoors(true);
        ReadyEnemies();
        howManyLive = enemies.transform.childCount;


        music.StartTrack(4, 0);

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
