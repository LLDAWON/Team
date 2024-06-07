using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{


    private Dictionary<string, GameObject> _monsterPrefab = new Dictionary<string, GameObject>();
   // private List<GameObject> _monsterPrefab = new List<GameObject>();


    private void Awake()
    {


    }


    private void Start()
    {
        
    }



    // 특정 몬스터 소환 
    private void Spawn(string monster,Vector3 pos)
    {
        if (monster == null)
            return;

        if(monster =="Follow")
        {

        }
    }

    private void DestroyMonster(string monster)
    {

    }

}
