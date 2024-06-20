using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
    }

    [SerializeField]
    private GameObject _hintPanel;
    [SerializeField]
    private GameObject _cursor;
    [SerializeField]
    private GameObject _miniMap;
    [SerializeField]
    private GameObject _setting;
    [SerializeField]
    private GameObject _white;

    private GameObject _4Door;

    private bool _inventoryOpen = false;
    private bool _miniMapOpen = false;
    private bool _uiOpen = false;
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
    private float _escapeTime = 180.0f;

    public void AddCandle()
    {
        _curCandle++;
    }

    public Inventory GetInventory
    {
        get { return _inventory; }
    }

    public GameObject Battery
    {
        get { return _battery; }
    }

    public GameObject GetCursor
    {
        get { return _cursor; }
    }

    public KeySlider KeySlider
    {
        get { return _keySlider; }
    }

    public TextMeshProUGUI CandleUI
    {
        get { return _candleCount; }
    }

    public GameObject VentUI
    {
        get { return _ventEventUI; }
    }

    public GameObject White
    {
        get { return _white; }
    }

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        _instance = this;

        // �ʿ��� ������Ʈ ��������
        _text = transform.GetChild(0).GetComponent<SystemTXT>();
        _inventory = transform.GetChild(1).GetComponent<Inventory>();
        _archive = transform.GetChild(2).GetComponent<ArchiveTXT>();
        _battery = transform.GetChild(3).gameObject;
        _keySlider = transform.GetChild(7).GetComponent<KeySlider>();
        _candleCount = transform.GetChild(8).GetComponent<TextMeshProUGUI>();
        _ventEventUI = transform.GetChild(9).gameObject;

        // �ʱ� ����
        _battery.SetActive(false);
        _curCandle = 0;
        _maxCandle = 6;
        _candleCount.gameObject.SetActive(false);
        _ventEventUI.SetActive(false);
        _miniMap.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(CheckInputs()); // �Է� üũ �ڷ�ƾ ����
    }

    // �� �����Ӹ��� �Է��� üũ�ϴ� �ڷ�ƾ
    private IEnumerator CheckInputs()
    {
        while (true)
        {
            // �κ��丮 ���
            if (Input.GetKeyDown(KeyCode.I))
            {
                _inventoryOpen = !_inventoryOpen;
                _inventory.gameObject.SetActive(_inventoryOpen);
                Cursor.visible = _inventoryOpen;
                _uiOpen = _inventoryOpen;
            }

            // Ŀ�� ���� ����
            if (_uiOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // �̴ϸ� ���
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (SceneManager.GetActiveScene().name == "3FloorScene" || SceneManager.GetActiveScene().name == "VentScene")
                    continue;

                _miniMapOpen = !_miniMapOpen;
                _miniMap.SetActive(_miniMapOpen);
                _uiOpen = _miniMapOpen;
            }

            // ���� UI ���
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _uiOpen = !_uiOpen;
                _setting.gameObject.SetActive(_uiOpen);
            }

            yield return null; // ���� �����ӱ��� ���
        }
    }

    // ĵ�� UI ������Ʈ �ڷ�ƾ
    public IEnumerator Candle()
    {
        while (_candleCount.gameObject.activeSelf)
        {
            yield return null;
            _candleCount.text = _curCandle.ToString() + "/" + _maxCandle.ToString();
            _4Door = GameObject.Find("AnimationDoor");
            if (_curCandle == _maxCandle)
            {
                SetText(9);
                _candleCount.gameObject.SetActive(false);
                _4Door.GetComponent<Animation>().Play();
                SoundManager.Instance.Play3D("Door", _4Door.transform.position, false);
                break;
            }
        }
    }

<<<<<<< Updated upstream
    // �ؽ�Ʈ ����
=======
    public IEnumerator EscapeTime()
    {
        while (_escapeTime > 0.0f)
        {

            int minute=(int)_escapeTime/60;
            int second = (int)_escapeTime%60;
            _escapeTime -= Time.deltaTime;
            _candleCount.text = minute.ToString() + " : " + second.ToString();


            yield return null;
        }
        _candleCount.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

>>>>>>> Stashed changes
    public void SetText(int key)
    {
        _textData = DataManager.Instance.GetTextData(key);

        if (CheckRedundancy(key)) return;
        _preTxTKey.Add(key);

        switch (_textData.Type)
        {
            case 1:
                _archive.SetText(_textData);
                break;
            case 0 when _textData.Key != 0:
                _text.SetText(_textData);
                break;
            case 2:
                _hintPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _textData.Text;
                _hintPanel.SetActive(true);
                Invoke("HintUI", 4);
                break;
        }
    }

    // �ߺ� üũ
    private bool CheckRedundancy(int key)
    {
        if (key == 1 || key == 9) return false;

        return _preTxTKey.Contains(key);
    }

    // ��Ʈ UI ��Ȱ��ȭ
    private void HintUI()
    {
        _hintPanel.SetActive(false);
    }
}
