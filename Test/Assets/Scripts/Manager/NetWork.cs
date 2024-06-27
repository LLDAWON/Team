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
    private PhotonView _pv;

    private void Awake()
    {
        Instance = this;

        Screen.SetResolution(1920,1080, false);
        PhotonNetwork.ConnectUsingSettings();
        _pv = GetComponent<PhotonView>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("���������Ӽ���");
    }
        
        //=> PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4}, null);

    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    
    public override void OnJoinedRoom()
    {
        string _masterNickName;
        _masterNickName = LobbyScripts.instance._nickName.text;
        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        Debug.Log("����������"+ _masterNickName);

        _pv.RPC("NickNameView", RpcTarget.All);
    }

    [PunRPC]
    private void NickNameView()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            string name = PhotonNetwork.PlayerList[i].NickName;
            LobbyScripts.instance._players.gameObject.SetActive(true);
            LobbyScripts.instance._players.text = name + "�����ϼ̽��ϴ�.";
        }
    }


    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("����������");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("����������");
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
        Debug.Log("���������" + _masterNickName);
    }


}
