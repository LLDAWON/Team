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

    private bool _inventoryOpen = false;

    private TextData _textData;
    
    private SystemTXT _text;
    private Image _conditionKey;
    private Inventory _inventory;
    private ArchiveTXT _archive;

    public Inventory GetInventory() { return _inventory; }
    public Image ConditionKey
    { get { return _conditionKey; } }

    public void SetText(int key)
    {
        _textData = DataManager.Instance.GetTextData(key);

        if(_textData.Type == 1)
        {
            _archive.SetText(_textData);
        }
        else if(_textData.Type == 0)
        {
            _text.SetText(_textData);
        }
    }

    private void Awake()
    {
        _instance = this;

        _text = transform.GetChild(0).GetComponent<SystemTXT>();
        _conditionKey = transform.GetChild(1).GetComponent<Image>();
        _inventory = transform.GetChild(2).GetComponent<Inventory>();
        _archive = transform.GetChild(3).GetComponent<ArchiveTXT>();

        _conditionKey.gameObject.SetActive(false);        
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

}
