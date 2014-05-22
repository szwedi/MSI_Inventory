using UnityEngine;
using System.Collections.Generic;

public class InventoryControl : MonoBehaviour {

	public Texture2D inventoryBackground;
	public float inventoryWidth = 525;
	public float inventoryHeight = 325;
	private bool showInventory = false;
	private bool showLabelGetItem = false;
	
	private int activedItem = 0;

	public RaycastHit hit;
	private Ray ray;

	public Slot[,] slots;
	private int slotsX = 5;
	private int slotsY = 3;

	public Transform itemHandler;

	private int originalSlotX;
	private int originalSlotY;
	private bool take;

	public Texture2D descriptionImage;
	public GUIStyle labelStyle;
	
	// Use this for initialization
	void Start () {
		//initialization slots
		slots = new Slot[slotsY, slotsX];
		for (int tmpY = 0; tmpY < slotsY; tmpY++) {
			for (int tmpX = 0; tmpX < slotsX; tmpX++) {
				slots[tmpY, tmpX] = new Slot();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		//display label to get item
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 2) && hit.transform.tag == "item" && hit.transform.gameObject.GetComponent<Item>().taken == false) {
			showLabelGetItem = true;
			//get item to slots
			if (Input.GetKeyDown (KeyCode.E)) {
				addItemToSlots(hit.transform.gameObject);
			}
		} else {
			showLabelGetItem = false;
		}

		//open inventory
		if (Input.GetKeyDown (KeyCode.I)) {
			if (showInventory == false) {
				showInventory = true;
				take = false;
				foreach(var mouseLook in GetComponentsInChildren<MouseLook>())
					mouseLook.enabled = false;
			} else {
				showInventory = false;
				foreach(var mouseLook in GetComponentsInChildren<MouseLook>())
					mouseLook.enabled = true;
			}
		}

		//active item by shortcut
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			activeItem (1);	
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			activeItem (2);	
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			activeItem (3);	
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			activeItem (4);	
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			activeItem (5);	
		}

		//drop item
		if (Input.GetKeyDown (KeyCode.Q)) {
			dropItem(activedItem);		
		}
	}
	
	//Display GUI
	void OnGUI() {

		if (showLabelGetItem == true)
			GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height * 0.8f, 200, 50), "Press E to get item");

		if (showInventory == true) {
			//draw inventory background
			GUI.DrawTexture (new Rect (Screen.width / 2 - inventoryWidth / 2, Screen.height / 2 - inventoryHeight / 2, inventoryWidth, inventoryHeight), inventoryBackground);	
			//draw occuoued slot image
			for (int tmpY = 0; tmpY < slotsY; tmpY++) {
				for (int tmpX = 0; tmpX < slotsX; tmpX++) {
					if (slots[tmpY, tmpX].occupied == true){
						GUI.DrawTexture(new Rect(Screen.width / 2 - inventoryWidth / 2 + 25 + tmpX*100, Screen.height / 2 - inventoryHeight / 2 + 25 + tmpY*100, 75, 75),slots[tmpY, tmpX].item.GetComponent<Item>().itemImage);
					}
				}
			}
			//drag and drop item
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0){
				Vector2 mouse = Event.current.mousePosition;
				for (int tmpY = 0; tmpY < slotsY; tmpY++) {
					for (int tmpX = 0; tmpX < slotsX; tmpX++) {
						if (mouse.x > Screen.width / 2 - inventoryWidth / 2 + 25 + tmpX*100 && 
						    mouse.x < Screen.width / 2 - inventoryWidth / 2 + 100 + tmpX*100 &&
						    mouse.y > Screen.height / 2 - inventoryHeight / 2 + 25 + tmpY*100 &&
						    mouse.y < Screen.height / 2 - inventoryHeight / 2 + 100 + tmpY*100){
							if (take == false && slots [tmpY, tmpX].occupied == true){
								Debug.Log ("copy");
								originalSlotX = tmpX;
								originalSlotY = tmpY;
								take = true;
							} 
							if (take == true && slots [tmpY, tmpX].occupied == false){
								Debug.Log ("paste");
								slots [tmpY, tmpX].item = slots [originalSlotY, originalSlotX].item;
								slots [tmpY, tmpX].occupied = true;
								slots [originalSlotY, originalSlotX].item = null;
								slots [originalSlotY, originalSlotX].occupied = false;
								take = false;
							}
							Debug.Log("occ = " + slots[tmpY,tmpX].occupied + " , take = " + take);
						}
					}
				}
				Debug.Log("[" + originalSlotY + "," + originalSlotX + "]");
			}
			// show description
			if (Input.GetMouseButton(1)){
				Vector2 mouse = Event.current.mousePosition;
				for (int tmpY = 0; tmpY < slotsY; tmpY++) {
					for (int tmpX = 0; tmpX < slotsX; tmpX++) {
						if (mouse.x > Screen.width / 2 - inventoryWidth / 2 + 25 + tmpX*100 && 
						    mouse.x < Screen.width / 2 - inventoryWidth / 2 + 100 + tmpX*100 &&
						    mouse.y > Screen.height / 2 - inventoryHeight / 2 + 25 + tmpY*100 &&
						    mouse.y < Screen.height / 2 - inventoryHeight / 2 + 100 + tmpY*100){
							if (slots [tmpY, tmpX].occupied == true){
								GUI.DrawTexture(new Rect(mouse.x, mouse.y - 200, 200, 200),descriptionImage);
								GUI.Label(new Rect(mouse.x, mouse.y - 200, 200, 200),slots[tmpY, tmpX].item.GetComponent<Item>().description, labelStyle);
							}
						}
					}
				}
			}
		}
	}
	
	//Add item to first not occupied slot main inventory  
	void addItemToSlots(GameObject item)
	{
		for (int tmpY = 1; tmpY < slotsY; tmpY++) {
			for (int tmpX = 0; tmpX < slotsX; tmpX++) {
				if (slots[tmpY, tmpX].occupied == false){
					item.SetActive(false);
					item.GetComponent<Item>().taken = true;
					item.transform.parent = itemHandler;
					item.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
					item.transform.localPosition = new Vector3(0,-1,2);
					item.rigidbody.isKinematic = true;
					slots [tmpY, tmpX].item = item;
					slots [tmpY, tmpX].occupied = true;
					return;
				}
			}
		}
	}

	void activeItem(int activeItem){
		if (slots [0, activeItem-1].occupied == true){
			if (activedItem != 0)
				slots [0, activedItem-1].item.gameObject.SetActive(false);
			slots [0,activeItem-1].item.gameObject.SetActive(true);
			activedItem = activeItem;
		}
	}

	void dropItem(int dropItem){
		if (activedItem != 0) {
			slots [0, dropItem - 1].item.GetComponent<Item> ().taken = false;
			slots [0, dropItem - 1].item.rigidbody.isKinematic = false;
			slots [0, dropItem - 1].item.transform.parent = null;
			slots [0, dropItem - 1].item = null;
			slots [0, dropItem - 1].occupied = false;
			activedItem = 0;
		}
	}
}
