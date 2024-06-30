using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetWork : MonoBehaviourPunCallbacks
{
    public static NetWork Instance; // �̱��� �ν��Ͻ�
    public GameObject player; // �÷��̾� ������Ʈ
    private PhotonView _pv; // PhotonView ������Ʈ

    // �÷��� �ð� ���
    private double _startTime; // ���� �ð�
    private bool _isTiming = false; // Ÿ�̹� ���� ����
    public double totalTime; // �� �÷��� �ð�

    private void Awake()
    {
        Instance = this; // �̱��� �ν��Ͻ� ����

        Screen.SetResolution(1920, 1080, false); // ȭ�� �ػ� ����
        PhotonNetwork.ConnectUsingSettings(); // Photon ���� ����
        _pv = GetComponent<PhotonView>(); // PhotonView ������Ʈ ��������
        DontDestroyOnLoad(gameObject); // �� �ε� �� ������Ʈ �ı� ����
    }

    // Ÿ�̹� ����
    public void StartTiming()
    {
        if (PhotonNetwork.IsMasterClient) // ������ Ŭ���̾�Ʈ�� ���
        {
            if (SceneManager.GetActiveScene().name == "5FloorScene") // ���� ���� "5FloorScene"�� ���
            {
                _startTime = PhotonNetwork.Time; // ���� �ð� ����
                ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "StartTime", _startTime } }; // Ŀ���� �Ӽ� ����
                PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties); // Ŀ���� �Ӽ� ����                
                _isTiming = true; // Ÿ�̹� ����
            }
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartTime")) // Ŀ���� �Ӽ��� "StartTime"�� �ִ� ���
            {
                _startTime = (double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"]; // ���� �ð� ��������
                _isTiming = true; // Ÿ�̹� ����
            }
        }
    }

    void Update()
    {
        if (_isTiming) // Ÿ�̹��� ���۵� ���
        {
            double elapsedTime = PhotonNetwork.Time - _startTime; // ��� �ð� ���
            Debug.Log("Elapsed Time: " + elapsedTime); // ��� �ð� �α� ���
        }
    }

    // ���� ���� �������� �� ȣ��
    public void OnEndingSceneEntered()
    {
        if (_isTiming)
        {
            _isTiming = false; // Ÿ�̹� ����
            totalTime = PhotonNetwork.Time - _startTime; // �� �÷��� �ð� ���
            Debug.Log("Total Time to reach Ending Scene: " + totalTime); // �� �÷��� �ð� �α� ���

            string playerName = PhotonNetwork.NickName; // �÷��̾� �г��� ��������
            ExitGames.Client.Photon.Hashtable playerTime = new ExitGames.Client.Photon.Hashtable { { playerName, totalTime } }; // Ŭ���� Ÿ���� Ŀ���� �Ӽ��� ����
            PhotonNetwork.CurrentRoom.SetCustomProperties(playerTime); // Ŀ���� �Ӽ� ����
            Destroy(player); // �÷��̾� ������Ʈ �ı�
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("���������Ӽ���"); // ������ ���� ���� ���� �α� ���
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name); // ���� ���� �α� ���
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("����������"); // �κ� ���� ���� �α� ���
    }
    public override void OnJoinedRoom()
    {
        //�̺κ��� �� ���â(�κ�)���� ���� Ŭ���������� �����ؾ���
        string _masterNickName;
        _masterNickName = LobbyScripts.instance._nickName.text; // �κ񿡼� �Է��� �г��� ��������
        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity); // �÷��̾� ������Ʈ ����
        Debug.Log("����������" + _masterNickName); // �� ���� ���� �α� ���

        _pv.RPC("NickNameView", RpcTarget.All); // ��� Ŭ���̾�Ʈ�� �г��� ǥ�� RPC ȣ��
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("����������"); // �� ���� ���� �α� ���
    }

    [PunRPC]
    private void NickNameView()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) // �濡 �ִ� ��� �÷��̾��� �г��� ǥ��
        {
            string name = PhotonNetwork.PlayerList[i].NickName;
            LobbyScripts.instance._players.gameObject.SetActive(true);
            LobbyScripts.instance._players.text = name + "���� �����ϼ̽��ϴ�.";
        }
    }



    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings(); // ���� �翬��
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        string _masterNickName;
        _masterNickName = LobbyScripts.instance._nickName.text; // �κ񿡼� �Է��� �г��� ��������

        Debug.Log("�� ���� ����" + _masterNickName); // �� ���� ���� �α� ���

        LobbyScripts.instance._nickName.gameObject.SetActive(false); // �г��� �Է� �ʵ� ��Ȱ��ȭ
        LobbyScripts.instance._createRoomBtn.gameObject.SetActive(false); // �� ���� ��ư ��Ȱ��ȭ
        LobbyScripts.instance._joinRoomBtn.gameObject.SetActive(false); // �� ���� ��ư ��Ȱ��ȭ
    }
}
