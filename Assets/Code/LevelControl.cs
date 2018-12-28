using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    public GameObject[] enemies;
    //public GameObject player;
    public Transform spawnPoints;
    public Score SS;

    GameObject smallList;
    GameObject BossList;
    public Levelprint LP;

    public int Level = 1;
    // Use this for initialization
    void Start()
    {
        smallList = GameObject.Find("LevelDesign/SmallMonsterList");
        BossList = GameObject.Find("LevelDesign/BossList");
    }

    // Update is called once per frame
    void Update()
    {
        if (smallList.transform.childCount + BossList.transform.childCount == 0)
        {
            switch (Level)
            {
                case 1: CreateLevel(8, 0, 0, 0, 0); LP.LevelPrint(Level); Level++; break;
                case 2: CreateLevel(0, 8, 0, 0, 0); LP.LevelPrint(Level); Level++; break;
                case 3: CreateLevel(0, 0, 8, 0, 0); LP.LevelPrint(Level); Level++; break;
                case 4: CreateLevel(0, 0, 0, 8, 0); LP.LevelPrint(Level); Level++; break;
                case 5: CreateLevel(4, 4, 4, 4, 0); LP.LevelPrint(Level); Level++; break;
                case 6: CreateLevel(2, 2, 2, 2, 1); LP.LevelPrint(Level); Level++; break;
                case 7: CreateLevel(1, 1, 1, 1, 2); LP.LevelPrint(Level); Level++; break;
                case 8: CreateLevel(6, 4, 10, 10, 2); LP.LevelPrint(Level); Level++; break;
                case 9: CreateLevel(0, 0, 0, 0, 4); LP.LevelPrint(Level); Level++; break;
                default: break;
            }
        }

    }
    void CreateLevel(int B, int R, int G, int S, int BOSS)
    {
        int index;
        Transform spawnPoint;
        GameObject go;
        EnemyBehaviour goEB;
        int[] list = new int[4] { B, R, G, S };

        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < list[j]; i++)
            {
                index = Random.Range(0, spawnPoints.childCount);
                spawnPoint = spawnPoints.GetChild(index);
                go = Instantiate(enemies[j], spawnPoint.position, spawnPoint.rotation, smallList.transform);
                goEB = go.GetComponent<EnemyBehaviour>();
                goEB.S = SS;
                goEB.patrolPoints = spawnPoints;
                goEB.isBOSS = false;
            }
        }

        for (int i = 0; i < BOSS; i++)
        {
            int enemyIndex = Random.Range(0, enemies.Length);
            index = Random.Range(0, spawnPoints.childCount);
            spawnPoint = spawnPoints.GetChild(index);
            go = Instantiate(enemies[enemyIndex], spawnPoint.position, spawnPoint.rotation, BossList.transform);
            goEB = go.GetComponent<EnemyBehaviour>();
            goEB.S = SS;
            goEB.patrolPoints = spawnPoints;
            goEB.isBOSS = true;
            goEB.smallMonster = enemies[enemyIndex];
            goEB.SmallMonsterList = smallList;
        }
    }
}
