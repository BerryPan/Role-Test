using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot :  MonoBehaviour
 {
	private const float hitRange = 100f;
	//public GameObject muzzle;
	private const float spreadRange = 0.1f;
    private float force;
    double oneFire = 2f;

    public GameObject Grenade;
    public HealthDemo HD;
    public CameraFindPlayerScript CFPS;
    public BulletNum BN;
    public PausePanelControl PPC;

    // Use this for initialization
    void Start () {
        HD = GetComponent<HealthDemo>();
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
		{
			if(oneFire>0.3&&BN.num>1)
			{
                BN.num -= 1;
                CFPS.shouldShake = true;
				Vector3 aimDir = GetAimDir();
				Vector3 shootDir = GetShootDir(aimDir);
				HitDetect(Camera.main.transform.position,shootDir);
			}
			oneFire = 0;
		}
		if(!Input.GetButton("Fire1"))
		{
			if(oneFire<2) oneFire+=Time.deltaTime;
        }
		if(Input.GetButton("Fire2"))
		{
			if(force<1) force += Time.deltaTime;
		}
		else if(Input.GetButtonUp("Fire2"))
		{
            HD.TakeDamage(PPC.cost);
			GameObject go = Instantiate(Grenade,transform.position+Vector3.up+ Camera.main.transform.forward, transform.rotation);
			go.GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward+Vector3.up/2)*force*20,ForceMode.Impulse);
            go.GetComponent<DestoryGrenade>().damage = (int)(PPC.damage);
			force = 0;
		}
	}

	bool HitDetect(Vector3 from,Vector3 to)
	{
		RaycastHit _hitInfo;
		bool _hit = Physics.Raycast(from,to,out _hitInfo,(to-from).magnitude,LayerMask.GetMask("Enemy"));
		Debug.DrawLine(Camera.main.transform.position,_hitInfo.point, Color.cyan, 4);
		if(_hit)
		{
			RaycastHit hitInfo;
			bool hit = Physics.Raycast(from,to,out hitInfo,(to-from).magnitude,LayerMask.GetMask("Floor"));
            //Debug.Log(hitInfo.distance < _hitInfo.distance);
            if (hit)
            {
                if (hitInfo.distance < _hitInfo.distance)
                    return false;
            }
            HD.TakeDamage(-5);
            _hitInfo.collider.GetComponent<EnemyBehaviour>().TakeDamage((int)(PPC.damage/2));
            _hitInfo.collider.GetComponent<EnemyBehaviour>().target = this.gameObject;
            return true;
		}
		return false;
	}

	Vector3 GetShootDir(Vector3 aim)
	{
		return GetDistribution(spreadRange)*aim;
	}

	Quaternion GetDistribution(float range)
	{
		float radius = Random.Range(0,range) + Random.Range(0,range);
		radius = radius > range ?range * 2 - radius : radius;
		float phase = Random.Range(0,2*Mathf.PI);
		Quaternion rotY = Quaternion.Euler(0,Mathf.Cos(phase)*radius,0);
		Quaternion rotX = Quaternion.Euler(Mathf.Sin(phase)*radius,0,0);
		return rotX*rotY;
	}

	Vector3 GetAimDir()
	{
		RaycastHit hitInfo;
		bool hit = Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hitInfo,hitRange,LayerMask.GetMask("Shootable","Floor"));
		//Debug.DrawLine(Camera.main.transform.position,Camera.main.transform.position+Camera.main.transform.forward*100, Color.cyan, 4);
		if (hit)
		{
			return Camera.main.transform.forward;
		}
		else
		{
			return Camera.main.transform.forward;//Camera.main.transform.forward;
		}
	}
}
