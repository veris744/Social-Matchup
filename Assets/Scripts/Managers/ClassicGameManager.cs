using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassicGameManager : GameManager
{
    public enum EmojiEnum
    {
        none1,
        none2,
        angry,
        crying,
        embarassed,
        laughing,
        scared,
        smiling
    }

    public EmojiEnum selected1 = EmojiEnum.none1;
    public EmojiEnum selected2 = EmojiEnum.none2;

    Vector3 baseEmojiPosition1 = new Vector3(0, 3, 1.1f);
    Vector3 baseEmojiPosition2 = new Vector3(0, 3, -1.1f);

    public GameObject angry1;
    public GameObject angry2;
    private GameObject angryButton1;
    private GameObject angryButton2;

    int init = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        if(init==0)
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
            }
        }
        if (selected1 == selected2)
        {
            Destroy(GameObject.Find(selected1.ToString() + "1").gameObject);
            Destroy(GameObject.Find(selected1+"2").gameObject);
        }

        if (GameObject.FindGameObjectsWithTag("Emoji").Length == 0)
        {
            victory();
        }
    }


    public void ClickOnAngry1()
    {
        Debug.Log("ClickOnAngry1");
        OnClick1("angry1", EmojiEnum.angry);
    }

    public void ClickOnAngry2()
    {
        Debug.Log("ClickOnAngry2");
        OnClick2("angry2", EmojiEnum.angry);
    }
    
    void OnClick1(string gameObjectName, EmojiEnum emojiName)
    {
        if (selected1.CompareTo(emojiName) == 0)
        {
            selected1 = EmojiEnum.none1;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);

        }
        else
        {
            if (selected1.CompareTo(EmojiEnum.none1) != 0)
            {
                GameObject.Find(selected1.ToString() + "1").transform.localScale = new Vector3(50f, 50f, 50f);
            }
            selected1 = emojiName;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);
        }
    }

    void OnClick2(string gameObjectName, EmojiEnum emojiName)
    {
        if (selected2.CompareTo(emojiName)==0)
        {
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);
            selected2 = EmojiEnum.none2;
        }
        else
        {
            if(selected2.CompareTo(EmojiEnum.none2) != 0)
            {
                GameObject.Find(selected2.ToString() + "2").transform.localScale = new Vector3(50f, 50f, 50f);
            }
            
            selected2 = emojiName;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);

        }
    }

    public void SpawnEmojis()
    {
        angry1 = PhotonNetwork.Instantiate("Models/Prefab/Angry", baseEmojiPosition1, Quaternion.identity, 0);
        GameObject.Find("Angry(Clone)").name = "angry1";
        angry2 = PhotonNetwork.Instantiate("Models/Prefab/Angry", baseEmojiPosition2, Quaternion.identity, 0);
        GameObject.Find("Angry(Clone)").name = "angry1";

        angryButton1 = PhotonNetwork.Instantiate("Models/Prefab/emojiButton", new Vector3(1, 3, 1.1f), Quaternion.identity, 0);
        angryButton2 = PhotonNetwork.Instantiate("Models/Prefab/emojiButton", new Vector3(1, 3, -1.1f), Quaternion.identity, 0);

        angryButton1.AddComponent(typeof(EventTrigger));
        EventTrigger trigger = angryButton1.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { ClickOnAngry1(); });
        trigger.triggers.Add(entry);

        angryButton2.AddComponent(typeof(EventTrigger));
        EventTrigger trigger2 = angryButton1.GetComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { ClickOnAngry2(); });
        trigger.triggers.Add(entry);

    }

    void victory()
    {

    }
}
