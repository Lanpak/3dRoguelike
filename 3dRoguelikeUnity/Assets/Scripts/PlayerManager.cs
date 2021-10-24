using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject primary;
    public GameObject secondary;

    [SerializeField] private float mouseSensitivity;


    private GunScript primaryScript;
    private GunScript secondaryScript;

    public Camera cam;
    public GameObject playerIcon;
    private GameObject arrow;
    public GameObject miniMapPosition;
    public GameObject miniMapContainer;

    void Start()
    {
        
        //primaryScript = primary.GetComponent<GunScript>();
        //secondaryScript = secondary.GetComponent<GunScript>();
        miniMapContainer = GameObject.Find("Canvas").transform.Find("GameUI").transform.Find("Minimap").gameObject;
        miniMapPosition = miniMapContainer.transform.Find("PlayerIcon").gameObject;
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


}
