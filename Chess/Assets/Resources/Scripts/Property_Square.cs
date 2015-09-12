using UnityEngine;
using System.Collections;

public class Property_Square : MonoBehaviour {

	public bool bWhite;
	public int iRow;
	public int iColumn;

	public void SetProperties(bool white, int i, int j){
		bWhite = white;
		iRow = i;
		iColumn = j;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
