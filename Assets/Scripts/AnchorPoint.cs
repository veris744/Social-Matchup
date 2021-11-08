using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

//class for the X marks in the Sorting game mode. 
public class AnchorPoint : MonoBehaviour
{
    public int Index { get; private set; } //index used by the SortingGameManager to keep trace of the position

    [SerializeField]
    public GameObject anchoredObject; //current object attached to the anchorPoint
    public bool puzzling = false;
    private void Update()
    {
        //keep the object freezed in the same position (a little above the table surface)
        if (anchoredObject != null)
        {
            anchoredObject.transform.position = this.transform.position + new Vector3(0f, 0.35f, 0f);
            if (puzzling) anchoredObject.transform.position = this.transform.position;
        }
    }

    [PunRPC]
    public void OnObjectPositioned(int viewId)
    {
        anchoredObject = PhotonView.Find(viewId).gameObject;
    }

    [PunRPC]
    public void OnObjectRemoved()
    {
        anchoredObject = null;
    }

    [PunRPC]
    public void SetIndex(int index)
    {
        Index = index;
    }
}
