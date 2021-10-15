using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject primary;
    public GameObject secondary;

    private GunScript primaryScript;
    private GunScript secondaryScript;

    void Start()
    {
        primaryScript = primary.GetComponent<GunScript>();
        secondaryScript = secondary.GetComponent<GunScript>();
    }

}
