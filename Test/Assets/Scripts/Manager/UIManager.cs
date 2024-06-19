using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    { get { return _instance; } }

    [SerializeField]
    private GameObject _hintPenal;
    [SerializeField]
    private GameObject _cursor;
    [SerializeField]
    private GameObject _miniMap;
   
    private GameObject _4Door;

    private bool _inventoryOpen = false;
    private bool _miniMapOpen = false;
    private List<int> _preTxTKey = new List<int>();

    private TextData _textData;
    
    private SystemTXT _text;
    private Inventory _inventory;
    private ArchiveTXT _archive;
    private GameObject _battery;
    private KeySlider _keySlider;
    private TextMeshProUGUI _candleCount;
    private GameObject _ventEventUI;

    private int _maxCandle;
    private int _curCandle;

    public void AddCandle()
    {
        _curCandle++;
    }

    public Inventory GetInventory
    { get { return _inventory; } }
    public GameObject Battery 
    {  get { return _battery; } }
    public GameObject GetCursor
    { get { return _cursor; } }
    public KeySlider KeySlider
    { get { return _keySlider; } }
    public TextMeshProUGUI CandleUI
    { get { return _candleCount; } }
    public GameObject VentUI
    { get { return _ventEventUI; } }

    private void Awake()
    {
        _instance = this;

        _text = transform.GetChild(0).GetComponent<SystemTXT>();
        _inventory = transform.GetChild(1).GetComponent<Inventory>();
        _archive = transform.GetChild(2).GetComponent<ArchiveTXT>();
        _battery = transform.GetChild(3).gameObject;
        _keySlider = transform.GetChild(7).GetComponent<KeySlider>();
        _candleCount = transform.GetChild(8).GetComponent<TextMeshProUGUI>();
        _ventEventUI = transform.GetChild(9).gameObject;

        _battery.SetActive(false);

        _curCandle = 5;
        _maxCandle = 6;
        _candleCount.gameObject.SetActive(false);
        _ventEventUI.SetActive(false);
        _miniMap.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            _inventoryOpen = !_inventoryOpen;
            _inventory.gameObject.SetActive(_inventoryOpen) ;
            Cursor.visible = _inventoryOpen;

            if(_inventoryOpen)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (SceneManager.GetActiveScene().name == "3FloorScene" || SceneManager.GetActiveScene().name == "VentScene")
                return;
            _miniMapOpen = !_miniMapOpen;
            _miniMap.SetActive(_miniMapOpen) ;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _ventEventUI.SetActive(false) ;
        }

    }
    public IEnumerator Candle()
    {
        while(_candleCount.gameObject.activeSelf)
        {
            yield return null;
            _candleCount.text = _curCandle.ToString() + "/" + _maxCandle.ToString();
            _4Door = GameObject.Find("AnimationDoor");
            if (_curCandle == _maxCandle)
            {
                SetText(9);
                _candleCount.gameObject.SetActive(false);
                _4Door.GetComponent<Animation>().Play();
                break;
            }
        }
    }

    public void SetText(int key)
    {
        _textData = DataManager.Instance.GetTextData(key);

        if (CheckRedundancy(key)) return;
        _preTxTKey.Add(key);
        if (_textData.Type == 1)
        {
            _archive.SetText(_textData);
        }
        else if (_textData.Type == 0 && _textData.Key != 0)
        {
            _text.SetText(_textData);
        }
        else if (_textData.Type == 2)
        {
            _hintPenal.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _textData.Text;
            _hintPenal.SetActive(true);
            Invoke("HintUI", 4);
        }
    }

    private bool CheckRedundancy(int key)
    {
        foreach (int preKey in _preTxTKey)
        {
            if (preKey == key)
            {
                return true;
            }
        }

        return false;
    }

    private void HintUI()
    {
        _hintPenal.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "4ChangeScene")
        {
            SceneManager.LoadScene("4FloorScene");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("4ChangeScene"))
        {
            SceneManager.LoadScene(1);
        }
    }

}
