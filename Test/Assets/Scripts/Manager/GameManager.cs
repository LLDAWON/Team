using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;


    private GameObject _playerprefab;
    private GameObject _player;

    public GameObject GetPlayer() { return _player; }


    private void Awake()
    {
        Instance = this;

        _playerprefab = Resources.Load<GameObject>("Prefabs/Character/Player/Player");
        //_player = Instantiate(_playerprefab, new Vector3(8f, 1.5f, -10f), Quaternion.identity);
        _player = GameObject.Find("Player");
    }
    private void Start()
    {
        // MonsterManager �̱��� �ν��Ͻ� ��������
        MonsterManager monsterManager = MonsterManager.Instance;

        // MonsterManager�� ����Ͽ� Monster�� ����
        monsterManager.Spawn("Follow", new Vector3(-16f, 2.5f, -10f));


    }

}

