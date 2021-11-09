using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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



    [Header("LevelGeneration")]


    public int startRooms; //how many rooms are on the first level

    public int levelsPerFloor;

    public int floorsPerGame;

    public int roomsPerFloor;

    public int enemyScaling;


    public LevelGeneration generator;

    [SerializeField] private int floor = 1;

    [SerializeField] private int level = 0;

    [Header("Upgrades")]

    public int upgradeChoices;
    public List<int> playerUpgrades = new List<int>();

    public Button[] upgradePrefab = new Button[10];
    public int[] allUpgrades = new int[10];
    //UpgradeCode chart

    // 0: splitter
    // 1: damage+
    // 2: explosive
    // 3: recoil-
    // 4: ammo+
    // 5: scale+
    // 6: scale-
    // 7: speed+
    // 8: armour
    // 9: hp+

    public GameObject options;
   
    


    public static LevelManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("should find levelgen");
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
        //Debug.Log("Level Loaded");
        //Debug.Log(scene.name);
        //Debug.Log(mode);

        if (SceneManager.GetActiveScene().buildIndex == 1) // what to do in game mode
        {
            Cursor.visible = false;
            Debug.Log("locked");
            CreateLevel();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2) // what to do in upgrade pick mode
        {
            Cursor.visible = true;

            options = GameObject.Find("Canvas/PortalUI/Options");

            SpawnUpgrades();
            
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0) // what to do in 
        {
            Cursor.visible = true;
        }


    }

    private void SpawnUpgrades()
    {

        List<int> possibleUpgrades = new List<int>();

        for (int i = 0; i < allUpgrades.Length; i++)
        {
            if (!playerUpgrades.Contains(i))
            {
                possibleUpgrades.Add(i);
            }
        }

        for (int i = 0; i < upgradeChoices; i++)
        {
            int rng = Random.Range(0, possibleUpgrades.Count - 1);

            int up = possibleUpgrades[rng];

            Instantiate(upgradePrefab[up], options.transform);

            possibleUpgrades.Remove(up);

            Debug.Log("removed: " + up);
        }









































        //List<int> displayedUpgrades = new List<int>();
        //List<int> possibleUpgrades = new List<int>();


        //for (int i = 0; i < upgradeSlot.Length; i++)
        //{
        //    if (!playerUpgrades.Contains(i))
        //    {
        //        possibleUpgrades.Add(i);
                

        //    }


        //}

        




        //for (int i = 0; i < upgradeChoices; i++)
        //{
           
        //    int rng = Random.Range(0, possibleUpgrades.Count - 1);

        //    Instantiate(upgradeSlot[possibleUpgrades[rng]], options.transform);
        //    possibleUpgrades.RemoveAt(rng);

        //    Debug.Log("Picked Value: " + possibleUpgrades[rng]);
        //    Debug.Log("Possible Upgrades --------------------------------------------");

        //    string debug = "";
        //    foreach (int item in possibleUpgrades)
        //    {
        //        debug += (", " + item);

        //    }
        //    Debug.Log(debug);

        //}



















        //List<int> displayedUpgrades = new List<int>();

        //for (int i = 0; i < upgradeChoices; i++)
        //{
        //    List<int> possibleUpgrades = new List<int>();

        //    for (int x = 0; x < upgradeSlot.Length; x++)
        //    {
        //        if (upgradeSlot[x] != null && !displayedUpgrades.Contains(x))
        //        {
        //            possibleUpgrades.Add(x);
        //        }
        //    }

        //    if (possibleUpgrades[0] == null)
        //    {
        //        Debug.Log("No more possible upgrades");
        //        return;
        //    }

        //    for (int y = 0; y < upgradeSlot.Length; y++)
        //    {
        //        Debug.Log(y);
        //    }

        //    int rng = Random.Range(0, possibleUpgrades.Count);
        //    displayedUpgrades.Add(rng);

        //    Debug.Log("displaying " + upgradeSlot[possibleUpgrades[rng]]);

        //    Instantiate(upgradeSlot[possibleUpgrades[rng]], options.transform);
        //    upgradeSlot[possibleUpgrades[rng]] = null;
        //}


    }


    public void PickUpgrade(int upgrade)
    {
        playerUpgrades.Add(upgrade);
        Debug.Log(upgrade);

        //upgradeSlot[upgrade] = null;
        

        SceneManager.LoadScene(1);
    }

    private void CreateLevel()
    {
        generator = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();

        level++;

        if (level == levelsPerFloor)
        {
            if(floor == floorsPerGame)
            {
                generator.isBoss = true;
                //Debug.Log("boss");
            }

            generator.isReward = true;
            //Debug.Log("Reward");
        }

        if(level == levelsPerFloor + 1)
        {
            level = 1;
            floor++;
        }
       

        generator.numberOfRooms = startRooms + (floor * roomsPerFloor);
        Debug.Log(startRooms + (floor * roomsPerFloor));
        Debug.Log(startRooms + " + " + floor + " x " + roomsPerFloor);

        //set number of enemy spawns

        generator.MakeMap();

        
        
    }

    public void GoToNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(level == levelsPerFloor)
        {
            sceneIndex = 2;
        }

        SceneManager.LoadScene(sceneIndex);
    }

    void Update()
    {//reload scene, for testing purposes
        if (Input.GetKeyDown("f"))
        {
            GoToNextLevel();
        }
    }

    public void MoveToFloor(int floor)
    {
        SceneManager.LoadScene(floor);
    }
}
