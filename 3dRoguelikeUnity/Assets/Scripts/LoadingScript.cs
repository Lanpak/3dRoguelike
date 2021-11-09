using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class LoadingScript : MonoBehaviour
{
    public AudioSource source;
    public float loadTime;

    private LevelManager manager;


    public TextMeshProUGUI levelText;
    public TextMeshProUGUI stageText;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<LevelManager>();

        if(manager.level == manager.levelsPerFloor)
        {
            levelText.text = "Level 1";
            stageText.text = "stage " + (manager.floor + 1);
        }
        else
        {
            levelText.text = "Level " + (manager.level + 1);
            stageText.text = "stage " + manager.floor;
        }
        

        Invoke("LoadGameScene", loadTime);
        source.Play();
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
