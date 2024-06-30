using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    [SerializeField]
    private Button _main;
    [SerializeField]
    private Button _end;
    [SerializeField]
    private ClearTimeUI _clearTimeUI;

    private void Awake()
    {
        _main.onClick.AddListener(() => MainButton());
        _end.onClick.AddListener(() => EndButton());

        _main.gameObject.SetActive(false);
        _end.gameObject.SetActive(false);
        SoundManager.Instance.Stop3D("BallerinaBG");
    }

    private void Start()
    {
        NetWork timeTracker = FindObjectOfType<NetWork>();

        if (timeTracker != null)
        {
            timeTracker.OnEndingSceneEntered();
        }
    }

    private void Update()
    {
        transform.Translate(Vector2.up * 2.0f);

        Vector3 pos = transform.parent.transform.position;

        if(transform.position.y >= pos.y)
        {
            transform.position = pos;
            _main.gameObject.SetActive(true);
            _end.gameObject.SetActive(true);
            Invoke("EndCredit", 2.0f);
        }
    }

    private void MainButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void EndButton()
    {
        Application.Quit();
    }

    private void EndCredit()
    {
        gameObject.SetActive(false);
        _clearTimeUI.GetClearTime();
    }
}
