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

    private void Awake()
    {
        _main.onClick.AddListener(() => MainButton());
        _end.onClick.AddListener(() => EndButton());

        _main.gameObject.SetActive(false);
        _end.gameObject.SetActive(false);
        SoundManager.Instance.Stop3D("BallerinaBG");
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
        }
    }

    private void MainButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void EndButton()
    {
        Application.Quit();
       // Destroy(gameObject);
    }
}
