using UnityEngine;
using System.Collections.Generic;

public class InventoryControl : MonoBehaviour {

	public Texture2D inventoryBackground;
	public float inventoryWidth = 525;
	public float inventoryHeight = 325;

	private bool showInventory = false;
	private bool showLabelGetItem = false;

	public RaycastHit hit;
	private Ray ray;

	public Slot[,] slots;
	private int slotsX = 5;
	private int slotsY = 3;

	// Use this for initialization
	void Start () {

		//initialization slots
		slots = new Slot[slotsX, slotsY];
		for (int tmpX = 0; tmpX < slotsX; tmpX++) {
			for (int tmpY = 0; tmpY < slotsY; tmpY++) {
				slots[tmpX, tmpY] = new Slot();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		//display label to get item
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 2) && hit.transform.tag == "item") {
			showLabelGetItem = true;
			//get item to slots
			if (Input.GetKeyDown (KeyCode.E)) {
				addItemToSlots(hit.transform.gameObject);
				Destroy (hit.transform.gameObject);
				if (slots[1,0].item != null)	
					Debug.Log("0 " + slots[1,0].item.name);
				if (slots[1,1].item != null)
					Debug.Log("1 " + slots[1,1].item.name);
				if (slots[1,2].item != null)
					Debug.Log("2 " + slots[1,2].item.name);
			}
		} else {
			showLabelGetItem = false;
		}

		//open inventory
		if (Input.GetKeyDown (KeyCode.I)) {
			if (showInventory == false) {
				showInventory = true;
			} else {
				showInventory = false;
			}
		}

	}
	
	//Display GUI
	void OnGUI() {

		if (showLabelGetItem == true)
			GUI.Label (new Rect (Screen.width / 2-100, Screen.height*0.8f, 200, 50), "Press E to get item");

		if (showInventory == true)
			GUI.DrawTexture(new Rect(Screen.width/2-inventoryWidth/2,Screen.height/2-inventoryHeight/2,inventoryWidth,inventoryHeight), inventoryBackground);	
	}

	//Add item to first not occupieted slot main inventory  
	void addItemToSlots(GameObject item)
	{
		for (int tmpX = 1; tmpX < slotsX; tmpX++) {
			for (int tmpY = 0; tmpY < slotsY; tmpY++) {
				if (slots[tmpX, tmpY].occupied == false){
					slots [tmpX, tmpY].item = item;
					slots [tmpX, tmpY].occupied = true;
					return;
				}
			}
		}
	}
}
