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
    private List<Button> _buttons = new List<Button>();
    [SerializeField]
    private GameObject _soundSetting;
    [SerializeField]
    private AudioSource _audioSource;


    private void Awake()
    {
        //_playStart.onClick.AddListener(() => PlayStart());        
        //_setting.onClick.AddListener(()=>  Setting());
        _soundSetting.SetActive(false);

        foreach(Button button in _buttons)
        {
            button.gameObject.AddComponent<AudioSource>();
            button.gameObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/UIButton") as AudioClip;
            button.gameObject.GetComponent<AudioSource>().playOnAwake = false;
        }

        _buttons[0].onClick.AddListener(() => PlayStart());
        _buttons[1].onClick.AddListener(() => LoadData());
        _buttons[2].onClick.AddListener(() => Setting());

    }

    private void Start()
    {
        if (SaveManager.Instance.GetData())
        {
            _buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            _buttons[1].onClick.AddListener(() => LoadData());
        }
        else
        {
            _buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;   

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _soundSetting.SetActive(false);
        }
    }

    private void PlayStart()
    {
        SceneManager.LoadScene("5FloorScene");
        _buttons[0].GetComponent<AudioSource>().Play();
    }

    private void LoadData()
    {
        SaveManager.Instance.JsonLoad();
        _buttons[1].GetComponent<AudioSource>().Play();
    }

    private void Setting()
    {
        _soundSetting.SetActive(true);
        _buttons[2].GetComponent<AudioSource>().Play();
    }
}
