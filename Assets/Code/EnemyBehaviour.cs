using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehaviour :  MonoBehaviour
{
    public GameObject target;
    public Transform patrolPoints;
    public Score S;
    public Animator anim;
    public GameObject smallMonster;
    public bool isBOSS;
    public int health = 100;
    public GameObject SmallMonsterList;
    public float DieTime = 3f;

    float dietime;
    float newMonsterTime = 0f;
    private NavMeshAgent agent;
    private int currPatrolIndex;

    void Start()
    {
        dietime = DieTime;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if(isBOSS)
        {
            this.gameObject.transform.localScale = new Vector3(400, 400, 400);
            health *= 10;
        }
    }

    void FixedUpdate()
    {
        if (dietime <= 0) { Destroy(this.gameObject);}
        if (agent&&dietime==DieTime)
        {
            anim.SetBool("Iswalking", true);
            if (target)
            {
                agent.SetDestination(target.transform.position);
            }
            else if (!agent.hasPath)
            {
                currPatrolIndex = Random.Range(0, patrolPoints.childCount);
                agent.SetDestination(patrolPoints.GetChild(currPatrolIndex).position);
            }
            if (isBOSS&&newMonsterTime>10)
            {
                newMonsterTime = 0;
                Spawn();
            }
        }
        else
            anim.SetBool("Iswalking", false);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Damage")) anim.SetBool("Isdamage", false);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            dietime -= Time.deltaTime;
        if (isBOSS) newMonsterTime += Time.deltaTime;
    }

    public void Init()
    {
        float minDistSqr = float.MaxValue;
        for(int i = 0;i<patrolPoints.childCount;i++)
        {
            Transform currPoint = patrolPoints.GetChild(i);
            float distSqr = (transform.position - currPoint.position).sqrMagnitude;
            if(distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                currPatrolIndex = i;
            }
        }
        agent.SetDestination(patrolPoints.GetChild(currPatrolIndex).position);
    }
    void Spawn()
    {
        if (SmallMonsterList.transform.childCount >= 5) return;

        int index = Random.Range(0, transform.childCount);
        Transform spawnPoint = transform.GetChild(index);

        GameObject go = Instantiate(smallMonster, spawnPoint.position, spawnPoint.rotation, SmallMonsterList.transform);
        EnemyBehaviour goEB = go.GetComponent<EnemyBehaviour>();

        goEB.agent = go.GetComponent<NavMeshAgent>();
        goEB.S = S;
        goEB.anim = go.GetComponent<Animator>();
        if (target)
            goEB.target = target;
        else
            goEB.target = this.gameObject;

    }

    public void TakeDamage(int damage)
    {
        S.score += damage;
        health -= damage;
        anim.SetBool("Isdamage",true);
        if (health<=0)
        {
            dietime -= Time.deltaTime;
            anim.SetTrigger("Die");
            GetComponent<SphereCollider>().enabled = false;         
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}