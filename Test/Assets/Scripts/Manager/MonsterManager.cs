using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{

    //몬스터 프리팹
    private Dictionary<string, GameObject> _monsterPrefabs = new Dictionary<string, GameObject>();

    private Dictionary<string, GameObject> _spawnedMonsters = new Dictionary<string, GameObject>(); //스폰된몬스터
    // private List<GameObject> _monsterPrefab = new List<GameObject>();


    private void Awake()
    {
        LoadMonsterPrefabs();
    }



    private void LoadMonsterPrefabs()
    {
        GameObject followEnemyPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/FollowEnemy");
        if (followEnemyPrefab != null)
        {
            _monsterPrefabs.Add("Follow", followEnemyPrefab);
        }

        GameObject teacherEnemyPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Teacher");
        if (teacherEnemyPrefab != null)
        {
            _monsterPrefabs.Add("Teacher", teacherEnemyPrefab);
        }
    }



    // 특정 몬스터 소환 
    public void Spawn(string monster, Vector3 pos)
    {
        if (!_monsterPrefabs.ContainsKey(monster))
        {
            Debug.Log("몬스터프리팹이 존재하지않습니다.");
            return;

        }

        GameObject spawnedMonster = Instantiate(_monsterPrefabs[monster], pos, Quaternion.identity);
        _spawnedMonsters.Add(monster, spawnedMonster);

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