using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private string name;
    [SerializeField] private int damage;
    [SerializeField] private float firingSpeed;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private bool fullAuto;
    [SerializeField] private int magSize;


    [Header("Shotgun Settings")]
    [SerializeField] private bool isShotgun = false;
    [SerializeField] private int bulletsPerShot = 6;
    [SerializeField] private float spreadX;
    [SerializeField] private float spreadY;
    [SerializeField] private float spreadZ;

    [Header("Sway")]
    public float amount;
    public float maxAmount;
    public float smoothAmount;

    [Header("Recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float snappiness;
    public float returnSpeed;

    [Header("Projectile")]
    [SerializeField] private bool isProjectile;
    public GameObject projectile;
    public Transform shootPoint;
    public float projectileSize;
    [SerializeField] private float shootForce;
    [SerializeField] private float arcForce;


    [Header("ParticleSystems")]

    [SerializeField] private ParticleSystem muzzleFlash;



    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Camera camera;
    [SerializeField] private Image crosshairs;
    [SerializeField] private Recoil recoilScript;
    [SerializeField] private Animator anim;


    [HideInInspector] public int bulletsInMag;
    

    private float nextTimeToFire = 0;
    private Vector3 initialPosition;
    private AnimatorStateInfo info;
    private bool isReloading = false;

    void Start()
    {
        initialPosition = transform.localPosition;
        bulletsInMag = magSize;
    }

    // Update is called once per frame
    void Update()
    {
        HandleSway();

        if(bulletsInMag != magSize && Input.GetButtonDown("Reload"))
        {
            Reload();
        } 

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
    }


    void FixedUpdate()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);

        isReloading = info.IsName(name + "_reload");
    }



    private void Fire()
    {
        if (isReloading)
        {
            return;
        }
        if (bulletsInMag == 0)
        {
            Reload();
        }
        else
        {
            if (isShotgun)
            {
                muzzleFlash.Play();
                bulletsInMag--;
                recoilScript.RecoilFire();
                anim.Play(name + "_fire");
                for (int i = 0; i < bulletsPerShot; i++)
                {

                    if (!isProjectile)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(camera.transform.position, HandleSpread(), out hit))
                        {
                            if (hit.collider.gameObject.transform.root.CompareTag("Shootable"))
                            {
                                hit.collider.gameObject.transform.root.GetComponent<Enemy>().TakeDamage(damage);
                            }
                        }
                    }
                    else
                    {
                        Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                        rb.AddForce(HandleSpread() * shootForce, ForceMode.Impulse);
                    }
                }
            }
            else
            {
                muzzleFlash.Play();
                bulletsInMag--;
                recoilScript.RecoilFire();
                anim.Play(name + "_fire");
                if (!isProjectile)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
                    {
                        if (hit.collider.gameObject.transform.root.CompareTag("Shootable"))
                        {
                            hit.collider.gameObject.transform.root.GetComponent<Enemy>().TakeDamage(damage);
                        }
                    }
                }
                else
                {
                    GameObject proj = Instantiate(projectile, shootPoint.position, Quaternion.identity);
                    proj.transform.localScale = new Vector3(projectileSize, projectileSize, projectileSize);
                    proj.GetComponent<Projectile>().damage = damage;
                    Rigidbody rb = proj.GetComponent<Rigidbody>();
                    rb.AddForce(transform.up * arcForce, ForceMode.Impulse);
                    rb.AddTorque(new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)), ForceMode.Impulse);
                    rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
                }
                
            }


            
        }  
    }

    private void Reload()
    {
        if(isReloading)
        {
            return;
        }
        anim.Play(name + "_reload");
        bulletsInMag = magSize;
    }

    private void HandleSway()
    {
        float movementX = Input.GetAxis("Mouse X") * amount;
        float movementY = Input.GetAxis("Mouse Y") * amount;

        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);


        Vector3 finalPosition = new Vector3(movementX, movementY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }


    private Vector3 HandleSpread()
    {
        Vector3 targetPos = camera.transform.position + camera.transform.forward;
        targetPos = new Vector3
            (
            targetPos.x + Random.Range(-spreadX, spreadX),
            targetPos.y + Random.Range(-spreadY, spreadY),
            targetPos.z + Random.Range(-spreadZ, spreadZ)
            );
        Vector3 direction = targetPos - camera.transform.position;
        return direction.normalized;
    }
}


