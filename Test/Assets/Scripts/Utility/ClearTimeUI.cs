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
        _clearTime.text = "Clear Time : " + NetWork.Instance.totalTime; // Ŭ���� �ð��� UI�� ǥ��
        DisplayClearTimes(); // ��� �÷��̾��� Ŭ���� �ð��� ǥ���ϴ� �Լ� ȣ��
    }

    // ��� �÷��̾��� Ŭ���� �ð��� ǥ���ϴ� �Լ�
    private void DisplayClearTimes()
    {
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.CurrentRoom.CustomProperties; // ���� ���� Ŀ���� �Ӽ� ��������
        List<string> playerTimes = new List<string>(); // �÷��̾���� Ŭ���� �ð��� ������ ����Ʈ

        foreach (var entry in customProperties)
        {
            string playerName = entry.Key.ToString(); // �÷��̾� �̸� ��������

            // "StartTime" �׸��� �ǳʶٱ�
            if (playerName == "StartTime") continue;

            double clearTimeInSeconds = (double)entry.Value; // Ŭ���� �ð�(��) ��������
            TimeSpan timeSpan = TimeSpan.FromSeconds(clearTimeInSeconds); // TimeSpan ��ü�� ��ȯ

            // �����õ� �ð� ���ڿ� ���� (00:00:00)
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds);

            // �÷��̾� �̸��� Ŭ���� �ð��� ����Ʈ�� �߰�
            playerTimes.Add($"{playerName} Clear Time : {formattedTime}");
        }

        // UI�� ��� �÷��̾��� Ŭ���� �ð��� �ٹٲ����� �����Ͽ� ǥ��
        _clearTime.text = string.Join("\n", playerTimes);
    }
}
