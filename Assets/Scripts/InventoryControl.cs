﻿using UnityEngine;
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
		if (Physics.Raycast (ray, out hit, 2) && hit.transform.tag == "item") {
			showLabelGetItem = true;
			//get item to slots
			if (Input.GetKeyDown (KeyCode.E)) {
				addItemToSlots(hit.transform.gameObject);
				//pomyśleć jak zrobić kopie obiektu a nie dezaktywować obiekt
				//Destroy (hit.transform.gameObject);
				hit.transform.gameObject.SetActive(false);
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
		}
	}
	//Add item to first not occupied slot main inventory  
	void addItemToSlots(GameObject item)
	{
		for (int tmpY = 1; tmpY < slotsY; tmpY++) {
			for (int tmpX = 0; tmpX < slotsX; tmpX++) {
				if (slots[tmpY, tmpX].occupied == false){
					slots [tmpY, tmpX].item = item;
					slots [tmpY, tmpX].occupied = true;
					return;
				}
			}
		}
	}
}
