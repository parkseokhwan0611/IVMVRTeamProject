using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Smokebar : MonoBehaviour
{
    [SerializeField] private Slider smokebar;
    [SerializeField] GameObject player;
    private float maxHP = 60;
    private float curHP = 60;
    private float timeAccumulator = 0f; 
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

        if (timeAccumulator >= 1f)
        {
            curHP -= 1f; 
            curHP = Mathf.Clamp(curHP, 0f, maxHP); 
            timeAccumulator = 0f; 
        }


    }
}