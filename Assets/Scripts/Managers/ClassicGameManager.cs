using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassicGameManager : GameManager
{

    public struct EmojiStruct
    {
        public string gameObjectName; // Angry1(Clone)
        public string emojiName; // Angry1
        public int number; //1
    };

    EmojiStruct StringToEmojiStruct(string s)
    {
        EmojiStruct structEmoji;
        structEmoji.gameObjectName = s;
        structEmoji.emojiName = "none";
        structEmoji.number = 0;
        switch (s)
        {
            case "Angry1(Clone)":
                structEmoji.emojiName = "Angry1";
                structEmoji.number = 1;
                break;
            case "Angry2(Clone)":
                structEmoji.emojiName = "Angry2";
                structEmoji.number = 2;
                break;
            case "Crying1(Clone)":
                structEmoji.emojiName = "Crying1";
                structEmoji.number = 1;
                break;
            case "Crying2(Clone)":
                structEmoji.emojiName = "Crying2";
                structEmoji.number = 2;
                break;
            case "Embarassed1(Clone)":
                structEmoji.emojiName = "Embarassed1";
                structEmoji.number = 1;
                break;
            case "Embarassed2(Clone)":
                structEmoji.emojiName = "Embarassed2";
                structEmoji.number = 2;
                break;
            case "Laughing1(Clone)":
                structEmoji.emojiName = "Laughing1";
                structEmoji.number = 1;
                break;
            case "Laughing2(Clone)":
                structEmoji.emojiName = "Laughing2";
                structEmoji.number = 2;
                break;
            case "Scared1(Clone)":
                structEmoji.emojiName = "Scared1";
                structEmoji.number = 1;
                break;
            case "Scared2(Clone)":
                structEmoji.emojiName = "Scared2";
                structEmoji.number = 2;
                break;
            case "Smiling1(Clone)":
                structEmoji.emojiName = "Smiling1";
                structEmoji.number = 1;
                break;
            case "Smiling2(Clone)":
                structEmoji.emojiName = "Smiling2";
                structEmoji.number = 2;
                break;
            case "Surprised1(Clone)":
                structEmoji.emojiName = "Surprised1";
                structEmoji.number = 1;
                break;
            case "Surprised2(Clone)":
                structEmoji.emojiName = "Surprised2";
                structEmoji.number = 2;
                break;
            default:
                break;

        }
        return structEmoji;
    }

    public EmojiStruct selected1;
    public EmojiStruct selected2;

    Vector3 baseEmojiPosition1 = new Vector3(0, 3, 1.1f);
    Vector3 baseEmojiPosition2 = new Vector3(0, 3, -1.1f);

    public GameObject angry1;
    public GameObject angry2;
    public GameObject crying1;
    public GameObject crying2;
    public GameObject Embarassed1;
    public GameObject Embarassed2;
    public GameObject Laughing1;
    public GameObject Laughing2;
    public GameObject Scared1;
    public GameObject Scared2;
    public GameObject Smiling1;
    public GameObject Smiling2;
    public GameObject Surprised1;
    public GameObject Surprised2;


    int init = 0;

    void ResetSelected1()
    {
        selected1.emojiName = "none1";
        selected1.gameObjectName = "none1";
        selected1.number = 0;
    }
    private void ResetSelected2()
    {
        selected2.emojiName = "none2";
        selected2.gameObjectName = "none2";
        selected2.number = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetSelected1();
        ResetSelected2();
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


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                EmojiStruct emjStruct = StringToEmojiStruct(hit.transform.gameObject.name);
                OnClick(emjStruct);
                /*
                if (thisPlayer.GetInstanceID() == 1001)
                {
                    Debug.Log("player 1001");
                }
                else
                {
                    if(thisPlayer.GetPhotonView().ViewID == 2001)
                    {
                        Debug.Log("player 2001");
                    }
                }
                */
            }
        }
        if (String.Equals(selected1.emojiName, selected2.emojiName))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(GameObject.Find(selected1.gameObjectName).gameObject);
                PhotonNetwork.Destroy(GameObject.Find(selected2.gameObjectName).gameObject);
                ResetSelected1();
                ResetSelected2();
            }
        }

        if (GameObject.FindGameObjectsWithTag("Emoji").Length == 0)
        {
            victory();
        }
    }



    public void SpawnEmojis()
    {
        Vector3 pos1 = new Vector3(1f, 0f, 0f);
        angry1 = PhotonNetwork.Instantiate("Models/Prefab/Angry1", baseEmojiPosition1, Quaternion.identity, 0);
        angry2 = PhotonNetwork.Instantiate("Models/Prefab/Angry2", baseEmojiPosition2, Quaternion.identity, 0);
        crying1 = PhotonNetwork.Instantiate("Models/Prefab/Crying1", baseEmojiPosition1 + pos1, Quaternion.identity, 0);
        crying2 = PhotonNetwork.Instantiate("Models/Prefab/Crying2", baseEmojiPosition2 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Embarassed1", baseEmojiPosition1 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Embarassed2", baseEmojiPosition2 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Laughing1", baseEmojiPosition1 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Laughing2", baseEmojiPosition2 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Scared1", baseEmojiPosition1 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Scared2", baseEmojiPosition2 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Smiling1", baseEmojiPosition1 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Smiling2", baseEmojiPosition2 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Surprised1", baseEmojiPosition1 + pos1, Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Surprised2", baseEmojiPosition2 + pos1, Quaternion.identity, 0);
    }


    void OnClick(EmojiStruct structEmoji)
    {
        if(structEmoji.number == 1)
        {
            if (String.Equals(selected1, structEmoji.emojiName))
            {
                ResetSelected1();
                GameObject.Find(structEmoji.gameObjectName).transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                if (!String.Equals(selected1.emojiName, "none1"))
                {
                    GameObject.Find(selected1.emojiName + "(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
                }
                selected1 = structEmoji;
                GameObject.Find(structEmoji.gameObjectName).transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
        } else
        {
            if (structEmoji.number == 2)
            {
                if (String.Equals(selected2.emojiName, structEmoji.emojiName))
                {
                    ResetSelected2();
                    GameObject.Find(structEmoji.gameObjectName).transform.localScale = new Vector3(1f, 1f, 1f);

                }
                else
                {
                    if (!String.Equals(selected2.emojiName, "none2"))
                    {
                        GameObject.Find(selected2.emojiName + "(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    selected2 = structEmoji;
                    GameObject.Find(structEmoji.gameObjectName).transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                }
            }
            else
            {
                Debug.Log("structEmoji.number is different to 1 or 2");
            }
        }

        
    }

    void victory()
    {

    }
}
