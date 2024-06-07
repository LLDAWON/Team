using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        // MonsterManager 싱글톤 인스턴스 가져오기
        MonsterManager monsterManager = MonsterManager.Instance;

        // MonsterManager를 사용하여 Monster를 스폰
        monsterManager.Spawn("Teacher", new Vector3(-16f, 1.5f, -10f));
    }
}

