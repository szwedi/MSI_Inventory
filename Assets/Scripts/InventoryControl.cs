using UnityEngine;
using System.Collections;

public class InventoryControl : MonoBehaviour {

	public Texture2D inventoryBackground;
	public float inventoryWidth = 525;
	public float inventoryHeight = 325;

	private bool showInventory = false;

	public RaycastHit hit;
	private Ray ray;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit,3)){
			if (hit.transform.tag == "item")
				Debug.Log ("Hit " + hit.transform.name);
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			if (showInventory == false) {
				showInventory = true;
			} else {
				showInventory = false;
			}
		}
	}

	void OnGUI() {
		if (showInventory == true)
			GUI.DrawTexture(new Rect(Screen.width/2-inventoryWidth/2,Screen.height/2-inventoryHeight/2,inventoryWidth,inventoryHeight), inventoryBackground);	
	}
}
