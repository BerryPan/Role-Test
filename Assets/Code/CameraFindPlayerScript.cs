using UnityEngine;
using System.Collections;


public class CameraFindPlayerScript:MonoBehaviour
{
    float Distance = 3f;
    float Speed = 8f;
    float Angle = 0.02f;
    float spreadAngle = 0.1f;
    float delay=0.01f;
    float shake=0;
    int shateTime = 0;
    double t=0;
    public bool shouldShake = false;
    Vector3 m_TargetPosition;
    Transform follow;

    void Start ()
    {
        follow = GameObject.FindWithTag ("Player").transform;
    }

    void LateUpdate ()
    {
        //相机目标位置
        Angle-=Input.GetAxis("Mouse Y")*0.03f;
        Angle = Mathf.Clamp(Angle,-0.8f,1f);
        if(shouldShake)
		{
            shateTime=10;
            shouldShake = false;
        }
        if(shateTime>0)
        {
            t-=Time.deltaTime;
            if(t<=0)
            {
                shateTime--;
                shake = Random.Range(-spreadAngle,spreadAngle);
                t=delay;
            }
        }
        else
        {
            shake = 0;
        }
        

        Distance += Input.GetAxis("Mouse ScrollWheel") * 5;
        Distance = Mathf.Clamp(Distance,2f,20f);

        float height = Distance*Mathf.Sin(Angle+shake)+1.2f;
        float length = Distance*Mathf.Cos(Angle+shake);

        m_TargetPosition = follow.position + Vector3.up * height - follow.forward * length;
        //相机位置移动
        transform.position = Vector3.Lerp (transform.position, m_TargetPosition, Speed * Time.deltaTime);
        RaycastHit hit;
        if (Physics.Linecast(follow.position + 1.2f*Vector3.up,this.transform.position,out hit, LayerMask.GetMask("Default")))
        {
            //string name = hit.collider.gameObject.tag;
            //if (name != "MainCamera" && hit.collider.gameObject.layer!=10)
            if(hit.collider.gameObject.layer==0)
            {
                float currentDistance = Vector3.Distance(hit.point,follow.position);
                if (currentDistance < Distance)
                {
                    this.transform.position = hit.point;
                }
            }
        }
        //相机时刻看着人物
        transform.LookAt (follow.position + 1.2f*Vector3.up);
    }
}
