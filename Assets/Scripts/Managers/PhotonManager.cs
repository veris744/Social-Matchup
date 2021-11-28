using Photon.Pun;
using Photon.Realtime;
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
}
