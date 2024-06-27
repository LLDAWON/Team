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

    //�����

    public void OnClickCreateBtn()
    {
        PhotonNetwork.CreateRoom(_nickName.text, new RoomOptions { MaxPlayers = 4 }, null);
        
        Debug.Log("���������");
    }

    public void OnClickJoinBtn()
    {
        

        for( int i = 0; i<PhotonNetwork.PlayerList.Length; i++ )
        {
            string name = PhotonNetwork.PlayerList[i].NickName;
            print(name);
        }


        _roomName.gameObject.SetActive(true);


        // ���̸� �Է��ϴ� �г� �߰� �Է¹�ư ������ �ؿ� �Լ� �ߵ�
        // PhotonNetwork.JoinRoom(_roomName.text,null);
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

    //    Debug.Log("�κ����Լ���");

    //}

    //public void OnClickConnect()
    //{
    //    // ������ ���� ���� ��û
    //    PhotonNetwork.ConnectUsingSettings();

    //}

    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //    PhotonNetwork.CreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    //}



}
