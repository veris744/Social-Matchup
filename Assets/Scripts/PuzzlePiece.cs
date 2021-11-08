using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour, IPunInstantiateMagicCallback
{

	public int correctRow;
	public int correctColumn;

	public int index;
	
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		object[] data = this.gameObject.GetPhotonView().InstantiationData;
		//info.photonView.instantiationData;
		correctRow = (int) data[0];
		correctColumn = (int) data[1];
		gameObject.GetComponent<DraggableObject>().puzzling = true;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
