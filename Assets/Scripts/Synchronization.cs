using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ClassicGameManager;

public class Synchronization : MonoBehaviour
{
    public EmojiStruct selected1;
    public EmojiStruct selected2;

    public void SetSelected1(EmojiStruct emj)
    {
        selected1.gameObjectName = emj.gameObjectName;
        selected1.emojiName = emj.emojiName;
        selected1.number = emj.number;
    }
    public void SetSelected2(EmojiStruct emj)
    {
        selected2.gameObjectName = emj.gameObjectName;
        selected2.emojiName = emj.emojiName;
        selected2.number = emj.number;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
