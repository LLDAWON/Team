using Firebase.Database;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearTimeUI : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI _clearTime; // UI�� ǥ���� Ŭ���� �ð��� ��Ÿ���� TextMeshProUGUI ������Ʈ

    private void Awake()
    {
        _clearTime = GetComponent<TextMeshProUGUI>(); // TextMeshProUGUI ������Ʈ ��������
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false); // �ʱ⿡�� UI�� ��Ȱ��ȭ ���·� ����
    }

    // Ŭ���� �ð��� �����ͼ� UI�� ǥ���ϴ� �Լ�
    public void GetClearTime()
    {
        gameObject.SetActive(true); // UI Ȱ��ȭ
        DisplayClearTimes(); // ��� �÷��̾��� Ŭ���� �ð��� ǥ���ϴ� �Լ� ȣ��
    }

    private void OnTopClearTimesLoaded(List<ClearTimeEntry> topClearTimes)
    {
        // ���� 5���� Ŭ���� �ð��� ������� ���
        string clearTimesText = "";
        for (int i = 0; i < Mathf.Min(topClearTimes.Count, 5); i++)
        {
            string playerName = topClearTimes[i].PlayerName;
            double clearTime = topClearTimes[i].ClearTime;
            string formattedTime = FormatTime(clearTime);
            clearTimesText += $"{playerName} - Clear Time: {formattedTime}\n";
        }
        _clearTime.text = clearTimesText;
    }

    // Ŭ���� �ð��� ��:��:�� �������� �������ϴ� �޼���
    private string FormatTime(double totalTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalTime); // TimeSpan ��ü�� ��ȯ
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Hours,
            timeSpan.Minutes,
            timeSpan.Seconds);
    }

    // ��� �÷��̾��� Ŭ���� �ð��� ǥ���ϴ� �Լ�
    private void DisplayClearTimes()
    {
        FirebaseManager.instance.LoadTopClearTimes(5, OnTopClearTimesLoaded);
    }
}

