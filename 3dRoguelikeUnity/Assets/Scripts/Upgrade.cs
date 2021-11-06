using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
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




    public int upgradeCode;
    private LevelManager manager;

    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<LevelManager>();
    }

    public void PickUpgrade()
    {
        manager.PickUpgrade(upgradeCode);
        Debug.Log("upgrade: " + upgradeCode);
    }
}
