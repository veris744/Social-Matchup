using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance = null;

    public Dictionary<string, RoomInfo> RoomInfoList { get; set; }

    public string Task;
    public string Location;
    public string ImageType;
    public string theme;
    public int NumberOfImages;
    public byte NumberOfPlayers;
    public bool AudioChat;
    public bool pvp;

    private int order;
    private GameObject gameManager;
    private GameObject photonVoiceManager;
    private TypedLobby lobby;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    
    void Start()
    {
        this.pvp = false;       //no pvp yet

        DontDestroyOnLoad(this.gameObject);
        RoomInfoList = new Dictionary<string, RoomInfo>();
        PhotonNetwork.AutomaticallySyncScene = true;
        photonVoiceManager = GameObject.Find("PhotonVoiceManager");
        Connect();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the server");
        //helper = null;
        lobby = new TypedLobby("MyLobby", LobbyType.Default);
        PhotonNetwork.JoinLobby(lobby);
    }
    /*
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            Debug.Log("CurrentLobby: " + PhotonNetwork.CurrentLobby);
            Debug.Log("Num of players: " + PhotonNetwork.CountOfPlayers);
            Debug.Log("Num of players in lobby: " + PhotonNetwork.CountOfPlayersOnMaster);
            Debug.Log("Num of players in rooms: " + PhotonNetwork.CountOfPlayersInRooms);
            Debug.Log("Num of rooms: " + PhotonNetwork.CountOfRooms);
        } 
    }*/

    private void Connect()
    {
        PhotonNetwork.NetworkingClient.EnableLobbyStatistics = true;
        try
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Trying to connnect...");
        }
        catch (System.Net.Sockets.SocketException e) { Debug.LogError("Connection failed!"); }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnection cause: "+ cause);
        
        //failed to reach photon server
        if (cause == DisconnectCause.Exception || cause == DisconnectCause.ExceptionOnConnect)
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
                SceneManager.LoadScene("MainMenu");
            Debug.Log("Connection failed!");
        }
        
        //disconnection happens after connection setup 
        else
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
                SceneManager.LoadScene("MainMenu");
            Debug.Log("Disconnected from server!");
        }
        
        Connect();
        Debug.Log("Trying to connnnect...");
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName, roomOptions, lobby);
    }

    public void JoinRoom(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnJoinedRoom()
    {
        order = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log("PlayerCount = " + order);
        this.gameObject.AddComponent<PhotonView>();
        gameObject.GetPhotonView().ViewID = PhotonNetwork.CurrentRoom.GetHashCode();
        StartCoroutine(WaitingForOtherPlayer());
        /*OnPhotonPlayerConnected(PhotonNetwork.LocalPlayer);*/
    }

    public override void OnLeftRoom()
    {
        Destroy(GetComponent<PhotonView>());
    }

    private IEnumerator WaitingForOtherPlayer()
    {
        while (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            yield return new WaitForEndOfFrame();
        }
        OnPhotonPlayerConnected(PhotonNetwork.LocalPlayer);
    }

    void OnPhotonPlayerConnected(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("MaxPLayers = " + PhotonNetwork.CurrentRoom.MaxPlayers);
        int nPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log(nPlayers);
        if (nPlayers == PhotonNetwork.CurrentRoom.MaxPlayers) //if all the players are connected
        {
            //no helper no PVP
            switch (order)
            {
                case 1:
                    PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                    gameObject.GetPhotonView().RPC("SetGameParameters", RpcTarget.Others, Task, Location, NumberOfImages, AudioChat, pvp);
                    Debug.Log(Task + "Gameplay" + Location);
                    PhotonNetwork.LoadLevel(Task + "Gameplay" + Location);
                    StartCoroutine(StartGameAndInstantiateGameManager(pvp));
                    break;
                case 2:
                    StartCoroutine(StartGameAsPlayer(0));
                    break;
            }

        }
    }

    IEnumerator StartGameAsPlayer(int playerNumber)
    {
        GameObject player;
        yield return new WaitForSeconds(5f);

        if (playerNumber == 0)
        {
            player = PhotonNetwork.Instantiate("Player", new Vector3(0, 3, -4), Quaternion.identity, 0);
        }
        else if (playerNumber == 1)
        {
            player = PhotonNetwork.Instantiate("Player", new Vector3(17, 3, 4), Quaternion.identity, 0);
        }
        else
        {
            player = PhotonNetwork.Instantiate("Player", new Vector3(17, 3, -4), Quaternion.identity, 0);
        }


        //enabling audio listener 
        player.GetComponent<AudioListener>().enabled = true;

        //audioChat disabled
        //photonVoiceManager.GetComponent<Recorder>().IsRecording = false;

    }

    IEnumerator StartGameAndInstantiateGameManager(bool pvp) //crea un GameManager al primo giocatore e una sua View in tutti i mondi
    {
        yield return new WaitForSeconds(5f);
        GameObject player;

        player = PhotonNetwork.Instantiate("Player", new Vector3(0, 3, 4), new Quaternion(0, 1, 0, 0), 0);
        
        //enabling audio listener 
        player.GetComponent<AudioListener>().enabled = true;

        //no audioChat
        //photonVoiceManager.GetComponent<Recorder>().IsRecording = false;

        Debug.Log("Task: " + Task);
        gameManager = PhotonNetwork.Instantiate("Managers/ClassicGameManager", Vector3.zero, Quaternion.identity, 0);
        //gameManager.GetComponent<GameManager>().SetPVP(pvp);  PVP always false for now
    }

    private void EnableAudioChat(GameObject player)
    {
        player.GetComponent<Speaker>().enabled = true;
        player.GetComponent<PhotonVoiceView>().enabled = true;
        photonVoiceManager.GetComponent<Recorder>().IsRecording = true;
    }

    [PunRPC]
    public void SetGameParameters(string task, string location, int numberOfElements, bool audioChat, bool pvp)
    {
        this.Task = task;
        this.Location = location;
        this.NumberOfImages = numberOfElements;
        //this.AudioChat = audioChat;
        this.AudioChat = false;
        //this.pvp = pvp;
        this.pvp = false;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Updating rooms");
        foreach (var info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (RoomInfoList.Keys.Contains(info.Name))
                {
                    RoomInfoList.Remove(info.Name);
                }
                continue;
            }

            if (RoomInfoList.Keys.Contains(info.Name)) RoomInfoList[info.Name] = info;
            else RoomInfoList.Add(info.Name, info);
        }
    }

    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("players in lobby: " + lobbyStatistics.Count);
    }

    public Dictionary<string, RoomInfo> GetRoomList()
    {
        int i = 0;
        foreach (var room in RoomInfoList.Keys)
        {
            Debug.Log("RoomInfo[" + i + "]: " + room);
            i++;
        }
        return RoomInfoList;
    }

}
