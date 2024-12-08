using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extinguisherbar : MonoBehaviour
{
    [SerializeField] private Slider bar;
    private float maxHP = 180;
    private float curHP = 180;
    float imsi;

    void Awake()
    {
        bar.value = (float)curHP / (float)maxHP;
    }


    // Update is called once per frame
    void Update()
    {
        imsi = (float)curHP/ (float) maxHP;
        HandleHp();
    }

    private void HandleHp()
    {
        bar.value = Mathf.Lerp(bar.value, imsi, Time.deltaTime * 10);
    }

    public void WaterSprayed()
    {
        curHP -= 10;
        curHP = Mathf.Clamp(curHP, 0f, maxHP); 
    }
}