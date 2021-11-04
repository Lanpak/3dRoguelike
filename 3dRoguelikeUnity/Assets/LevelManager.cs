using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{

    // ----- floor 1 -----

    // ----- level -----
    // ----- level -----
    // ----- level -----


    // ----- floor 2 -----

    // ----- level -----
    // ----- level -----
    // ----- level -----

    // ----- floor 3 -----

    // ----- level -----
    // ----- level -----
    // ----- level -----



    public int levelsPerFloor;

    public int floorsPerGame;

    public int rewardsPerFloor;

    public int enemyScaling;

    public LevelGeneration generator;


    private int floor = 0;

    private int level = 0;


    public static LevelManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("should find levelgen");
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

        }

        //the rest of your code

        DontDestroyOnLoad(this.gameObject);

    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
        CreateLevel();
    }

    void Start()
    {
        

    }

    private void CreateLevel()
    {
        generator = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();

        generator.MakeMap();

    }

    public void MoveToFloor(int floor)
    {
        SceneManager.LoadScene(floor);
    }
}
