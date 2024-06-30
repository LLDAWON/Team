using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyScripts : MonoBehaviourPunCallbacks
{
    public GameObject _LobbyPanel;
    public Button _createRoomBtn;
    public Button _joinRoomBtn;
    public TMP_InputField _nickName;
    public Button _logInButton;
    public TMP_InputField _roomName;
    public TextMeshProUGUI _players;

    public static LobbyScripts instance;



    [SerializeField] GameObject roomLists;
    Dictionary<string, RoomInfo> dicRoomInfo = new Dictionary<string, RoomInfo>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _createRoomBtn.interactable = false;
        _joinRoomBtn.interactable = false;

    }

    // Enable/disable buttons based on nickname input
    public void OnNickNameChanged()
    {
        bool isNickNameEntered = !string.IsNullOrEmpty(_nickName.text);
        _createRoomBtn.gameObject.SetActive(isNickNameEntered);
        _joinRoomBtn.gameObject.SetActive(isNickNameEntered);
        _nickName.gameObject.SetActive(!isNickNameEntered);
        _roomName.gameObject.SetActive(!isNickNameEntered);
        _createRoomBtn.interactable = isNickNameEntered;
        _joinRoomBtn.interactable = isNickNameEntered;
        _logInButton.interactable = isNickNameEntered;
    }

    // ¹æ»ý¼º
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
        _roomName.gameObject.SetActive(true);
    }

    public void Room()
    {
        if (_roomName.text.Length == 0) return;
        PhotonNetwork.JoinRoom(_roomName.text, null);
        print(_roomName.text);
    }

    public void OnCreateNickNameBtn()
    {
        PhotonNetwork.NickName = _nickName.text;
        //PhotonNetwork.JoinLobby();
    }

    public void PlayGameBtn()
    {
        SceneManager.LoadScene("5FloorScene");
    }

    public void JoinLobbyBtn()
    {
        PhotonNetwork.JoinLobby();
    }
    
}
