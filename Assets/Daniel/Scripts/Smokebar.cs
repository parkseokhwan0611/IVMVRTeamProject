using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Smokebar : MonoBehaviour
{
    [SerializeField] private Slider smokebar;
    [SerializeField] GameObject player;
    private float maxHP = 180;
    private float curHP = 180;
    private float timeAccumulator = 0f; 
    public bool isHand = false;
    public Healthbar healthbar;
    private bool isAttacked = false;
    float imsi;
    void Awake()
    {
        smokebar.value = (float) curHP / (float) maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        imsi = (float)curHP/ (float) maxHP;
        HandleHp();
        UpdateTime();
    }

    private void HandleHp()
    {
        smokebar.value = Mathf.Lerp(smokebar.value, imsi, Time.deltaTime * 10);
    }

    private void UpdateTime()
    {
        timeAccumulator += Time.deltaTime;

        if (timeAccumulator >= 1f && !isHand && curHP >= 0)
        {
            curHP -= 1f; 
            curHP = Mathf.Clamp(curHP, 0f, maxHP); 
            timeAccumulator = 0f;
        }
        else if(timeAccumulator >= 1f && isHand && curHP >= 0) {
            timeAccumulator = 0f;
        }
        else if(curHP <= 0 && !isAttacked) {
            healthbar.DamageFromFire();
            isAttacked = true;
            StartCoroutine(AttackedCor());
        }
    }
    public void AidBox() {
        curHP = maxHP;
    }
    public void Handkerchief() {
        isHand = true;
        StartCoroutine(HandKerchiefCor());
    }
    IEnumerator HandKerchiefCor() {
        yield return new WaitForSeconds(20);
        isHand = false;
    }
    IEnumerator AttackedCor() {
        yield return new WaitForSeconds(0.1f);
        isAttacked = false;
    }
}