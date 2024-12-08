using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extinguisherbar : MonoBehaviour
{
    [SerializeField] private Slider bar;
    private const float MAX = 180f; 
    private float maxHP = MAX;
    public float curHP = MAX;

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
        curHP -= 1;
        curHP = Mathf.Clamp(curHP, 0f, maxHP); 
    }

    public void FillBar()
    {
        maxHP = MAX;
    }
}