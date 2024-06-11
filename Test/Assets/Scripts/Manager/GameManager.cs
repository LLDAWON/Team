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
    private MonsterManager _monsterManager;

    private Vector3 _playerSpawnPosition = new Vector3(41.3f, 0.8f, 16.9f);
    private bool _isSpawning = false;
    public GameObject GetPlayer() { return _player; }
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
        UIManager.Instance.SetText(1);
        UIManager.Instance.SetText(2);
    }
    private void Update()
    {
       Floor5MonsterSpawn();
    }
    private void Floor5MonsterSpawn()
    {
        if (_isSpawning) return;

        if (_eventManager != null)
        {
            int currentEvent = _eventManager.CurEvent();
            //Debug.Log("Current Event Key: " + currentEvent);
            if (currentEvent == 6)
            {
                _monsterManager.Spawn("Follow", new Vector3(2.4f, 0.8f, -5.4f));
                _isSpawning = true;
            }
        }
        
    }


        


  }




