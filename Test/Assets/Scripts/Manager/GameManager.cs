using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        // MonsterManager �̱��� �ν��Ͻ� ��������
        MonsterManager monsterManager = MonsterManager.Instance;

        // MonsterManager�� ����Ͽ� Monster�� ����
        monsterManager.Spawn("Teacher", new Vector3(-16f, 1.5f, -10f));
    }
}

