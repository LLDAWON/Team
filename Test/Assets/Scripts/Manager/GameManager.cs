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


    //디졸브관련
    protected float _desolveEndTime = 2.0f;
    protected bool _desolveStart = false;
    protected float _desolveSpeed = 0.3f;

    private void Awake()
    {
        Instance = this;

        _playerprefab = Resources.Load<GameObject>("Prefabs/Character/Player/Player");
       // _player = Instantiate(_playerprefab, _playerSpawnPosition, Quaternion.identity);
       // _eventManager = _player.GetComponent<EventManager>();
        _player = GameObject.Find("Player");
        _monsterManager = MonsterManager.Instance;

        Observer.OnDesolveEvents.Add(1, DisolveEffect);
    }
    private void Start()
    {
        // MonsterManager 싱글톤 인스턴스 가져오기
       // MonsterManager monsterManager = MonsterManager.Instance;

        // MonsterManager를 사용하여 Monster를 스폰
       // _monsterManager.Spawn("Pig", new Vector3(-3.89f, 1.28f, 27.77f));
        
        //PlayerSpawn();
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

    private void MiniMonsterSpawn()
    {
        if (!_isSpawning)
        {
            
            _isSpawning = true;
        }

    }

    public IEnumerator DisolveEffect(GameObject target)
    {

        Renderer[] _renderers = target.GetComponentsInChildren<Renderer>();

        float _time = 0.0f;

        while (_time < 2.0f)
        {
            _time += _desolveSpeed * Time.deltaTime;

            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_DesolveTime", _time);
            }

            yield return null;
        }
        if (_time > 2.0f)
        {
            target.SetActive(false);
        }
    }




}




