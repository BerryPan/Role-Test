using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour {
	public GameObject[] enemies;
	public GameObject player;

	GameObject enemyParent;

	private const float timeToFirstCreate = 5f;
	private const float rebornTime  = 2f;
	private const int enemyNum = 5;
	// Use this for initialization
	void Awake()
	{
		enemyParent = GameObject.Find("EnemyParent");
		if(!enemyParent)
		{
			enemyParent = new GameObject("EnemyParent");
		}
		//enemies = enemyParent.GetChild();
	}
	void Start () {
		InvokeRepeating("Spawn",timeToFirstCreate,rebornTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Spawn()
	{
		if(enemyParent.transform.childCount>=enemyNum) return ;

		int index = Random.Range(0,transform.childCount);
		Transform spawnPoint = transform.GetChild(index);

		int enemyIndex = Random.Range(0,enemies.Length);
		GameObject go = Instantiate(enemies[enemyIndex],spawnPoint.position,spawnPoint.rotation,enemyParent.transform);

		//go.GetComponent<EnemyBehaviour>().patrolPoints=GameObject.Find("PatrolPoint").transform;
		go.GetComponent<EnemyBehaviour>().Init();
		
	}
}
