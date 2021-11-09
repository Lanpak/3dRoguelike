using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    public GunScript gs;


    
    
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gs.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, gs.snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(gs.recoilX, Random.Range(-gs.recoilY, gs.recoilY), Random.Range(-gs.recoilZ, gs.recoilZ));
    }

}
