using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWork : MonoBehaviourPunCallbacks
{
    public static NetWork Instance;
    public GameObject player;


    private void Awake()
    {
        Instance = this;

        Screen.SetResolution(1920,1080, false);
        PhotonNetwork.ConnectUsingSettings();

        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4}, null);

    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    public override void OnJoinedRoom()
    {
        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }
}
