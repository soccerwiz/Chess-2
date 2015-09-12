using UnityEngine;
using System.Collections;

public class Property_Piece : MonoBehaviour {

	public bool bIsWhite;
	public GameObject gobSquare;
	public string sType;

	public void SetProperties(bool white, GameObject square, string type){
		bIsWhite = white;
		gobSquare = square;
		sType = type;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
