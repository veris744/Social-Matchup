using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

//used in the JoinGame Scene. Simply creates a JoinRoomButton for each open room available, and refreshes every 2 seconds.
public class RoomDisplayer : MonoBehaviour
{
    private PhotonManager photonManager;
    private float timer;
    private Dictionary<string,RoomInfo> roomInfos;

    private void Start()
    {
        photonManager = PhotonManager.instance;
        timer = 0;
        roomInfos = photonManager.GetRoomList();
        DisplayRooms();
    }

    //Refresh every 2 seconds
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 2f)
        {
            roomInfos = photonManager.GetRoomList();
            Debug.Log("RoomInfos.Length = " + roomInfos.Count);
            DisplayRooms();
            timer = 0;
        }
    }

    //Gets info about rooms in the PhotonManager and creates a JoinRoomButton prefab for each one of them
    private void DisplayRooms()
    {
        foreach (GameObject roomButton in GameObject.FindGameObjectsWithTag("JoinRoomButton"))
            Destroy(roomButton);
        
        int i = 0;
        foreach (string roomInfoName in roomInfos.Keys)
        {
            GameObject joinRoomButton = Instantiate(Resources.Load<GameObject>("JoinRoomButton"), GameObject.Find("Canvas").transform);
            joinRoomButton.GetComponent<RectTransform>().localPosition = new Vector3(0, i, 0);
            joinRoomButton.transform.Find("Text").GetComponent<Text>().text = roomInfos[roomInfoName].Name;
            i -= 80;
        }

        /*StartCoroutine(WaitingForRoomInfo());*/
    }

    private IEnumerator WaitingForRoomInfo()
    {
        while (roomInfos == null)
        {
            roomInfos = photonManager.GetRoomList();
            Debug.Log("RoomInfos = " + roomInfos);
            yield return new WaitForEndOfFrame();
        }
        
        Debug.Log("RoomInfos = " + roomInfos);

        int i = 0;
        foreach (string roomInfoName in roomInfos.Keys)
        {
            GameObject joinRoomButton = Instantiate(Resources.Load<GameObject>("JoinRoomButton"), GameObject.Find("Canvas").transform);
            joinRoomButton.GetComponent<RectTransform>().localPosition = new Vector3(0, i, 0);
            joinRoomButton.transform.Find("Text").GetComponent<Text>().text = roomInfos[roomInfoName].Name;
            i -= 80;
        }
    }
}
