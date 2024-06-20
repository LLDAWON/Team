using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{

    //¸ó½ºÅÍ ÇÁ¸®ÆÕ
    private Dictionary<string, GameObject> _monsterPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _spawnedMonsters = new Dictionary<string, GameObject>();

    private GameObject _curEnemy;
    // private List<GameObject> _monsterPrefab = new List<GameObject>();

    public GameObject GetCurEnemy() { return _curEnemy;}
    private List<GameObject> _spawnMonsters = new List<GameObject>();  //½ºÆùµÈ¸ó½ºÅÍ

    public List<GameObject> GetSpawnMonster() { return _spawnMonsters;}

    private void Awake()
    {
        LoadMonsterPrefabs();
    }



    private void LoadMonsterPrefabs()
    {
        //5Ãþ
        GameObject followEnemyPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Follow");
        if (followEnemyPrefab != null)
        {
            _monsterPrefabs.Add("Follow", followEnemyPrefab);
        }

        //4Ãþ
        GameObject darkEnemyPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Dark");
        if (darkEnemyPrefab != null)
        {
            _monsterPrefabs.Add("Dark", darkEnemyPrefab);
        }

        //3Ãþ
        GameObject teacherEnemyPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Teacher");
        if (teacherEnemyPrefab != null)
        {
            _monsterPrefabs.Add("Teacher", teacherEnemyPrefab);
        }

        //ÂÉ²¿¹Ìµé
        GameObject cat = Resources.Load<GameObject>("Prefabs/Character/Enemy/MiniMonster/Cat");
        if (cat != null)
        {
            _monsterPrefabs.Add("Cat", cat);
        }
        GameObject chicken = Resources.Load<GameObject>("Prefabs/Character/Enemy/MiniMonster/Chicken");
        if (chicken != null)
        {
            _monsterPrefabs.Add("Chicken", chicken);
        }
        GameObject Dog = Resources.Load<GameObject>("Prefabs/Character/Enemy/MiniMonster/Dog");
        if (Dog != null)
        {
            _monsterPrefabs.Add("Dog", Dog);
        }
        GameObject Elephant = Resources.Load<GameObject>("Prefabs/Character/Enemy/MiniMonster/Elephant");
        if (Elephant != null)
        {
            _monsterPrefabs.Add("Elephant", Elephant);
        }
        GameObject Pig = Resources.Load<GameObject>("Prefabs/Character/Enemy/MiniMonster/Pig");
        if (Pig != null)
        {
            _monsterPrefabs.Add("Pig", Pig);
        }
        GameObject Rabbit = Resources.Load<GameObject>("Prefabs/Character/Enemy/MiniMonster/Rabbit");
        if (Rabbit != null)
        {
            _monsterPrefabs.Add("Rabbit", Rabbit);
        }
        GameObject Unicorn = Resources.Load<GameObject>("Prefabs/Character/Enemy/MiniMonster/Unicorn");
        if (Unicorn != null)
        {
            _monsterPrefabs.Add("Unicorn", Unicorn);
        }

        //2Ãþ


        //1Ãþ

    }



    // Æ¯Á¤ ¸ó½ºÅÍ ¼ÒÈ¯ 
    public void Spawn(string monster, Vector3 pos)
    {
        GameObject spawnedMonster = Instantiate(_monsterPrefabs[monster], pos, Quaternion.identity);

        _spawnedMonsters.Add(monster, spawnedMonster);
        _spawnMonsters.Add(spawnedMonster);
        _curEnemy = spawnedMonster;
    }

    public void DestroyMonster(string monster)
    {

        if (_spawnedMonsters.ContainsKey(monster))
        {
            GameObject spawnedMonster = _spawnedMonsters[monster];
            EnemyController enemyController = spawnedMonster.GetComponent<EnemyController>();
            spawnedMonster.SetActive(false);
            _spawnMonsters.Remove(spawnedMonster);

        }

    }

}