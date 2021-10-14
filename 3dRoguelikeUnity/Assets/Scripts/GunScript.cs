using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float damage;
    [SerializeField] private float firingSpeed;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private bool fullAuto;
    [SerializeField] private int magSize;


    [Header("Recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float snappiness;
    public float returnSpeed;


    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Camera camera;
    [SerializeField] private Image crosshairs;
    [SerializeField] private Recoil recoilScript;
    [SerializeField] private Animator anim;

    private float nextTimeToFire = 0;



    // Update is called once per frame
    void Update()
    {
        if (fullAuto)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f/firingSpeed;
                Fire();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / firingSpeed;
                Fire();
            }
        }
        if(Time.time <= nextTimeToFire)
        {
            anim.SetBool("isShooting", true);
        }
        else
        {

            anim.SetBool("isShooting", false);

        }
        
    
    }


    private void Fire()
    {
        recoilScript.RecoilFire();
        
        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
