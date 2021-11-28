using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviour
{
    public static PhotonManager instance = null;

    public Dictionary<string, RoomInfo> RoomInfoList { get; set; }

    public string Task;
    public string Location;
    public string ImageType;
    public string theme;
    public int NumberOfImages;
    public byte NumberOfPlayers;
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

    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = NumberOfPlayers;

        PhotonNetwork.CreateRoom(roomName, roomOptions, lobby);
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
                    gameObject.GetPhotonView().RPC("SetGameParameters", RpcTarget.Others, Task, Location, NumberOfImages);
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
        photonVoiceManager.GetComponent<Recorder>().IsRecording = false;

    }

    IEnumerator StartGameAndInstantiateGameManager(bool pvp) //crea un GameManager al primo giocatore e una sua View in tutti i mondi
    {
        yield return new WaitForSeconds(5f);
        GameObject player;

        player = PhotonNetwork.Instantiate("Player", new Vector3(0, 3, 4), new Quaternion(0, 1, 0, 0), 0);
        
        //enabling audio listener 
        player.GetComponent<AudioListener>().enabled = true;

        //no audioChat
        photonVoiceManager.GetComponent<Recorder>().IsRecording = false;

        Debug.Log("Task: " + Task);
        gameManager = PhotonNetwork.Instantiate("Managers/" + Task + "GameManager", Vector3.zero, Quaternion.identity, 0);
        //gameManager.GetComponent<GameManager>().SetPVP(pvp);  PVP always false for now
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
