using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField]
    private Button _playStart;
    [SerializeField]
    private Button _loadData;
    [SerializeField]
    private Button _setting;
    [SerializeField]
    private GameObject _soundSetting;
    [SerializeField]
    private AudioSource _audioSource;


    private void Awake()
    {
        _playStart.onClick.AddListener(() => PlayStart());        
        _setting.onClick.AddListener(()=>  Setting());
        _soundSetting.SetActive(false);
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        _soundSetting.SetActive(true);
    }
}
