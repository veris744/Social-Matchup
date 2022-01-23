using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class ClassicGameManager : GameManager
{

    public struct EmojiStruct
    {
        public string gameObjectName; // Angry1(Clone)
        public string emojiName; // Angry1
        public string emoji; // Angry
        public int number; //1
    };

    public bool selected;
    

    public EmojiStruct selected1;
    public EmojiStruct selected2;

    public GameObject synchronization;

    Vector3 baseEmojiPosition1 = new Vector3(0, 3, 1.1f);
    Vector3 baseEmojiPosition2 = new Vector3(0, 3, -1.1f);
    Vector3[] positionsArray;
    PhotonView photonView;
    List<InputDevice> devices;

    int init = 0;

    void ResetSelected1()
    {
        selected1.emojiName = "none1";
        selected1.gameObjectName = "none1";
        selected1.emoji = "none1";
        selected1.number = 0;
    }
    private void ResetSelected2()
    {
        selected2.emojiName = "none2";
        selected2.gameObjectName = "none2";
        selected2.gameObjectName = "none2";
        selected2.number = 0;
    }

    EmojiStruct CreateEmojiStruct(string gameObject, string emjName, string emj, int n)
    {
        EmojiStruct res;
        res.gameObjectName = gameObject;
        res.emojiName = emjName;
        res.number = n;
        res.emoji = emj;
        return res;
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        ResetSelected1();
        ResetSelected2();
        positionsArray = new[] { new Vector3(0f, 0f, 0f), new Vector3(1.2f, 0f, 0f), new Vector3(-1.2f, 0f, 0f),
            new Vector3(0.6f, -1f, 0f), new Vector3(-0.6f, -1f, 0f), new Vector3(1.8f, -1f, 0f), new Vector3(-1.8f, -1f, 0f) };

        devices = new List<InputDevice>();
        InputDeviceCharacteristics controllersCharacteristics = InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(controllersCharacteristics, devices);
    }


        // Update is called once per frame
        void Update()
    {

        if (init == 0)
        {
            init++;
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnEmojis();
            }
        }

        bool gripButton = false;

        if (devices.Count == 2) {
            devices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton1);
            devices[1].TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton2);
            gripButton = gripButton1 || gripButton2;
        } else if (devices.Count == 4)
        {
            devices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton1);
            devices[1].TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton2);
            devices[2].TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton3);
            devices[3].TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton4);
            gripButton = gripButton1 || gripButton2 || gripButton3 || gripButton4;
        }

        if (Input.GetMouseButtonDown(0) || gripButton)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                EmojiStruct emjStruct = StringToEmojiStruct(hit.transform.gameObject.name);
                photonView.RPC("OnClick", RpcTarget.All, emjStruct.number, emjStruct.emoji, emjStruct.emojiName, emjStruct.gameObjectName);
            }
        }
        if (String.Equals(selected1.emoji, selected2.emoji))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(GameObject.Find(selected1.gameObjectName));
                PhotonNetwork.Destroy(GameObject.Find(selected2.gameObjectName).gameObject);
            }
            ResetSelected1();
            ResetSelected2();
        }

        if (GameObject.FindGameObjectsWithTag("Emoji").Length == 0)
        {
            victory();
        }
    }

    public static Vector3[] Shuffle(Vector3[] array)
    {
        System.Random rnd = new System.Random();
        for (int i = 0; i < array.Length; i++)
        {
            int k = rnd.Next(0, i);
            Vector3 value = array[k];
            array[k] = array[i];
            array[i] = value;
        }
        return array;
    }


    public void SpawnEmojis()
    {
        string[] prefabs1 = new string[] { "Angry1", "Crying1", "Embarassed1", "Laughing1", "Involve1", "Smiling1", "Smiling1", "Surprised1" };
        positionsArray = Shuffle(positionsArray);
        for (int i = 0; i < PhotonManager.instance.NumberOfImages; i++)
        {
            PhotonNetwork.Instantiate("Models/Prefab/" + prefabs1[i], baseEmojiPosition1 + positionsArray[i], Quaternion.identity, 0);
        }
        /*
        PhotonNetwork.Instantiate("Models/Prefab/Crying1", baseEmojiPosition1 + positionsArray[1], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Embarassed1", baseEmojiPosition1 + positionsArray[2], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Laughing1", baseEmojiPosition1 + positionsArray[3], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Involve1", baseEmojiPosition1 + positionsArray[4], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Smiling1", baseEmojiPosition1 + positionsArray[5], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Surprised1", baseEmojiPosition1 + positionsArray[6], Quaternion.identity, 0);
        */
        string[] prefabs2 = new string[] { "Angry2", "Crying2", "Embarassed2", "Laughing2", "Involve2", "Smiling2", "Smiling2", "Surprised2" };
        positionsArray = Shuffle(positionsArray);
        for (int i = 0; i < PhotonManager.instance.NumberOfImages; i++)
        {
            PhotonNetwork.Instantiate("Models/Prefab/" + prefabs2[i], baseEmojiPosition1 + positionsArray[i], Quaternion.identity, 0);
        }
        /*
        positionsArray = Shuffle(positionsArray);
        PhotonNetwork.Instantiate("Models/Prefab/Angry2", baseEmojiPosition2 + positionsArray[0], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Crying2", baseEmojiPosition2 + positionsArray[1], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Embarassed2", baseEmojiPosition2 + positionsArray[2], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Laughing2", baseEmojiPosition2 + positionsArray[3], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Involve2", baseEmojiPosition2 + positionsArray[4], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Smiling2", baseEmojiPosition2 + positionsArray[5], Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Surprised2", baseEmojiPosition2 + positionsArray[6], Quaternion.identity, 0);
        */
    }

    [PunRPC]
    void OnClick(int n, string emj, string name, string goName)
    {
        if (n == 1)
        {
            selected = true;
            if (String.Equals(selected1.emojiName, name))
            {
                ResetSelected1();
                GameObject.Find(goName).transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                if (!String.Equals(selected1.emojiName, "none1"))
                {
                    GameObject.Find(selected1.emojiName + "(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
                }
                selected1 = CreateEmojiStruct(goName, name, emj, n);
                GameObject.Find(goName).transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
        } else
        {
            selected = true;
            if (n == 2)
            {
                if (String.Equals(selected2.emojiName, name))
                {
                    ResetSelected2();
                    GameObject.Find(goName).transform.localScale = new Vector3(1f, 1f, 1f);

                }
                else
                {
                    if (!String.Equals(selected2.emojiName, "none2"))
                    {
                        GameObject.Find(selected2.emojiName + "(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    selected2 = CreateEmojiStruct(goName, name, emj, n);
                    GameObject.Find(goName).transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                }
            }
        }
    }

    void victory()
    {

    }


    EmojiStruct StringToEmojiStruct(string s)
    {
        EmojiStruct structEmoji;
        structEmoji.gameObjectName = s;
        structEmoji.emojiName = "none";
        structEmoji.emoji = "none";
        structEmoji.number = 0;
        switch (s)
        {
            case "Angry1(Clone)":
                structEmoji.emojiName = "Angry1";
                structEmoji.emoji = "Angry";
                structEmoji.number = 1;
                break;
            case "Angry2(Clone)":
                structEmoji.emojiName = "Angry2";
                structEmoji.emoji = "Angry";
                structEmoji.number = 2;
                break;
            case "Crying1(Clone)":
                structEmoji.emojiName = "Crying1";
                structEmoji.emoji = "Crying";
                structEmoji.number = 1;
                break;
            case "Crying2(Clone)":
                structEmoji.emojiName = "Crying2";
                structEmoji.emoji = "Crying";
                structEmoji.number = 2;
                break;
            case "Embarassed1(Clone)":
                structEmoji.emojiName = "Embarassed1";
                structEmoji.emoji = "Embarassed";
                structEmoji.number = 1;
                break;
            case "Embarassed2(Clone)":
                structEmoji.emojiName = "Embarassed2";
                structEmoji.emoji = "Embarassed";
                structEmoji.number = 2;
                break;
            case "Laughing1(Clone)":
                structEmoji.emojiName = "Laughing1";
                structEmoji.emoji = "Laughing";
                structEmoji.number = 1;
                break;
            case "Laughing2(Clone)":
                structEmoji.emojiName = "Laughing2";
                structEmoji.emoji = "Laughing";
                structEmoji.number = 2;
                break;
            case "Involve1(Clone)":
                structEmoji.emojiName = "Involve1";
                structEmoji.emoji = "Involve";
                structEmoji.number = 1;
                break;
            case "Involve2(Clone)":
                structEmoji.emojiName = "Involve2";
                structEmoji.emoji = "Involve";
                structEmoji.number = 2;
                break;
            case "Smiling1(Clone)":
                structEmoji.emojiName = "Smiling1";
                structEmoji.emoji = "Smiling";
                structEmoji.number = 1;
                break;
            case "Smiling2(Clone)":
                structEmoji.emojiName = "Smiling2";
                structEmoji.emoji = "Smiling";
                structEmoji.number = 2;
                break;
            case "Surprised1(Clone)":
                structEmoji.emojiName = "Surprised1";
                structEmoji.emoji = "Surprised";
                structEmoji.number = 1;
                break;
            case "Surprised2(Clone)":
                structEmoji.emojiName = "Surprised2";
                structEmoji.emoji = "Surprised";
                structEmoji.number = 2;
                break;
            default:
                break;

        }
        return structEmoji;
    }
}
