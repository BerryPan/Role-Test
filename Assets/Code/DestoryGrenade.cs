using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DestoryGrenade : MonoBehaviour {
    public HealthDemo HD;
    public int damage;
    float sph=0.3f;
    bool destory=false;
    // Use this for initialization
    void Start () {
        HD = GameObject.Find("Player").GetComponent<HealthDemo>();
    }
	
	// Update is called once per frame
	void Update () {
		if(destory)
        {
            if (sph < 4)
            {
                sph +=Time.deltaTime*4;
                this.gameObject.transform.localScale = 2*new Vector3( sph, sph, sph);
            }
            else Destroy(this.gameObject);
        }
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==10)
        {
            other.GetComponent<EnemyBehaviour>().TakeDamage(damage);
        }
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        destory = true;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }
}
