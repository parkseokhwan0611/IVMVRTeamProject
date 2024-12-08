using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public Extinguisherbar exbar;
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint; // 총알 발사 위치
    public float bulletSpeed = 1f;
    public bool isPressed = false;
    public bool isFireOn = false;
    public static bool isFireExtOn = false;
    // Update is called once per frame
    void Awake()
    {
        exbar = FindObjectOfType<Extinguisherbar>();
    }
    void Update()
    {
        float handRight = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        if((handRight > 0 && isFireExtOn == true) || Input.GetMouseButton(0)) {
        //if(Input.GetMouseButton(0)) {
            isPressed = true;
            if(!isFireOn) {
                StartCoroutine(Water());
                isFireOn = true;
            }
        }
        else {
            isPressed = false;
            isFireOn = false;
        }
    }
    IEnumerator Water() {
        while (isPressed && exbar.curHP != 0) {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (exbar != null)
            {
                exbar.WaterSprayed();
            }

            if (rb != null)
            {
                rb.velocity = firePoint.forward * bulletSpeed;
            }
            Destroy(bullet, 1f);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
