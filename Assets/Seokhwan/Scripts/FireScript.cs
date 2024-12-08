using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public bool isAttacked;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Smoke" && !isAttacked) {
            hp -= 2.5f;
            isAttacked = true;
            if(hp <= 0) {
                Destroy(gameObject);
            }
            StartCoroutine(AttackedCor());
        }
    }
    IEnumerator AttackedCor() {
        yield return new WaitForSeconds(0.1f);
        isAttacked = false;
    }
}
