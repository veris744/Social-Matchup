using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiManager : MonoBehaviour
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

    public GameObject angry1;
    public GameObject angry2;

    void Start()
    {
    }

    void Update()
    {
        
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

    public void ClickOnCrying1()
    {
        Debug.Log("ClickOnCrying1");
        OnClick1("angry1", EmojiEnum.angry);
    }

    public void ClickOnCrying2()
    {
        Debug.Log("ClickOnCrying2");
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
        if (selected2.CompareTo(emojiName) == 0)
        {
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);
            selected2 = EmojiEnum.none2;
        }
        else
        {
            if (selected2.CompareTo(EmojiEnum.none2) != 0)
            {
                GameObject.Find(selected2.ToString() + "2").transform.localScale = new Vector3(50f, 50f, 50f);
            }

            selected2 = emojiName;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);

        }
    }
}
