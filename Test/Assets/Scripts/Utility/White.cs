using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class White : MonoBehaviour
{
    private float _duration = 2f;
    public Image _whitePanel;

    void Start()
    {
        _whitePanel.color = new Color(1f, 1f, 1f, 0f);
        End();
    }

    public void End()
    {
        StartCoroutine(WhiteCo());
    }

    private IEnumerator WhiteCo()
    {
        float time = 0f;

        while (time < _duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, time / _duration);
            _whitePanel.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
    }

}
