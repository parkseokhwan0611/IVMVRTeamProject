using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public MonsterBehavior monsterBehavior;
    public Image hpBar;
    public GameObject background;
    public float hpAmount;
    private float currentHpFill;
    private Quaternion fixedRotation;
    public float changeSpeed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        //fixedRotation = transform.rotation;
        currentHpFill = monsterBehavior.hp / monsterBehavior.maxHp;
    }
    void LateUpdate() {
        //transform.rotation = fixedRotation;
    }
    void Awake() {
        hpAmount = monsterBehavior.GetComponent<MonsterBehavior>().hp;
    }
    // Update is called once per frame
    void Update()
    {
        HpChange();
    }
    void HpChange() {
        hpAmount = monsterBehavior.hp;
        float targetFill = hpAmount /monsterBehavior.maxHp;
        currentHpFill = Mathf.MoveTowards(currentHpFill, targetFill, changeSpeed * Time.deltaTime);
        hpBar.fillAmount = currentHpFill;
    }
}
