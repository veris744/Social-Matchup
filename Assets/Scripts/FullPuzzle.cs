using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullPuzzle : MonoBehaviour
{

	public void Initialize(Texture2D tex, float width, float height)
	{
		transform.localScale = new Vector3(width*1.5f,height*1.5f,1);
		gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = tex;
	}
}
