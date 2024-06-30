using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetWork : MonoBehaviourPunCallbacks
{
    public static NetWork Instance; // 싱글톤 인스턴스
    public GameObject player; // 플레이어 오브젝트
    private PhotonView _pv; // PhotonView 컴포넌트

    // 플레이 시간 계산
    private double _startTime; // 시작 시간
    private bool _isTiming = false; // 타이밍 시작 여부
    public double totalTime; // 총 플레이 시간

    private void Awake()
    {
        Instance = this; // 싱글톤 인스턴스 설정

        Screen.SetResolution(1920, 1080, false); // 화면 해상도 설정
        PhotonNetwork.ConnectUsingSettings(); // Photon 서버 연결
        _pv = GetComponent<PhotonView>(); // PhotonView 컴포넌트 가져오기
        DontDestroyOnLoad(gameObject); // 씬 로드 시 오브젝트 파괴 방지
    }

    // 타이밍 시작
    public void StartTiming()
    {
        if (PhotonNetwork.IsMasterClient) // 마스터 클라이언트인 경우
        {
            if (SceneManager.GetActiveScene().name == "5FloorScene") // 현재 씬이 "5FloorScene"인 경우
            {
                _startTime = PhotonNetwork.Time; // 시작 시간 설정
                ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "StartTime", _startTime } }; // 커스텀 속성 설정
                PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties); // 커스텀 속성 저장                
                _isTiming = true; // 타이밍 시작
            }
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartTime")) // 커스텀 속성에 "StartTime"이 있는 경우
            {
                _startTime = (double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"]; // 시작 시간 가져오기
                _isTiming = true; // 타이밍 시작
            }
        }
    }

    void Update()
    {
        if (_isTiming) // 타이밍이 시작된 경우
        {
            double elapsedTime = PhotonNetwork.Time - _startTime; // 경과 시간 계산
            Debug.Log("Elapsed Time: " + elapsedTime); // 경과 시간 로그 출력
        }
    }

    // 엔딩 씬에 진입했을 때 호출
    public void OnEndingSceneEntered()
    {
        if (_isTiming)
        {
            _isTiming = false; // 타이밍 종료
            totalTime = PhotonNetwork.Time - _startTime; // 총 플레이 시간 계산
            Debug.Log("Total Time to reach Ending Scene: " + totalTime); // 총 플레이 시간 로그 출력

            string playerName = PhotonNetwork.NickName; // 플레이어 닉네임 가져오기
            ExitGames.Client.Photon.Hashtable playerTime = new ExitGames.Client.Photon.Hashtable { { playerName, totalTime } }; // 클리어 타임을 커스텀 속성에 저장
            PhotonNetwork.CurrentRoom.SetCustomProperties(playerTime); // 커스텀 속성 저장
            Destroy(player); // 플레이어 오브젝트 파괴
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("마스터접속성공"); // 마스터 서버 접속 성공 로그 출력
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name); // 서버 접속 로그 출력
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("방참가성공"); // 로비 참가 성공 로그 출력
    }
    public override void OnJoinedRoom()
    {
        //이부분을 방 목록창(로비)에서 방을 클릭했을때로 변경해야함
        string _masterNickName;
        _masterNickName = LobbyScripts.instance._nickName.text; // 로비에서 입력한 닉네임 가져오기
        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity); // 플레이어 오브젝트 생성
        Debug.Log("방참가성공" + _masterNickName); // 방 참가 성공 로그 출력

        _pv.RPC("NickNameView", RpcTarget.All); // 모든 클라이언트에 닉네임 표시 RPC 호출
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("방참가실패"); // 방 참가 실패 로그 출력
    }

    [PunRPC]
    private void NickNameView()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) // 방에 있는 모든 플레이어의 닉네임 표시
        {
            string name = PhotonNetwork.PlayerList[i].NickName;
            LobbyScripts.instance._players.gameObject.SetActive(true);
            LobbyScripts.instance._players.text = name + "님이 입장하셨습니다.";
        }
    }



    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings(); // 서버 재연결
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        string _masterNickName;
        _masterNickName = LobbyScripts.instance._nickName.text; // 로비에서 입력한 닉네임 가져오기

        Debug.Log("방 생성 성공" + _masterNickName); // 방 생성 성공 로그 출력

        LobbyScripts.instance._nickName.gameObject.SetActive(false); // 닉네임 입력 필드 비활성화
        LobbyScripts.instance._createRoomBtn.gameObject.SetActive(false); // 방 생성 버튼 비활성화
        LobbyScripts.instance._joinRoomBtn.gameObject.SetActive(false); // 방 참가 버튼 비활성화
    }
}
