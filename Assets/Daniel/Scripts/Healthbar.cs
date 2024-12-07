using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider hpbar;
    [SerializeField] GameObject player;
    [SerializeField] private GameObject[] fireObjects;
    private float damageRate = 8; 
    private float damageRadius = 2; 
    private float damageCooldown = 0;
    private float maxHP = 180;
    private float curHP = 180;
    float imsi;
    void Awake()
    {
        hpbar.value = (float) curHP / (float) maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        imsi = (float)curHP/ (float) maxHP;
        HandleHp();
        DamageFromFire();
    }

    private void HandleHp()
    {
        hpbar.value = Mathf.Lerp(hpbar.value, imsi, Time.deltaTime * 10);
    }

    private void DamageFromFire()
    {
        fireObjects = GameObject.FindGameObjectsWithTag("Fire");

        foreach (GameObject fire in fireObjects)
        {
            float distance = Vector3.Distance(player.transform.position, fire.transform.position);

            // Check if the player is within the damage radius
            if (distance <= damageRadius && Time.time >= damageCooldown)
            {
                curHP -= damageRate * Time.deltaTime; 
                curHP = Mathf.Clamp(curHP, 0f, maxHP); 
                damageCooldown = Time.time + 0.1f; 
            }
        }
    }

    public void DamageFromMonster()
    {
        curHP -= 20; 
        curHP = Mathf.Clamp(curHP, 0f, maxHP); 
    }
}