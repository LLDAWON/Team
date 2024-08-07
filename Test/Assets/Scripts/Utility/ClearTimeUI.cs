using Firebase.Database;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearTimeUI : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI _clearTime; // UI에 표시할 클리어 시간을 나타내는 TextMeshProUGUI 컴포넌트

    private void Awake()
    {
        _clearTime = GetComponent<TextMeshProUGUI>(); // TextMeshProUGUI 컴포넌트 가져오기
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false); // 초기에는 UI를 비활성화 상태로 설정
    }

    // 클리어 시간을 가져와서 UI에 표시하는 함수
    public void GetClearTime()
    {
        gameObject.SetActive(true); // UI 활성화
        DisplayClearTimes(); // 모든 플레이어의 클리어 시간을 표시하는 함수 호출
    }

    private void OnTopClearTimesLoaded(List<ClearTimeEntry> topClearTimes)
    {
        // 상위 5명의 클리어 시간을 순서대로 출력
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

    // 클리어 시간을 시:분:초 형식으로 포맷팅하는 메서드
    private string FormatTime(double totalTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalTime); // TimeSpan 객체로 변환
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Hours,
            timeSpan.Minutes,
            timeSpan.Seconds);
    }

    // 모든 플레이어의 클리어 시간을 표시하는 함수
    private void DisplayClearTimes()
    {
        FirebaseManager.instance.LoadTopClearTimes(5, OnTopClearTimesLoaded);
    }
}

