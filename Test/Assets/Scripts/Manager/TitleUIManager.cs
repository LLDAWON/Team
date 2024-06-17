using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField]
    private Button _playStart;
    [SerializeField]
    private Button _loadData;
    [SerializeField]
    private Button _setting;

    private void Awake()
    {
        _playStart.onClick.AddListener(() => PlayStart());        
        _setting.onClick.AddListener(()=>  Setting());
    }

    private void Start()
    {
        if (SaveManager.Instance.GetData())
        {
            _loadData.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            _loadData.onClick.AddListener(() => LoadData());
        }
        else
        {
            _loadData.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
        }
    }

    private void PlayStart()
    {
        SceneManager.LoadScene("5FloorScene");

    }

    private void LoadData()
    {
        SaveManager.Instance.JsonLoad();
    }

    private void Setting()
    {

    }
}
