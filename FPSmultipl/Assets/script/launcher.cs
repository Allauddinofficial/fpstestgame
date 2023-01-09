using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
public class launcher : MonoBehaviourPunCallbacks
{
    public static launcher instance;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;  
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;  
    void Awake()
    {
        instance = this;    
        
    }
    public void Start()
    {
        Debug.Log("conneting to master");
        PhotonNetwork.ConnectUsingSettings();
       
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("conneted to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined lobby");
        //PhotonNetwork.NickName = "palyer" + Random.Range(0, 1000).ToString("0000"); 
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
        PlayerPrefs.SetString("RoomName", roomNameInputField.text);
    }

    public override void OnJoinedRoom()
    {

        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = roomNameInputField.text;
        Player[] playerss = PhotonNetwork.PlayerList;
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < playerss.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItems>().SetUp(playerss[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient); 
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
       
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failde :" + message;
        MenuManager.Instance.OpenMenu("error");
    }
    public void Leaveroom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void Join_Room(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUP(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItems>().SetUp(newPlayer);
    }
   
}
