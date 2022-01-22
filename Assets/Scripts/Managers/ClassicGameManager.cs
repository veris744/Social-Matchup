using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassicGameManager : MonoBehaviour
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
        /*
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
        }*/
    }



    public void SpawnEmojis()
    {
        angry1 = PhotonNetwork.Instantiate("Models/Prefab/Angry", baseEmojiPosition1, Quaternion.identity, 0);
        GameObject.Find("Angry(Clone)").name = "angry1";
        angry2 = PhotonNetwork.Instantiate("Models/Prefab/Angry", baseEmojiPosition2, Quaternion.identity, 0);
        GameObject.Find("Angry(Clone)").name = "angry1";

        //angryButton1 = PhotonNetwork.Instantiate("Models/Prefab/emojiButton", new Vector3(1, 3, 1.1f), Quaternion.identity, 0);
        //angryButton2 = PhotonNetwork.Instantiate("Models/Prefab/emojiButton", new Vector3(1, 3, -1.1f), Quaternion.identity, 0);

    }

    void victory()
    {

    }
}
