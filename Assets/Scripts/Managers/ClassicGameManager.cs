using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassicGameManager : GameManager
{

    struct EmojiStruct
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
            default:
                break;

        }
        return structEmoji;
    }

    public string selected1 = "none1";
    public string selected2 = "none2";

    Vector3 baseEmojiPosition1 = new Vector3(0, 3, 1.1f);
    Vector3 baseEmojiPosition2 = new Vector3(0, 3, -1.1f);

    public GameObject angry1;
    public GameObject angry2;
    public GameObject crying1;
    public GameObject crying2;

    int init = 0;


    // Start is called before the first frame update
    void Start()
    {
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
                Debug.Log("Object clicked: " + hit.transform.gameObject.name);
                EmojiStruct emjStruct = StringToEmojiStruct(hit.transform.gameObject.name);
                Debug.Log(emjStruct.gameObjectName + emjStruct.emojiName + emjStruct.number);
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
        if (selected1 == selected2)
        {
            Destroy(GameObject.Find(selected1.ToString() + "1").gameObject);
            Destroy(GameObject.Find(selected1 + "2").gameObject);
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
    }


    void OnClick(EmojiStruct structEmoji)
    {
        if(structEmoji.number == 1)
        {
            Debug.Log("selected1: " + selected1);
            if (String.Equals(selected1, structEmoji.emojiName))
            {
                selected1 = "none1";
                GameObject.Find(structEmoji.gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);

            }
            else
            {
                if (!String.Equals(selected1, structEmoji.emojiName))
                {
                    GameObject.Find(selected1.ToString() + "(Clone)").transform.localScale = new Vector3(50f, 50f, 50f);
                }
                selected1 = structEmoji.emojiName;
                GameObject.Find(structEmoji.gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);
            }
        } else
        {
            Debug.Log("selected1: " + selected1);
            if (structEmoji.number == 2)
            {
                if (String.Equals(selected2, structEmoji.emojiName))
                {
                    selected2 = "none2";
                    GameObject.Find(structEmoji.gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);

                }
                else
                {
                    if (!String.Equals(selected2, structEmoji.emojiName))
                    {
                        GameObject.Find(selected2.ToString() + "(Clone)").transform.localScale = new Vector3(50f, 50f, 50f);
                    }
                    selected2 = structEmoji.emojiName;
                    GameObject.Find(structEmoji.gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);
                }
            }
            else
            {
                Debug.Log("warning: emojiStruct might not be instantiated");
            }
        }

        
    }

    void victory()
    {

    }
}
