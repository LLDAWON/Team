using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyScripts : MonoBehaviourPunCallbacks
{

    public GameObject _LobbyPanel;
    public Button _createRoomBtn;
    public Button _joinRoomBtn;
    public TextMeshProUGUI _nickName;
    public Button _logInButton;

    public TextMeshProUGUI _roomName;
    public TextMeshProUGUI _players;

    public static LobbyScripts instance;

    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {

        Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings();
    }

    //방생성

    public void OnClickCreateBtn()
    {
        PhotonNetwork.CreateRoom(_nickName.text, new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }
    public void OnClickJoinBtn()
    {
        _roomName.transform.parent.transform.parent.gameObject.SetActive(true);
    }

    public void Room()
    {
        if (_roomName.text.Length == 0) return;

        PhotonNetwork.JoinRoom(_roomName.text, null);

        print(_roomName.text);
    }


    public void OnCreateNickNameBtn()
    {
        //if (string.IsNullOrEmpty(_nickName.text))
        //    return;

        PhotonNetwork.NickName = _nickName.text;
        PhotonNetwork.JoinLobby();
    }




    //private void OnNickNameChange(string s)
    //{
    //    _logInButton.interactable = s.Length > 0; 

    //    if (string.IsNullOrEmpty(_nickName.text)) 
    //        return;
    //    PhotonNetwork.NickName = _nickName.text;
    //}

    //public override void OnJoinedLobby()
    //{

    //    base.OnJoinedLobby();

    //    PhotonNetwork.LoadLevel("Lobby");

    //    Debug.Log("로비진입성공");

    //}

    //public void OnClickConnect()
    //{
    //    // 마스터 서버 접속 요청
    //    PhotonNetwork.ConnectUsingSettings();

    //}

    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //    PhotonNetwork.CreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    //}



}
