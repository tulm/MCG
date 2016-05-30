using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using SimpleSQL;

public class FormulaS : MonoBehaviour {
	//playerScript
	//public Sprite testSprite;
	public PlayerS playerScript;
	public GameManager gameManager;

	public CanvasGroup SpaceCanvas;
	public CanvasGroup PlayerItemsCanvas;
	public CanvasGroup TrainingCanvas;
	public ItemMenuS itemMenu;
	//public Text debugText, debugText1, debugText2;

	private List<PlacedCreatureT> placedCreatureList;
	private List<MonsterT> _monsterList;
	private List<SpaceT> spaceList;
	private List<PlayerItemsTTemp> _playerItemsList;

	public int Duration;
	public int StartOffset;
	public int thisStartTime;
	public int thisEndTime;

	public GameObject [] visualSpaces;

	// reference to our db manager object
	public SimpleSQL.SimpleSQLManager dbManager;

	// Use this for initialization 
	void Start () {
		//decide which Canvas panel groups should be visible or invisible
		//TurnOffCanvasGroup(SpaceCanvas);
		//TurnOnCanvasGroup(PlayerItemsCanvas);
		dbManager = GameObject.Find ("DB Manager").GetComponent<SimpleSQL.SimpleSQLManager> ();
	}
		
	// Update is called once per frame
	void Update () {


		//messaging?
		//if you get a notice that:
			//Bait Placed
			//Item Removed
			//Item Placed
			//Resume

	}

	public void TurnOffCanvasGroup (CanvasGroup aCG) {
		aCG.blocksRaycasts=false;
		aCG.interactable = false;
		aCG.alpha = 0;
	}
	public void TurnOnCanvasGroup (CanvasGroup aCG) {
		aCG.blocksRaycasts=true;
		aCG.interactable = true;
		aCG.alpha = 100;
	}
	public void CalcBait () {
		//based on time, bait gets set to a percentage chunk
		//check time since bait was placed
		double baitPassTime=0;
		//debugText.text += "\nFormulaS: CalcBait called";
		if (playerScript.bait1B) {
			Debug.Log ("bait1starval: "+playerScript.bait1StartValSecs);
			playerScript.bait1StartValSecs -= gameManager.minimizedSeconds;
			baitPassTime = playerScript.bait1StartValSecs/60;
			Debug.Log ("bait1starval after sub: "+playerScript.bait1StartValSecs);
			Debug.Log ("baitPassTime -- so divided: "+ baitPassTime);
			if (baitPassTime <= 0) {
				//debugText.text += "\nFormulas: bait1 was less than one so empty, put to 0";
				playerScript.bait1 = 0;
				playerScript.bait1B = false;
			}
			if (baitPassTime > 0 && baitPassTime < 1) {
				//debugText.text += "\nFormulas: bait1 there between 1 and 2 minutes, cut to 25";
				playerScript.bait1 = 25;
			}
			if (baitPassTime > 1 && baitPassTime < 2) {
				//debugText.text += "\nFormulas: bait1 there between 2 and 3 minutes, cut to 50";
				playerScript.bait1 = 50;
			}
			if (baitPassTime > 2 && baitPassTime < 3) {
				//debugText.text += "\nFormulas: bait1 there between 3 and 4 minutes, cut to 75";
				playerScript.bait1 = 75;
			}
			if (baitPassTime >= 3) {
				//debugText.text += "\nFormulas: bait1 is still full at 4 minutes";
				playerScript.bait1 = 90; //TODO should be 100, just trying the save script
			}
		}
		/*
		//calculate bait2 Lesley 1704089
		if (playerScript.bait2B) {
			//baitPassTime = (gameManager.lastMinimize - playerScript.bait2Start).TotalMinutes;
			playerScript.bait2StartValSecs -= gameManager.minimizedSeconds;
			debugText.text += "\nFormulas: bait2startvalsecs is now " + playerScript.bait2StartValSecs;
			baitPassTime = playerScript.bait2StartValSecs/60;

			if (baitPassTime <= 0) {
				debugText.text += "\nFormulas: bait2 was less than one so empty, put to 0";
				playerScript.bait2 = 0;
				playerScript.bait2B = false;
			}
			if (baitPassTime > 0 && baitPassTime < 1) {
				debugText.text += "\nFormulas: bait2 there between 1 and 2 minutes, cut to 25";
				playerScript.bait2 = 25;
			}
			if (baitPassTime > 1 && baitPassTime < 2) {
				debugText.text += "\nFormulas: bait2 there between 2 and 3 minutes, cut to 50";
				playerScript.bait2 = 50;
			}
			if (baitPassTime > 2 && baitPassTime < 3) {
				debugText.text += "\nFormulas: bait2 there between 3 and 4 minutes, cut to 75";
				playerScript.bait2 = 75;
			}
			if (baitPassTime >= 3) {
				debugText.text += "\nFormulas: bait2 is still full at 4 minutes";
				playerScript.bait2 = 100;
			}
		}
		*/
	}
		
	public void UpdateSpace () {
		//need Placed creature db access AND current bait values
		//need to go through all placed creatures and calculate those that have end times that have passed
		//if ended, calculate presents, memento, remove from PlacedCreature list
		int cID, iID, cStartTime, cEndTime, cSpaceID;
	

		//TODO Visually fill in the Spaces with the EMPTY ITEM images FIRST -- then 'add' the images for currently visible creatures
		//maybe move this to an itemMenu method
		List <SpaceT> spaces = new List<SpaceT> (from ps in dbManager.Table<SpaceT> () orderby ps.ID select ps);
	//	visualSpaces[0].GetComponent<Image>().sprite = testSprite;

		foreach (SpaceT aSpace in spaces) {
			//get the coordinates you need for this space
				//you will have a link to visual image object transforms in the scene so that you can replace their images
				//it will be an ordered array so that it matches the SpaceT db which is returned ordered
			//get the image you need for this item using itemID TODO will be more than one for items that can hold more than 0 or 1 creatures
				//string itemImageName=tostring itemID
			string tempSpaceImageName = "ItemImages/i"+aSpace.Item;
			//Debug.Log ("the tempImageSpace should be something like i3: " + tempSpaceImageName);
			Sprite tempSprite = Resources.Load<Sprite>(tempSpaceImageName);
			//TENU stupid android string hidden character test
			//Sprite tempSprite = Resources.Load<Sprite>("i2");
			//Sprite tempSprite = (Sprite)Resources.Load("ItemImages/i2",typeof(Sprite));
			//Sprite tempSprite = Resources.Load("i2") as Sprite; //does not work on computer
			//Sprite tempSprite = Resources.Load<Sprite>("ItemImages/i3");
			//place the image in the visual space
			visualSpaces[aSpace.ID-1].GetComponent<Image>().sprite = tempSprite;
		}



		placedCreatureList = new List<PlacedCreatureT> (from ps in dbManager.Table<PlacedCreatureT> () select ps);

		foreach (PlacedCreatureT aCreature in placedCreatureList.ToList()) {
			//debugText.text += "\nthe placed creature list has: " + aCreature.CreatureID;
			Debug.Log("There is a PlacedCreature with ID: "+aCreature.CreatureID);
			cID = aCreature.CreatureID;
			iID = aCreature.ItemID;
			cStartTime = aCreature.StartTime;
			cEndTime = aCreature.EndTime;
			cSpaceID = aCreature.SpaceID;
		

			if (playerScript.bait1<cEndTime){
				//creature should be removed from DB and calculated for Presents and Mementos
				Debug.Log("Removed creature from you PlacedCreature list with ID: "+cID);
				CalculatePresents(cID);
			}

			//if CurrentBait is between StartTime and EndTime, it is visible
			if ((cStartTime >= playerScript.bait1) && (playerScript.bait1 <= cEndTime)) {
				//creature should be made visible
				Debug.Log ("There is a VISIBLE creature in PlacedCreature with ID: " + cID + " and SPOT is: " + cSpaceID);
				//will get coordinates for space, get image for creature/item combo, will place it in visual space
				//itemMenu.VisualSpace (cSpaceID, iID, cID);
				string tempItemCreatureImage= "ItemImages/C"+cID+"I"+iID;
				Sprite tempSprite2 = Resources.Load <Sprite>(tempItemCreatureImage);
				visualSpaces[cSpaceID-1].GetComponent<Image>().sprite=tempSprite2;
			} else {
				//safe to remove from local placedCreatureList because they are either over or currently invisible
				placedCreatureList.Remove(aCreature);
			}

		}

		////remove from the scene if placed? -- well actually we'll have a visual scene updater
		////that will only show the ones that are still in the placed creature list
	}

	public void RemoveCreatures (){
		//check startTimes on all PlacedCreatureList
		//if they have not yet started, remove them and nothing else for them
	}

	public void AdjustCreatureEndTime () {
		//adjust all remaining creatures in PlacedCreatureList
		//they will all end in 10 minutes from currentTime
	}


	public void ShowItemsToPlace () {
		TurnOnCanvasGroup(PlayerItemsCanvas);
		TurnOffCanvasGroup (SpaceCanvas);
		//clear old buttons first
		itemMenu.ClearOldButtons("playerItemPanel");
		//go through the PlayerItemT
		//if you have no items, then nothing to show
		//get the list of ones whose states are owned and/or placed [not NOT owned]
		//_playerItemsList = new List<PlayerItemsT> (from ps in dbManager.Table<PlayerItemsT> () where ps.ItemState==0 select ps);
		//string sql = "SELECT " + "PS.ItemID, " + "PS.ItemState, " + "T.Name AS Name " + "FROM PlayerItemsT PS " +
		             //"JOIN ItemsT T " + "ON PS.ItemID = T.ID "+ "WHERE PS.ItemState != -1";
		string sql = "SELECT " + "PS.ItemID, " + "PS.ItemState, " + "T.Name AS Name " + "FROM PlayerItemsT PS " +
			"JOIN ItemsT T " + "ON PS.ItemID = T.ID "+ "WHERE PS.ItemState != -1";
		_playerItemsList = new List<PlayerItemsTTemp> (dbManager.Query<PlayerItemsTTemp>(sql));
		//if you have one or more items, then display each as buttons to select
		//by calling ItemMenuS.MakePlayerItemMakeB(itemID);
			//from that method, buttons call ShowSpotsToPlace() and pass ItemID
		foreach (PlayerItemsTTemp anItem in _playerItemsList) {
			//debugText.text += "\nthe placed creature list has: " + anItem.ItemID+" with state of: "+anItem.ItemState;
			if(anItem.ItemState==0)
				itemMenu.MakePlayerItemPlaceB (anItem.ItemID, anItem.Name);
			if (anItem.ItemState == 1)
				itemMenu.MakePlayerItemRemoveB (anItem.ItemID, anItem.Name);
		}
	}
	public void ShowSpacesToPlace(int itemID) {
		//we are here if you hit button for itemMenu.MakePlayerItemPlaceB(itemID, name)
		//now we will display the Space screen 
		//turn off other canvases
		TurnOffCanvasGroup(PlayerItemsCanvas);
		TurnOnCanvasGroup (SpaceCanvas);
		//clear out old buttons
		itemMenu.ClearOldButtons("spacePanel");
		//with placed items and with highlighted Spaces [buttons]
		//grab the SpaceT and get IDs and States
		//if the State is empty, then show 0, if the state is filled, show X
		spaceList = new List<SpaceT> (from ps in dbManager.Table<SpaceT> () select ps);
		//Spaces are clickable buttons to call PlaceInSpace() passing ItemID and SpaceID
		foreach (SpaceT aSpace in spaceList) {
			//debugText.text += "\nthe placed creature list has: " + anItem.ItemID+" with name of: "+anItem.Name;
			itemMenu.MakeSpaceB (aSpace.ID, aSpace.Item, itemID);
		}
		Debug.Log ("called ShowSpotsToPlace: "+ itemID);
		/*
		List<PlacedCreatureT> listPCreatures = new List<PlacedCreatureT> (from ps in dbManager.Table<PlacedCreatureT> ()
			select ps);
		foreach (PlacedCreatureT pc in listPCreatures) {
			debugText.text += "Placed Creature: " + pc.CreatureID;
			//Debug.Log ("Placed Creature: " + pc.CreatureID);
		}
		*/
	}
	public void PlaceInSpace( int SpaceID, int Item, int newItem){
		Debug.Log ("Place in Space from clicking a space button with spaceID: " + SpaceID.ToString()+" and current Item was: "+Item.ToString()+" While NEW item to place is: "+newItem.ToString());
		//when a Spot has been clicked from the PlaceItem menu that means that you clicked on an item already
		//that item cannot already be placed or you wouldn't have it to click
		//the spot may be occupied already though
		if (Item == 0) {
			//no item was there
			Debug.Log ("no item in space");
		} else {
			Debug.Log ("removing current item to make room for new");
			//item was already in space -- TODO worry about large items with -1
			//start removing the item and doing calculations
			//call RemovePlacedItem(SpaceID, True or False) which will call RemovePlacedCreatures(CreatureID)
			RemovePlacedItem(SpaceID, Item, false);
		}
		//if here we have selected a spot and it is now free
		//TODO: if Item size from ItemT for ItemID is Large AND Space size from SpaceID is Small
		//then return;

		//else if Item is Small
		//it is time to add this itemID to this spaceID in SpaceT [remember to use 2 if L]
		SpaceT spaces = new SpaceT { ID = SpaceID, Item=newItem};
		Debug.Log ("Updated the sapce table with ID: " + SpaceID + " and Item: " + newItem);
		dbManager.UpdateTable(spaces);
		//then set the State of this Item in PlayerItemsT so that it is placed
		PlayerItemsT playeritems = new PlayerItemsT {ItemID=newItem,ItemState=1};
		dbManager.UpdateTable (playeritems);
		//calculate the player's placedCreatureList with newly placed item
		PlaceCreatures(newItem, SpaceID);
		//then go back to the playersItemList canvas and refresh
		ShowItemsToPlace();
	}
	public void RemovePlacedItem(int SpaceID, int Item, bool large) {
		Debug.Log ("called RemovePlacedItems on this space id: " + SpaceID.ToString ());
		//check to see if the spot is occupied, if not return;
		//if so, remove the item TODO [if 'large' you need to clear 2 spots/ 2 items]
		//remove item from SpaceT
		if (SpaceID == -1) {
			SpaceT tempspace = (from ps in dbManager.Table<SpaceT>() where ps.Item == Item select ps).FirstOrDefault ();
			SpaceID = tempspace.ID;
		}
		SpaceT spaces = new SpaceT { ID = SpaceID, Item=0};
		//Debug.Log ("Updated the sapce table with ID: " + SpaceID + " and Item as: " + newItem);
		dbManager.UpdateTable(spaces);
		//reset State value of item in PlayerItemsT
		PlayerItemsT playeritems = new PlayerItemsT {ItemID=Item,ItemState=0};
		dbManager.UpdateTable (playeritems);
		//when you remove an item, if monsters, calculate monster presents [not mementos]
			//this essentially also clears out currently visible and yet to be visible monsters on it
		//go through PlacedCreaturesT for the ItemID
		placedCreatureList = new List<PlacedCreatureT> (from ps in dbManager.Table<PlacedCreatureT> () where ps.ItemID==Item select ps);
		Debug.Log ("in RemovePlacedItems and going to see if there are any creatures assoc. with it.");
		foreach (PlacedCreatureT aCreature in placedCreatureList.ToList()) {
			CalculatePresents (aCreature.CreatureID);
			Debug.Log ("RemovedPlacedItem had this creature: " + aCreature.CreatureID);
		}
		//for each creature who was at the item
		//call CalculatePresents(CreatureID)
		//which will remove from the PlacedCreaturesT table
	}


	public void PlaceCreatures (int thisItemID, int thisSpace) {
		//if Bait1 and Bait2 are false, return
		//if Bait1 and/or Bait2 is true, but neither percentage is >= 25%, return
		Debug.Log("in PLACE CREATURES!!!");
		if (playerScript.bait1 <= 25)
			return;

		//loop through each item placed in scene: use SpaceT table

		//determine if item should have 0, 1, 2, or 3 creatures
		//Size= S or L : 0 or 1    Bait Type Quality can be 1, 2, 3
/*
B1 S [0 = 50%, 1 = 50%]
B1 L [0 = 30%, 1 = 35%, 2 = 20%, 3 = 15%]

B2 S [0 = 30%, 1 = 70%]
B2 L [0 = 10%, 1 = 45%, 2 = 30%, 3 = 15%]

B3 S [0 = 10%, 1 = 90%]
B3 L [0 = 5%, 1 = 35%, 2 = 35%, 3 = 25%]
*/
		int creaturesForItem = 0;
		int randnum = UnityEngine.Random.Range (1, 101);
		if (playerScript.bait1Q == 1) {
			if (randnum > 50)
				creaturesForItem = 1;
			else
				creaturesForItem = 0;
		}
		if (playerScript.bait1Q == 2) {
			if (randnum > 30 )
				creaturesForItem = 1;
			else
				creaturesForItem = 0;		
		}
		if (playerScript.bait1Q == 3) {
			if (randnum > 10)
				creaturesForItem = 1;
			else
				creaturesForItem = 0;
		}

		Debug.Log ("Creatures for Item: " + creaturesForItem);
		//if 0, return
			if (creaturesForItem == 0)
				return;
		//else, 1, 2, 3 so select creatures
		//Search all Creatures and get those who have the Item as FAV 1, 2, 3 && NOT placed in training area
		//QCArray = Qualifying Creature Array
		//if length < 6? 10? then search again for those that have a FAV1 type that matches this Item type
		//This Item Type is?
		List<ItemsT> itemInfo = new List<ItemsT> (from ps in dbManager.Table<ItemsT> () where ps.ID == thisItemID select ps);
		int itemType=itemInfo[0].TypeID;
		Debug.Log("T=his item id "+thisItemID+" has this type: "+itemType);

		while (creaturesForItem > 0) {
			//Rarity
			//choose rarity for this creature
			//rarityArray is 0-44 filled with 1, 45-74 filled with 2, 75-89 filled with 3, 90-99 filled with 4
			//choose random number 0-99 inclusive and get rarityLevel
			int thisRarity = 1;
			int RarityLvl = UnityEngine.Random.Range (1, 101);
			if (RarityLvl >= 44 && RarityLvl <= 73)
				thisRarity = 2;
			else if (RarityLvl >= 74 && RarityLvl <= 89)
				thisRarity = 3;
			else if (RarityLvl >= 90 && RarityLvl <= 100)
				thisRarity = 4;
			//EACH CREATURE
			//go through the MonsterT and select those who's FAVType==itemType
			//choose rarity for this creature
			//rarityArray is 0-44 filled with 1, 45-74 filled with 2, 75-89 filled with 3, 90-99 filled with 4
			List<MonsterT> qualifiedCreatureList = new List<MonsterT> (from ps in dbManager.Table<MonsterT> ()
			                                                         where ps.FavType == itemType && ps.Rarity == thisRarity
			                                                         select ps);
			foreach (MonsterT qMonster in qualifiedCreatureList.ToList()) {
				Debug.Log ("QUALIFIED MONSTER ID: " + qMonster.ID + "  Rarity is: " + qMonster.Rarity);
				//if qMonster.ID in PlacedCreatureT then REMOVE as an option
				PlacedCreatureT templist = (from ps in dbManager.Table<PlacedCreatureT>() where ps.CreatureID == qMonster.ID select ps).FirstOrDefault ();
				if (templist != null) {
					Debug.Log ("CREATURE ALREDY PLACED, SO REMOVED from list of qualified creatures");
					qualifiedCreatureList.Remove (qMonster);
				}
			}

			//if length of this shortList is NOT 0, select either random one or first one and return
			int qcListLen = qualifiedCreatureList.Count;

			//if length is 0 then choose either random or first from the QCArray instead
			//may need to move 'down' rarity, like if it was 3, to 2, and re-search and update shortList,
			//rather than just grab any creature at any rarity
			if (qcListLen == 0) {
				Debug.Log ("qualified creature list is EMPTY!"); //hopefully just out of this attempt at however many creatures you are trying place
				if(thisRarity==1)  //otherwise hopefully we'll place the creatures but they will all be common
					creaturesForItem--;
				break;
			}
		
			int randQC = UnityEngine.Random.Range (0, qcListLen);
			//creature to place is:
			MonsterT creatureToPlace = qualifiedCreatureList [randQC];

			//50% chance, if this selectedCreature == captured, select another one from shortList
			PlayerMonstersT checkCapt = (from ps in dbManager.Table<PlayerMonstersT> ()
			                            where ps.MonsterID == creatureToPlace.ID
			                            select ps).FirstOrDefault ();
			if (checkCapt != null) {
				if (checkCapt.MonsterState > 0) {
					Debug.Log ("MONSTER already captured! Try for another");
					randQC = UnityEngine.Random.Range (0, qcListLen);
					//creature to place is:
					creatureToPlace = qualifiedCreatureList [randQC];
				}
			}
			//ProcessPlacedCreature(offset?)
			//mark as placed in PlacedCreatureT, include this Item ID, calculate Start, End PlacedCreatureT
			//update visits in PlayerMonstersT -- NO not yet, do that when calculating Presents so you know they visited
			//calculate StartOffset and Duration
			//StartTime = startBait-StartOffset
			thisStartTime = playerScript.bait1 - StartOffset;
			//EndTime = startBait-startOffset-Duration
			thisEndTime = playerScript.bait1 - StartOffset - Duration;
			PlacedCreatureT placedCreatures = new PlacedCreatureT {
				CreatureID = creatureToPlace.ID,
				ItemID = thisItemID,
				StartTime = thisStartTime,
				EndTime = thisEndTime,
				SpaceID = thisSpace
			};
			dbManager.Insert (placedCreatures);
			Debug.Log ("You updated PlacedCreaturet by adding this Monster: " + placedCreatures.CreatureID);
			creaturesForItem--;
			Debug.Log ("End of class. did creaturesforitem--. Creatures who need to be placed still: " + creaturesForItem);
		}//end WHILE creaturestoplace > 0

		//repeat until creaturesNeededForItem == 0
	}

	public void CalculateDuration () {
		//three options for now
		bool Dur1, Dur2, Dur3; //30 min, 60 min, 90 min
		//add ⅓ of overall extra time to each if we increase the overall food length above 120 minutes]
		//based on current start bait
		int numOptions = 0;
		if (playerScript.bait1 >= 30) {
			Dur1 = true;
			numOptions++;
		}
		if (playerScript.bait1 >= 60) {
			Dur2 = true;
			numOptions++;
		}
		if (playerScript.bait1 >= 90) {
			Dur3 = true;
			numOptions++;
		}
		if (numOptions == 0)
			return;
		
		int[] DurArray;
		DurArray= new int[10] ;
		for (int i = 0; i < 10; i++) {
			DurArray [i] = 1;
		}
		if (numOptions == 2) {
			for (int i = 5; i < 10; i++) {
				DurArray [i] = 2;
			}
		}
		if (numOptions == 3) {
			for (int i = 4; i < 7; i++) {
				DurArray [i] = 2;
			}
			for (int i = 7; i < 10; i++) {
				DurArray [i] = 3;
			}
		}
		int RanDur = UnityEngine.Random.Range (0, 11);
		if (DurArray [RanDur] == 1)
			Duration = 30;
		else if (DurArray [RanDur] == 2)
			Duration = 60;
		else if (DurArray [RanDur] == 3)
			Duration = 90;
	}

	public void CalculateStartOffset () {
		// 0, 30, 60, 90 based on duration and start bait
		//1 = 0 minutes; 2=30 minutes; 3=60minutes; 4=90 minutes
		//Duration = 90 then half 1s half 2s
		//Duration = 60 4 1s; 3 2s; 3 3s;
		//Duration = 30 3 1s; 3 2s; 2 3s; 2 4s
		int [] StartArray;
		StartArray = new int[10];

		for (int i = 0; i < 5; i++) {
			StartArray [i] = 1;
		}
		for (int i = 5; i < 10; i++) {
			StartArray [i] = 2;
		}


		if (Duration == 60) {
			for (int i = 0; i < 4; i++) {
				StartArray [i] = 1;
			}
			for (int i = 4; i < 7; i++) {
				StartArray [i] = 2;
			}
			for (int i = 7; i < 10; i++) {
				StartArray [i] = 3;
			}
		}

		if (Duration == 90) {
			for (int i = 0; i < 3; i++) {
				StartArray [i] = 1;
			}
			for (int i = 3; i < 6; i++) {
				StartArray [i] = 2;
			}
			for (int i = 6; i < 8; i++) {
				StartArray [i] = 3;
			}
			for (int i = 8; i < 10; i++) {
				StartArray [i] = 4;
			}
		}
		int RanDur = UnityEngine.Random.Range (0, 11);
		if (StartArray [RanDur] == 1)
			StartOffset = 0;
		else if (StartArray [RanDur] == 2)
			StartOffset = 30;
		else if (StartArray [RanDur] == 3)
			StartOffset = 60;
		else if (StartArray [RanDur] == 4)
			StartOffset = 90;
	}

	public void CalculatePresents (int CreatureID) {
		Debug.Log ("Calculating Presents");
		//totLesser and totUpper are global vars
		int totLesser, totUpper;
		int rarityLvl=1;
		//fill in their values by getting them from the MonsterT with the CreatureID
		MonsterT tempRare = (from ps in dbManager.Table<MonsterT> () where ps.ID == CreatureID select ps).FirstOrDefault();
		rarityLvl=tempRare.Rarity;
		//Debug.Log ("CALCULATE PRESENTS: rarity is now " + rarityLvl);
		//fill in EndTime and StartTime by searching PlacedCreatureT with CreatureID
		PlacedCreatureT tempPlaced = (from ps in dbManager.Table<PlacedCreatureT> () where ps.CreatureID == CreatureID select ps).FirstOrDefault();
		int tempStartTime = tempPlaced.StartTime;
		int tempEndTime = tempPlaced.EndTime;
		int tempItem = tempPlaced.ItemID;
		//its removal will not happen until the end of this
		totLesser=(((tempEndTime-tempStartTime) * 10 + UnityEngine.Random.Range(1,50)) +CreatureID)/ (5-rarityLvl);
		//30 lesser bait = 1 upper bait, so divide by 30 to get how many upper ones
		totUpper=totLesser/30;
		//totLesser= remainder of totLesser/30
			totLesser=totLesser%30;
		Debug.Log("Upper Coins: " + totUpper + " Lower Coins: "+totLesser);

		//playerScript.SilverCoins += totLesser; //these get added to AFTER player accepts from PresentsToAcceptT TODO make another method
		//playerScript.GoldCoins += totUpper;
		//add CreatureID, tempItem, totLesser, totUpper to PresentToAcceptT 
		PresentsToAcceptT presents = new PresentsToAcceptT {
			CreatureID = CreatureID,
			ItemID = tempItem,
			Silver = totLesser,
			Gold = totUpper,
		};
		dbManager.Insert (presents);

		//remove this CreatureID from PlacedCreatureT
		//PlacedCreatureT templist = (from ps in dbManager.Table<PlacedCreatureT>() where ps.CreatureID == CreatureID select ps).FirstOrDefault ();
		dbManager.Delete<PlacedCreatureT>(tempPlaced);
			Debug.Log ("CREATURE REMOVED from PlacedCreatureL after Presents calculated");
		//update visits in PlayerMonstersT 
		int newVisits;
		PlayerMonstersT tempMon = (from ps in dbManager.Table<PlayerMonstersT> ()
			where ps.MonsterID == CreatureID
			select ps).FirstOrDefault ();
		newVisits = tempMon.MonsterState;
		if (newVisits <= 0)
			newVisits -= 1;
		else
			newVisits += 1;

		PlayerMonstersT playerMon = new PlayerMonstersT{MonsterID=CreatureID,MonsterState=newVisits};
		dbManager.UpdateTable(playerMon);
	}

	public bool CalculateMemento (int CreatureID) {
		//search for CreatureID in PlayersCreaturesT
		//creature is Captured must be true 
		//visit for creature > 30
		//no memento for creature already
		//chance 1 out of 10, that memento will be given
		return false;
	}
}
