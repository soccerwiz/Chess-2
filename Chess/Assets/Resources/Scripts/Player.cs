using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider.gameObject.tag == "SQUARE" || hit.collider.gameObject.tag == "PIECE"){
					Debug.Log ("HIT: " + hit.collider.gameObject.name);
				}
			}
		}
	}
}
