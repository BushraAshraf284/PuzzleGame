
    using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info
    public static PhotonRoom room;
    private PhotonView PV;
    public bool isGameLoaded;
    public int currentScene;

    [Header("HUDSettings")]
    public TMP_Text timer;
    public TMP_Text currentPlayer;
    public TMP_Text MaxPlayer;


    // Player info
    [Header("PlayersInfo")]
    Player[] photonPlayers;

    public int playersInRoom;
    public int myNumberInRoom;
    public int playersInGame;

    private bool readyToCount;
    private bool readyToStart;
    public float startingTime; 
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;



    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                UnityEngine.Object.Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
       
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
    }


    void Update()
    {
        if(MultiplayerSettings.ms.delayStart)
        {
            if(playersInRoom==1)
            {
                RestartTimer();
            }
            if(!isGameLoaded)
            {
                if(readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                }
                else if(readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                timer.text = timeToStart.ToString();
                if(timeToStart<=0)
                {
                    StartGame();
                }
            }
        }
    }
    public override void OnEnable()
    {
        //subscribe to functions
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;

    }
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("now Inside a room");
       

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();
        if(MultiplayerSettings.ms.delayStart)
        {
            Debug.Log(playersInRoom + "/" + MultiplayerSettings.ms.maxPlayers);

            currentPlayer.text = playersInRoom.ToString() + ":";
            MaxPlayer.text = MultiplayerSettings.ms.maxPlayers.ToString();

            if (playersInRoom > 1)
                readyToCount = true;
            if(playersInRoom == MultiplayerSettings.ms.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }


    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Player has joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if (playersInRoom > 1)
            readyToCount = true;
        if (playersInRoom == MultiplayerSettings.ms.maxPlayers)
        {
            readyToStart = true;
            if (!PhotonNetwork.IsMasterClient)
                return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log("Loading Level");
        if(MultiplayerSettings.ms.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSettings.ms.GameScene);
    }

    void RestartTimer()
    {
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettings.ms.GameScene)
        {
            isGameLoaded = true;
            if(MultiplayerSettings.ms.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
                Debug.Log("I'm here ");
            }
            else
                RPC_CreatePlayer();
        }

    }
    [PunRPC]
    public void RPC_CreatePlayer()
    {
        Debug.Log("I'm creating a player!");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerObject"), transform.position, transform.rotation, 0);

    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        Debug.Log("Players In Room:"+ playersInGame);
        Debug.Log("Players in photon"+ PhotonNetwork.PlayerList.Length);
        if(playersInGame == PhotonNetwork.PlayerList.Length)
        {
            Debug.Log("Im in loaded scene");
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

}

