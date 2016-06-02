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

	public int SilverCoins, GoldCoins;
	public double bait1StartValSecs;
	const double B1START = 240;
	public int bait1;
	public int bait1Q;

	public bool bait1B = true;
	public Text bait1T;
	public Text SilverT, GoldT;
	//public double bait2StartValSecs=B1START;
	//public int bait2=0;
	//public bool bait2B = false;
	//public int bait2Q=1;
	//public Text bait2T;
	public int minSecondsConstant;

	public CanvasGroup SpaceCanvas;
	public CanvasGroup PlayerItemsCanvas;
	public CanvasGroup TrainingCanvas;
	public CanvasGroup PresentsCanvas;

	public GameManager gameManager;
	public FormulaS formulas;
	public PersistentSaving myPS;
	public PlayerData data;

	public bool showPresentsNotification;
	public bool showMementoNotifcation;
	public bool sceneloaded=false;
	public int menuShown = 0;
	//0 is Training
	//1 is Items
	//2 is Presents

	void Awake() {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
			menuShown = 0;
			myPS=new PersistentSaving();
		} else if (control != this) {
			Destroy (gameObject);
		}

		minSecondsConstant = 1;
		//bait1StartValSecs = B1START; //put back after testing
	}

	public void LoadLevel (int menushow){
		menuShown = menushow;
		sceneloaded = false;
	}

	public void AddBait1(int baitQuality){
		Debug.Log ("Bait1 is set to 100");
		bait1 = 100;
		bait1B = true;
		bait1Q = baitQuality;
		bait1StartValSecs = B1START;
		Save ();

		//sceneloaded = false; //maybe this would be as good idea INSTEAD of just setting bait text
		bait1T.text=bait1.ToString();
	}

	public void Save() {
		//Debug.Log ("Went to SAVE command so now we will see if playerInfo.dat was created in the persistent datapath");
		data.bait1=bait1;
		data.bait1Q = bait1Q;
		data.bait1StartValSecs = bait1StartValSecs;
		data.SilverCoins = SilverCoins;
		data.GoldCoins = GoldCoins;

		myPS.Save (data);
	}

	public void Load(){
		//Debug.Log("in LOAD");
		myPS.Load (ref data);

		if (data != null) {
			bait1 = data.bait1;
			bait1Q = data.bait1Q;
			bait1StartValSecs = data.bait1StartValSecs;
			SilverCoins = data.SilverCoins;
			GoldCoins = data.GoldCoins;
			if (bait1 > 0)
				bait1B = true;
			else
				bait1B = false;
		} else {
			//in case first time, then nothing to Load(), so put defaults here to be overwritten if something to Load
			bait1B = true;
			bait1=100;
			bait1Q = 1;
			bait1StartValSecs = B1START;
			SilverCoins = 100;
			GoldCoins = 10;
		}
	}

	// Use this for initialization
	void Start () {
		Load ();
		bait1T.text=bait1.ToString();
		//Debug.Log("PlayerS hit Start. set bait1 to 100 but after Load() it is? "+ bait1);
	}

	void Update () {

		//we only update bait and monsters upon a Resume but we also need to make sure things are there from start
		if (gameManager.resumed) {
			gameManager.resumed = false;
			if (gameManager.minimizedSeconds > minSecondsConstant) {
				//Calculate Baits: they only decrease DURING MINIMIZED time -- TODO see if that's what you want
				formulas.CalcBait ();
				menuShown = 0;
				sceneloaded = false;
				} //end RESUME only events
		}//end gameManager.resumed

		//Training Capture Area
		if ((menuShown==0) && sceneloaded==false) {
			sceneloaded=true;
			Load ();
			bait1T.text = bait1.ToString ();
			formulas.TurnOffCanvasGroup (SpaceCanvas);
			formulas.TurnOnCanvasGroup (TrainingCanvas);
			formulas.TurnOffCanvasGroup (PresentsCanvas);
			formulas.TurnOffCanvasGroup (PlayerItemsCanvas);
			formulas.UpdateSpace ();
		}
		//Inventory Area: Place item scene
		if ((menuShown==1) && sceneloaded==false) {
			sceneloaded = true;
			SilverT.text = SilverCoins.ToString ();
			GoldT.text = GoldCoins.ToString ();
			formulas.TurnOffCanvasGroup (SpaceCanvas);
			formulas.TurnOffCanvasGroup (TrainingCanvas);
			formulas.TurnOffCanvasGroup (PresentsCanvas);
			formulas.TurnOnCanvasGroup (PlayerItemsCanvas);
			formulas.ShowItemsToPlace ();
			Save ();
		}
		//Presents Area
		if ((menuShown==2) && sceneloaded==false) {
			sceneloaded=true;
			Load ();
			bait1T.text = bait1.ToString ();
			formulas.TurnOffCanvasGroup (SpaceCanvas);
			formulas.TurnOffCanvasGroup (TrainingCanvas);
			formulas.TurnOnCanvasGroup (PresentsCanvas);
			formulas.TurnOffCanvasGroup (PlayerItemsCanvas);
			formulas.ShowPresents ();
		}
	}
}
