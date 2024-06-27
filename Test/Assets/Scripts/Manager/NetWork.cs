using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetWork : MonoBehaviourPunCallbacks
{
    public static NetWork Instance;
    public GameObject player;
    private List<string>_nickName = new List<string>();


    private void Awake()
    {
        Instance = this;

        Screen.SetResolution(1920,1080, false);
        PhotonNetwork.ConnectUsingSettings();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("마스터접속성공");
    }
        
        //=> PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4}, null);

    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    
    public override void OnJoinedRoom()
    {
        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        Debug.Log("방참가성공");
    }

    
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("방참가성공");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("방참가실패");
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnCreatedRoom()
    {

        base.OnCreatedRoom();

        string _masterNickName;
        _masterNickName = LobbyScripts.instance._nickName.text;

        PhotonNetwork.CreateRoom(_masterNickName, new RoomOptions { MaxPlayers = 4 }, null);

    }


}
