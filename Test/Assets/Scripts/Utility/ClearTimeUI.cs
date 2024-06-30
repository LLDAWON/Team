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
        _clearTime.text = "Clear Time : " + NetWork.Instance.totalTime; // 클리어 시간을 UI에 표시
        DisplayClearTimes(); // 모든 플레이어의 클리어 시간을 표시하는 함수 호출
    }

    // 모든 플레이어의 클리어 시간을 표시하는 함수
    private void DisplayClearTimes()
    {
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.CurrentRoom.CustomProperties; // 현재 방의 커스텀 속성 가져오기
        List<string> playerTimes = new List<string>(); // 플레이어들의 클리어 시간을 저장할 리스트

        foreach (var entry in customProperties)
        {
            string playerName = entry.Key.ToString(); // 플레이어 이름 가져오기

            // "StartTime" 항목은 건너뛰기
            if (playerName == "StartTime") continue;

            double clearTimeInSeconds = (double)entry.Value; // 클리어 시간(초) 가져오기
            TimeSpan timeSpan = TimeSpan.FromSeconds(clearTimeInSeconds); // TimeSpan 객체로 변환

            // 포맷팅된 시간 문자열 생성 (00:00:00)
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds);

            // 플레이어 이름과 클리어 시간을 리스트에 추가
            playerTimes.Add($"{playerName} Clear Time : {formattedTime}");
        }

        // UI에 모든 플레이어의 클리어 시간을 줄바꿈으로 구분하여 표시
        _clearTime.text = string.Join("\n", playerTimes);
    }
}
