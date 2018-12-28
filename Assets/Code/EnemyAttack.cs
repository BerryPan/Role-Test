using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    public EnemyBehaviour EB;
    HealthDemo HD;
    float attacktime = 0f;
    public int damage;
    bool stay = false;
	// Use this for initialization
	void Start () {
        stay = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(stay)
        {
            if (attacktime <= 0 && EB.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8 && EB.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) 
            {
                HD.TakeDamage(damage);
                attacktime = 0.5f;
               // stay = false;
            }
        }

        if (attacktime > 0) attacktime -= Time.deltaTime;
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EB.anim.SetBool("Isattack", true);
            HD = other.GetComponent<HealthDemo>();
            stay = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EB.anim.SetBool("Isattack", false);
            stay = false;
        }
        
    }
}
