using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private GameObject _playerprefab;
    private GameObject _player;
    private EventManager _eventManager;
    public GameObject GetPlayer() { return _player; }
    private Vector3 _playerSpawnPosition = new Vector3(41.38365f, 0.8915222f, 16.93983f);
   private MonsterManager _monsterManager;
    private void Awake()
    {
        Instance = this;

        _playerprefab = Resources.Load<GameObject>("Prefabs/Character/Player/Player");
        _player = Instantiate(_playerprefab, new Vector3(8f, 1.5f, -10f), Quaternion.identity);
        _eventManager = _player.GetComponent<EventManager>();
        //  _player = GameObject.Find("Player");
        _monsterManager = MonsterManager.Instance;
    }
    private void Start()
    {
        // MonsterManager �̱��� �ν��Ͻ� ��������
       // MonsterManager monsterManager = MonsterManager.Instance;

        // MonsterManager�� ����Ͽ� Monster�� ����
       

        PlayerSpawn();
    }
    private void PlayerSpawn()
    {
        _player.transform.position = _playerSpawnPosition;
    }
    private void Update()
    {
       Floor5MonsterSpawn();
    }
    private void Floor5MonsterSpawn()
    {
        if (_eventManager != null)
        {
            int currentEvent = _eventManager.CurEvent();
            Debug.Log("Current Event Key: " + currentEvent);
            if (currentEvent == 6)
            {
                _monsterManager.Spawn("Follow", new Vector3(2.455653f, 0.880652f, -5.482373f));
            }
        }
        
    }


}

