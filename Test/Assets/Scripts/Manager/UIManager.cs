using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private bool _inventoryOpen = false;
    private List<int> _preTxTKey = new List<int>();

    private TextData _textData;
    
    private SystemTXT _text;
    private Inventory _inventory;
    private ArchiveTXT _archive;
    private GameObject _battery;

    public Inventory GetInventory
    { get { return _inventory; } }
    public GameObject Battery 
    {  get { return _battery; } }
    public GameObject GetCursor
    { get { return _cursor; } }
    public void SetText(int key)
    {
        _textData = DataManager.Instance.GetTextData(key);

        if (CheckRedundancy(key)) return;

        if (_textData.Type == 1)
        {
            _archive.SetText(_textData);
        }
        else if(_textData.Type == 0 && _textData.Key != 0)
        {
            _text.SetText(_textData);
        }
        else if(_textData.Type ==2)
        {
            _hintPenal.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _textData.Text;
            _hintPenal.SetActive(true);
            Invoke("HintUI", 4);
        }
    }

    private void Awake()
    {
        _instance = this;

        _text = transform.GetChild(0).GetComponent<SystemTXT>();
        _inventory = transform.GetChild(1).GetComponent<Inventory>();
        _archive = transform.GetChild(2).GetComponent<ArchiveTXT>();
        _battery = transform.GetChild(3).gameObject;

        _battery.SetActive(false);
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

}
