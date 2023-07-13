using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLobby: MonoBehaviourPunCallbacks
{
    public GameObject JoinRoom;
    public GameObject CancelButton;

    public static PhotonLobby inst;

    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connects to Photon Master Server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon has connected to photon master server.");
        PhotonNetwork.AutomaticallySyncScene = true;
        JoinRoom.SetActive(true);
    }

    public void onJoinRoomClicked()
    {
        Debug.Log("Joining Room...");
        JoinRoom.SetActive(false);
        CancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();      
            
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join room but failed. Trying to Create Room");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room already exists. Creating another Room.");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating Room...");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.ms.maxPlayers};
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

   
    public void OnCancelButtonClick()
    {
        JoinRoom.SetActive(true);
        CancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

   /* public void OnPlayerSelect(int PlayerSelect)
    {
        if(PlayerInfo.PI !=null)
        {
            PlayerInfo.PI.mySelectedPlayer = PlayerSelect;
            PlayerPrefs.SetInt("MyPlayer", PlayerSelect);
        }
    }*/


}
