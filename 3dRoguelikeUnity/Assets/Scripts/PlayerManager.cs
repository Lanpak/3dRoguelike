using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject primary;
    public GameObject secondary;

    [SerializeField] private float mouseSensitivity;


    public int maxHitpoints;
    public int boostedHitpoints;
    public int hitpoints;

    private GunScript primaryScript;
    private GunScript secondaryScript;

    public Camera cam;
    public GameObject playerIcon;
    private GameObject arrow;
    public GameObject miniMapPosition;
    public GameObject miniMapContainer;


    public AudioClip takeDamageClip;
    public AudioClip deathClip;
    private AudioSource source;

    private LevelManager manager;
    public bool usingHp;
    public bool usingArmour;
    public float armourEffectiveness;

    public PauseMenu pauseManager;
    public MusicManager musicManager;


    public Slider hpSlider;

    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<LevelManager>();

        if (manager.playerUpgrades.Contains(8)) { usingArmour = true; }
        if (manager.playerUpgrades.Contains(9)) { usingHp = true; maxHitpoints = boostedHitpoints; }



        hitpoints = maxHitpoints;
        source = gameObject.GetComponent<AudioSource>();
        //primaryScript = primary.GetComponent<GunScript>();
        //secondaryScript = secondary.GetComponent<GunScript>();
        miniMapContainer = GameObject.Find("Canvas").transform.Find("GameUI").transform.Find("Minimap").gameObject;
        miniMapPosition = miniMapContainer.transform.Find("PlayerIcon").gameObject;
        hpSlider = GameObject.Find("Canvas").transform.Find("GameUI").transform.Find("Health").transform.Find("HP").GetComponent<Slider>();

        if (usingHp)
        {
            hpSlider.maxValue = boostedHitpoints;
        }
        MoveIntoRoom(0,0);
    }


    public void MoveIntoRoom(int x, int y)
    {
        foreach (Transform child in miniMapPosition.transform)
        {
            Destroy(child.gameObject);
        }
        Instantiate(playerIcon, new Vector2(miniMapContainer.transform.position.x + x*25, miniMapContainer.transform.position.y + y*25), Quaternion.identity, miniMapPosition.transform);
    }


    public void ResetPlayer()
    {
        hitpoints = maxHitpoints;


    }

    bool rndbool
    {
        get { return (Random.value > armourEffectiveness); }
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.other.CompareTag("EnemyProjectile"))
        {
            Projectile enemproj = collision.other.GetComponent<Projectile>();

            
            int dam = enemproj.damage;
            Destroy(enemproj.gameObject);

            if(hitpoints - dam <= 0)
            {
                hitpoints = 0;
                PlayerDied();
            }
            else
            {
                if (usingArmour)
                {
                    
                    if(rndbool)
                    {
                        hitpoints -= dam;
                        Debug.Log("dmg");
                    }
                    else
                    {
                        Debug.Log("OH MAH GAWD");
                    }
                }
                else
                {
                    Debug.Log("in this loo[p");
                    hitpoints -= dam;
                }
                

                source.clip = takeDamageClip;
                source.Play();
            }
        }
            
    }

    public void PlayerDied()
    {
        source.clip = deathClip;
        source.Play();

        SceneManager.LoadScene(0);
    }



    void Update()
    {
        hpSlider.value = hitpoints;
    }
}
