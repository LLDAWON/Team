using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{

    //���� ������
    private Dictionary<string, GameObject> _monsterPrefabs = new Dictionary<string, GameObject>();

    private Dictionary<string, GameObject> _spawnedMonsters = new Dictionary<string, GameObject>(); //�����ȸ���
    private GameObject _curEnemy;
    // private List<GameObject> _monsterPrefab = new List<GameObject>();

    public GameObject GetCurEnemy() { return _curEnemy;}

    private void Awake()
    {
        LoadMonsterPrefabs();
    }



    private void LoadMonsterPrefabs()
    {
        //5��
        GameObject followEnemyPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Follow");
        if (followEnemyPrefab != null)
        {
            _monsterPrefabs.Add("Follow", followEnemyPrefab);
        }

        //4��
        GameObject darkEnemyPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Dark");
        if (darkEnemyPrefab != null)
        {
            _monsterPrefabs.Add("Dark", darkEnemyPrefab);
        }

        //3��
        GameObject teacherEnemyPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Teacher");
        if (teacherEnemyPrefab != null)
        {
            _monsterPrefabs.Add("Teacher", teacherEnemyPrefab);
        }
        
        //2��


        //1��

    }



    // Ư�� ���� ��ȯ 
    public void Spawn(string monster, Vector3 pos)
    {
        GameObject spawnedMonster = Instantiate(_monsterPrefabs[monster], pos, Quaternion.identity);
        _spawnedMonsters.Add(monster, spawnedMonster);
        _curEnemy = spawnedMonster;

    }

    private void DestroyMonster(string monster)
    {
        if (_spawnedMonsters.ContainsKey(monster))
        {
            GameObject spawnedMonster = _spawnedMonsters[monster];
            EnemyController enemyController = spawnedMonster.GetComponent<EnemyController>();
            enemyController.DestroyMonster();
            _spawnedMonsters.Remove(monster);
            
        }
    }

}