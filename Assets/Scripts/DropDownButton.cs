using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropDownButton : MonoBehaviour
{
    private GameObject parentButton;
    private TextMeshProUGUI thisText;

    private void Start()
    {
        parentButton = transform.parent.parent.gameObject;
        thisText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void OnClick()
    {
        parentButton.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = thisText.text;
    }
}
