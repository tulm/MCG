using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using SimpleSQL;

public class PlayerS : MonoBehaviour {
	public static PlayerS control;

	//public DateTime bait1Start;
	//public DateTime bait2Start;
	public int SilverCoins=0, GoldCoins=0;
	public double bait1StartValSecs=240;
	public int bait1=100;
	public int bait1Q=2;

	public bool bait1B = true;
	public Text bait1T;
	public Text SilverT, GoldT;
	//public double bait2StartValSecs=240;
	//public int bait2=0;
	//public bool bait2B = false;
	//public int bait2Q=1;
	//public Text bait2T;

	public CanvasGroup SpaceCanvas;
	public CanvasGroup PlayerItemsCanvas;
	public CanvasGroup TrainingCanvas;

	public int minSecondsConstant;
	public GameManager gameManager;
	public FormulaS formulas;

	public bool showPresentsNotification;
	public bool showMementoNotifcation;
	public bool sceneloaded=false;
	public int menuShown = 0;
	//0 is Training
	//1 is Items

	//private SpaceT spacesList;
	//DBs for 
	//PlayerItemT
	//PlayerMementosT
	//PlayerMonstersT
	//PlayerPrefs

	//public SimpleSQL.SimpleSQLManager dbManager;
	//private List<PlayerItemsT> _playerItemsList;

	void Awake() {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
			menuShown = 0;
		} else if (control != this) {
			Destroy (gameObject);
		}
	}
	public void Save() {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file= File.Create( Application.persistentDataPath+"/playerInfo.dat");
		PlayerData data = new PlayerData();

		data.bait1=bait1;
		data.bait1Q = bait1Q;
		data.SilverCoins = SilverCoins;
		data.GoldCoins = GoldCoins;

		bf.Serialize(file, data);
		file.Close();
	}

	public void Load(){
		if (File.Exists(Application.persistentDataPath+"/playerinfo.dat")){
			BinaryFormatter bf= new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath+"/playerinfo.dat", FileMode.Open);
			PlayerData data = (PlayerData) bf.Deserialize(file);  //CAST your serialized data!

			Debug.Log ("we loaded info on bait1 and stuff");

			bait1 = data.bait1;
			bait1Q = data.bait1Q;
			SilverCoins = data.SilverCoins;
			GoldCoins = data.GoldCoins;
		}
	}
	// Use this for initialization
	void Start () {
		//we need to read in values from SAVED for the Player on start
		//for right now, just going to set bait1 to 100;

		bait1B = true;
		bait1=100;
		bait1T.text=bait1.ToString();
		bait1Q = 2;

		Load ();
	}
	public void LoadLevel (int menuToShow) {
		
		//load the item inventory scene
		//Application.LoadLevel(LvlName);
		menuShown=menuToShow;
		sceneloaded = false;
	}

	// Update is called once per frame
	void Update () {

		//we only update bait and monsters upon a Resume but we also need to make sure things are there from start
		if (gameManager.resumed) {
			gameManager.resumed = false;
			//debugText.text += "\nPlayerS: gameManager.resumed == true";
			if (gameManager.minimizedSeconds > minSecondsConstant) {

				menuShown = 0;
				sceneloaded = false;

				//calculate baits
				//they only decrease DURING MINIMIZED time -- see if that's what you want
				formulas.CalcBait ();

				//clean up/remove any creatures whose time is over and add to Presents and Memento
				//formulas.UpdateSpace ();
			

				}


				//update VISUAL baits
				//bait1T.text = bait1.ToString ();
				//bait2T.text = bait2.ToString();

				//ClearDebugPanel ();
				//update visual notifications for presents and mementos
		
		
			//for now, this will serve as the main menu screen
			//and for now, we will just show bait and all the items in PlayerItemsT
			//and make them clickable so that you can place them in Area or remove them if already placed in Area
			//ClearDebugPanel ();
			//ListPlayerItems();

		}//end gameManager.resumed
		//if ((Application.loadedLevelName == "DebugScene") && sceneloaded==false) {
		if ((menuShown==1) && sceneloaded==false) {
			sceneloaded = true;
			SilverT.text = SilverCoins.ToString ();
			GoldT.text = GoldCoins.ToString ();
			formulas.TurnOffCanvasGroup (SpaceCanvas);
			formulas.TurnOffCanvasGroup (TrainingCanvas);
			formulas.TurnOnCanvasGroup (PlayerItemsCanvas);
			formulas.ShowItemsToPlace ();
		}
		//if ((Application.loadedLevelName=="TrainingArea") && sceneloaded==false) {
		if ((menuShown==0) && sceneloaded==false) {
			sceneloaded=true;
			bait1T.text = bait1.ToString ();
			formulas.TurnOffCanvasGroup (SpaceCanvas);
			formulas.TurnOnCanvasGroup (TrainingCanvas);
			formulas.TurnOffCanvasGroup (PlayerItemsCanvas);
			formulas.UpdateSpace ();
		}
	}
	[System.Serializable]
	class PlayerData{
		public int SilverCoins, GoldCoins;
		public int bait1;
		public int bait1Q;
	}

	/*
	public void ClearDebugPanel(){
		//debugText.text = "";
	}
	public void ListPlayerItems () {
		//need to access PlayerItemsT
		//get the list of ones whose states are owned and/or placed [not NOT owned]
		_playerItemsList = new List<PlayerItemsT> (from ps in dbManager.Table<PlayerItemsT> () where ps.ItemState==0 select ps);

		//do a loop and display item name and item state and a button with either a connection to a function
		foreach (PlayerItemsT anItem in _playerItemsList) {
			debugText.text += "\nthe placed creature list has: " + anItem.ItemID+" with state of: "+anItem.ItemState;

		}
		//to remove it from Area or Place it in Area, and with the itemID associated with it
	}*/
}
