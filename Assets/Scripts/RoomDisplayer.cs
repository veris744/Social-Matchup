using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomDisplayer : MonoBehaviour
{
    private PhotonManager photonManager;
    private float timer;
    private Dictionary<string, RoomInfo> roomInfos;

    // Start is called before the first frame update
    void Start()
    {
        photonManager = PhotonManager.instance;
        timer = 0;
        roomInfos = photonManager.GetRoomList();
        DisplayRooms();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2f)
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
