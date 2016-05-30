using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using SimpleSQL;

public class monsterListDisplayS : MonoBehaviour {

	// The list of player stats from the database
	//private List<PlayerStats> _playerStatsList;
	private List<MonsterT> _monsterList;
	//private List<MonsterTypeT> _monsterTypelist;

	// These variables will be used to store data from the GUI interface
	private string _newName;
	private string _newDescription;
	private string _newMTID;
	private string _newCID;
	//private string _newMEID;
	private string _newRare;
	private string _newFI1, _newFI2, _newFI3;

	// reference to our db manager object
	public SimpleSQL.SimpleSQLManager dbManager;

	public float offsetX = 100;
	public float offset=-20;
	public float startY = -20;
	public Font myfont;
	public InputField mnF;
	public InputField mdF;
	public InputField mtF;
	public InputField ctF;
	//public InputField memF;
	public InputField rareF;
	public InputField fi1F;
	public InputField fi2F;
	public InputField fi3F;
	private GameObject newGO;
	public Sprite mySprite;
	private bool madeGO1=false;

	void Start() {
		ResetFields ();
		/*
		//GameObject newGO = new GameObject("myTextGO");
		newGO=new GameObject("myTextGO");
		newGO.transform.SetParent(this.transform);
		newGO.transform.localScale = new Vector3 (1, 1, 1);
		newGO.transform.localPosition = new Vector3 (0, 0, 0);
		newGO.layer = 5;

		Text myText = newGO.AddComponent<Text>();
		myText.font = myfont;
		myText.text = "Ta-dah!";
		*/
      
	}

	void Update() {
		/*
		if (!madeGO1) {
			//Debug.Log ("it was init one time hopefully");
			//GameObject newGO = new GameObject("myTextGO");
			newGO = new GameObject ("myTextGO");
			newGO.transform.SetParent (this.transform);
			newGO.transform.localScale = new Vector3 (1, 1, 1);
			newGO.transform.localPosition = new Vector3 (0, 0, 0);
			newGO.layer = 5;

			Text myText = newGO.AddComponent<Text> ();
			myText.font = myfont;
			myText.text = "Ta-dah!";
			ResetFields ();
			madeGO1 = true;
		}
		*/
	}

	void createMonsterLabel( string theText, float theX, float theY) {

		GameObject newGO = new GameObject("MonLabel");
		newGO.transform.SetParent(this.transform);
		newGO.transform.localScale = new Vector3 (1, 1, 1);
		newGO.transform.localPosition = new Vector3 (theX, theY, 0);
		newGO.layer = 5;

		Text myText = newGO.AddComponent<Text>();
		myText.font = myfont;
		myText.text = theText;
		myText.horizontalOverflow = HorizontalWrapMode.Overflow;
		myText.verticalOverflow = VerticalWrapMode.Overflow;

	}


	public void onSubmitMonster () {
		_newName = mnF.text;
		mnF.text="you submitted";
		_newDescription = mdF.text;
		_newMTID = mtF.text;
		_newCID = ctF.text;
		//_newMEID = memF.text;
		_newRare = rareF.text;
		_newFI1 = fi1F.text;
		_newFI2 = fi2F.text;
		_newFI3 = fi3F.text;
			
	
		SavePlayerStats_Query(_newName, _newDescription, int.Parse(_newMTID), int.Parse(_newCID), int.Parse(_newRare), int.Parse(_newFI1), int.Parse(_newFI2), int.Parse(_newFI3));
		ResetFields();
	}

	private void ResetFields()
	{
		// Reset the temporary GUI variables
		_newName = "";
		_newDescription = "";
		_newCID = "";
		_newRare = "";
		//_newMEID = "";
		_newMTID = "";
		_newFI1 = "";
		_newFI2 = "";
		_newFI3 = "";

		mnF.text = "";
		mdF.text = "";
		mtF.text = "";
		ctF.text = "";
		//memF.text = "";
		rareF.text = "";
		fi1F.text = "";
		fi2F.text = "";
		fi3F.text = "";


		// Loads the player stats from the database using Linq
		//_playerStatsList = new List<PlayerStats> (from ps in dbManager.Table<PlayerStats> () select ps);
		_monsterList = new List<MonsterT> (from ps in dbManager.Table<MonsterT> () select ps);
		//_monsterTypelist = new List<MonsterTypeT> (from ps in dbManager.Table<MonsterTypeT> () select ps);
	
		//it does NOT destroy the previous Labels from the last time it listed the Monsters out to Canvas
		foreach (MonsterT playerStats in _monsterList)
		{
			startY = -120;
			offset -= 30;
			createMonsterLabel(playerStats.Name,-330, startY+offset);
			createMonsterLabel (playerStats.Description, -230, startY + offset);
			createMonsterLabel(playerStats.MonsterTypeID.ToString(), -130 , startY + offset);
			createMonsterLabel(playerStats.CageTypeID.ToString(),  -110, startY + offset);
			//createMonsterLabel(playerStats.MementoID.ToString(),  -90 , startY + offset);
			createMonsterLabel(playerStats.Rarity.ToString(),  -70, startY + offset);
			createMonsterLabel(playerStats.FavItem1ID.ToString(), -50 , startY + offset);
			createMonsterLabel(playerStats.FavItem2ID.ToString(), -30 , startY + offset);
			createMonsterLabel(playerStats.FavItem3ID.ToString(),  -10 , startY + offset);

		}
			
	
	}
	
	private void SavePlayerStats_Query(string name, string description, int mid, int cid, int rare, int fi1, int fi2, int fi3)
	{
		// Call our SQL statement using ? to bind our variables
		//dbManager.Execute("INSERT INTO PlayerStats (PlayerName, TotalKills, Points) VALUES (?, ?, ?)", playerName, totalKills, points);
		dbManager.Execute("INSERT INTO MonsterT (Name, Description, MonsterTypeID, CageTypeID, Rarity, FavItem1ID, FavItem2ID, FavItem3ID) VALUES (?, ?, ?, ?, ?  , ? , ? , ?)", name, description, mid, cid, rare, fi1, fi2, fi3).ToString();
		 
	
	}


}
