using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor.Rendering;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject _playerprefab;
    private GameObject _player;

    //카메라 관련
    [SerializeField]
    private GameObject _mainCamera;
    [SerializeField] 
    private GameObject _cutSceneCamera;
    public GameObject GetMainCamera() { return _mainCamera; }

    //Manager 관리
    private EventManager _eventManager;
    private MonsterManager _monsterManager;

    // private Vector3 _playerSpawnPosition = new Vector3(41.3f, 0.8f, 16.9f);
    private Vector3 _playerSpawnPosition;
    private bool _isSpawning = false;

    public Vector3 _savePoint;
    public int _curEvent;
    public GameObject GetPlayer()
    { 
        if(_player == null)
        {
            _playerprefab = Resources.Load<GameObject>("Prefabs/Character/Player/Player");
            _player = Instantiate(_playerprefab, transform);
            UIManager.Instance.SetText(1);
            UIManager.Instance.SetText(2);
            CameraManager.Instance.StartFuzziness();
        }

        return _player; 
    }


    //디졸브관련
    protected float _desolveEndTime = 2.0f;
    protected bool _desolveStart = false;
    protected float _desolveSpeed = 0.3f;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // 이 오브젝트를 씬 전환 시 파괴되지 않도록 설정
        //Initialize(); // 초기에 awkae할 애들        

        //SoundManager.Instance.Play2D("BG", true);
    }

    private void Start()
    {
        Initialize();
        SoundManager.Instance.Play2D("BG", true);
    }

    private void Initialize()
    {
        
        //_playerSpawnPosition = GameObject.Find("PlayerSpawn").transform.position;

        _monsterManager = MonsterManager.Instance;
        _eventManager = _player.GetComponent<EventManager>();
        _curEvent = _eventManager.CurKey;
        Observer.OnDesolveEvents.Add(1, DisolveEffect);
    }
    private void PlayerSpawn()
    {
        _playerSpawnPosition = GameObject.Find("PlayerSpawn").transform.position;
        _player.transform.position = _playerSpawnPosition;

        UIManager.Instance.SetText(1);
        UIManager.Instance.SetText(2);
    }
    private void Update()
    {
         Floor5MonsterSpawn();

        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene(3);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            List<int> item = UIManager.Instance.GetInventory.SaveItem;
            SaveManager.Instance.JsonSave(_player.transform.position, _curEvent, item);
            Debug.Log("curSceneSave:" + SceneManager.GetActiveScene().name);
        }
    }
    private void Floor5MonsterSpawn()
    {
        if (_isSpawning) return;

        if (_eventManager != null)
        {
            int currentEvent = _eventManager.CurKey;

            if (_eventManager.CurKey == 6)
            {
                _monsterManager.Spawn("Follow", new Vector3(2.4f, 0.8f, -5.4f));
                _isSpawning = true;
                SoundManager.Instance.Play3D("Monster", new Vector3(2.4f, 0.8f, -5.4f), false   );
                CameraManager.Instance.Shake(2.0f, 2.0f);
                return;
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

    public void ChangeCamera()
    {
        _mainCamera.GetComponent<Camera>().enabled = false;
        _cutSceneCamera.GetComponent<Camera>().enabled = true;
        
        Animator _cutSceneAnimator = _cutSceneCamera.GetComponent<Animator>();
        _cutSceneAnimator.SetTrigger("Destroy");
    }
    public void RestoreMainCamera()
    {
        _cutSceneCamera.GetComponent<Camera>().enabled = false;
        _mainCamera.GetComponent<Camera>().enabled = true;
    }


}




