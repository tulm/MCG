using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using SimpleSQL;

public class FormulaS : MonoBehaviour {
	public PlayerS playerScript;
	public GameManager gameManager;
	public ItemMenuS itemMenu;

	public CanvasGroup SpaceCanvas;
	public CanvasGroup PlayerItemsCanvas;
	public CanvasGroup TrainingCanvas;
	public CanvasGroup PresentsCanvas;

	public Transform PresentsPanel;
	public GameObject imagePrefab;
	public Text debugText;

	private List<PlacedCreatureT> placedCreatureList;
	private List<MonsterT> _monsterList;
	private List<SpaceT> spaceList;
	private List<PlayerItemsTTemp> _playerItemsList;

	public int Duration;
	public int StartOffset;
	public int thisStartTime;
	public int thisEndTime;
	const int B1START = 240;

	public GameObject [] visualSpaces;

	// reference to our db manager object
	public SimpleSQL.SimpleSQLManager dbManager;
	 
	void Start () {
		dbManager = GameObject.Find ("DB Manager").GetComponent<SimpleSQL.SimpleSQLManager> ();
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

	public void PrintCreaturesToPlace() {
		List<PlacedCreatureT>placedCreatureList2 = new List<PlacedCreatureT> (from ps in dbManager.Table<PlacedCreatureT> () select ps);
		debugText.text = "";
		foreach (PlacedCreatureT aCreature in placedCreatureList2.ToList()) {
			Debug.Log ("PlacedCreatureT ID: " + aCreature.CreatureID+" Start: "+aCreature.StartTime+" End: "+aCreature.EndTime);
			debugText.text+="\n PlacedCreatureT ID: " + aCreature.CreatureID+" Start: "+aCreature.StartTime+" End: "+aCreature.EndTime;
		}
	}

	public void DebugListCaptCreatures () {
		List<PlayerMonstersT>playerMonsters = new List<PlayerMonstersT> (from ps in dbManager.Table<PlayerMonstersT> () where ps.MonsterState>0 select ps);
		debugText.text = "";
		foreach (PlayerMonstersT aCreature in playerMonsters.ToList()) {
			Debug.Log ("In List of CAPTURED Creatures, creature with ID: " + aCreature.MonsterID);
			debugText.text+="\n Capt Creature ID: " + aCreature.MonsterID;
		}
	}

	public void AdjustCreatureEndTime () {
		//adjust all remaining creatures in PlacedCreatureList
		//they will all end in 10 minutes from currentTime
	}

	public void CaptureCreature (int CreatureID) {
		//you clicked on a creature in the Training Area
		//you can capture it here [taking a picture or sharing it only happens in our special Cage area, not here]
		//you capture it by tapping on it, which triggers this function
		//this should get the creature ID and then add this creature to your captured list
		//PlayerMonstersT find the creatureID
		//Debug.Log ("Hit a Creature to Capture and ID was: " + CreatureID);
		PlayerMonstersT tempCreature = (from ps in dbManager.Table<PlayerMonstersT> () where ps.MonsterID == CreatureID select ps).FirstOrDefault ();
		//if >0 nothing -- return
		//if <0 change to positive *-1
		//if 0, change to 1 [default]
		int tempState=1;
		if (tempCreature.MonsterState > 0) {
			//Debug.Log ("In Capture: ALREADY CAPTURED");
			return;
		}
		if (tempCreature.MonsterState < 0) {
			tempState = tempCreature.MonsterState * -1;
			//Debug.Log("In Capture: you have caught a monster that had visited before with ID:" +CreatureID+" and visits: "+tempCreature.MonsterState);
		}
		//now update the state in our PlayerMonstersT table
		PlayerMonstersT playerMons = new PlayerMonstersT {MonsterID=tempCreature.MonsterID,MonsterState=tempState};
		dbManager.UpdateTable (playerMons);

		//TODO we have some issues -- when we click, do we show them to the cages and they have to put it in an EMPTY cage? Can they remove a current creature
		//and replace it? Does that UNCAPTURE the removed creature? YES, I would say so, so that you have to end up buying all the cages you need and you could do a
		//strategy thing and only capture the rare ones first, then get the ones you know will come more often after you have the money to buy all the cages
		//Currently going with you can only place it in an empty cage of the right type. [so you could run and buy one and reclick on creature to capture it]
	}

	public void CalcBait () {
		//based on time, bait gets set to a percentage chunk
		//check time since bait was placed
		double baitPassTime=0;

		if (playerScript.bait1B) {
			Debug.Log ("Bait Start Value: "+playerScript.bait1StartValSecs);
			playerScript.bait1StartValSecs -= gameManager.minimizedSeconds;
			baitPassTime = playerScript.bait1StartValSecs/60;
			Debug.Log ("Bait minus min seconds and divided by 60: "+ baitPassTime);

			if (baitPassTime <= 0) {
				//debugText.text += "\nFormulas: bait1 was less than one so empty, put to 0";
				Debug.Log("CalcBait: bait1 was less than one so empty, put to 0");
				playerScript.bait1 = 0;
				playerScript.bait1B = false;
			}
			if (baitPassTime > 0 && baitPassTime < 1) {
				//debugText.text += "\nFormulas: bait1 there between 1 and 2 minutes, cut to 25";
				Debug.Log("CalcBait: bait1 there between 1 and 2 minutes, cut to 25");
				playerScript.bait1 = 25;
			}
			if (baitPassTime > 1 && baitPassTime < 2) {
				//debugText.text += "\nFormulas: bait1 there between 2 and 3 minutes, cut to 50";
				Debug.Log("CalcBait: bait1 there between 2 and 3 minutes, cut to 50");
				playerScript.bait1 = 50;
			}
			if (baitPassTime > 2 && baitPassTime < 3) {
				//debugText.text += "\nFormulas: bait1 there between 3 and 4 minutes, cut to 75";
				Debug.Log("CalcBait: bait1 there between 3 and 4 minutes, cut to 75");
				playerScript.bait1 = 75;
			}
			if (baitPassTime >= 3) {
				//debugText.text += "\nFormulas: bait1 is still full at 4 minutes";
				Debug.Log("CalcBait: bait1 is still full at 4 minutes");
				playerScript.bait1 = 100;
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
		playerScript.Save ();
	}
		
	public void UpdateSpace () {
		/*need Placed creature db access AND current bait values
		need to go through all placed creatures and calculate those that have end times that have passed
		if ended, calculate presents, memento, remove from PlacedCreature list
		*/
		int cID, iID, cStartTime, cEndTime, cSpaceID;
	

		//Visually fill in the Spaces with the EMPTY ITEM images FIRST -- then 'add' the images for currently visible creatures
		List <SpaceT> spaces = new List<SpaceT> (from ps in dbManager.Table<SpaceT> () orderby ps.ID select ps);

		foreach (SpaceT aSpace in spaces) {
			/*get the coordinates you need for this space
			you will have a link to visual image object transforms in the scene so that you can replace their images
			it will be an ordered array so that it matches the SpaceT db which is returned ordered
			get the image you need for this item using itemID
			*/
			//TODO will be more than one for items that can hold more than 0 or 1 creatures

			string tempSpaceImageName = "ItemImages/i"+aSpace.Item;
			Sprite tempSprite = Resources.Load<Sprite>(tempSpaceImageName);
			//place the image in the visual space
			visualSpaces[aSpace.ID-1].GetComponent<Image>().sprite = tempSprite;
			//remove previously placed listeners on Space Buttons -- for clicking to capture creatures
			visualSpaces[aSpace.ID-1].GetComponentInChildren<Button>().onClick.RemoveAllListeners();
		} //END foreach space
			
		//Now fill spaces that have Items AND Creatures
		placedCreatureList = new List<PlacedCreatureT> (from ps in dbManager.Table<PlacedCreatureT> () select ps);

		foreach (PlacedCreatureT aCreature in placedCreatureList.ToList()) {
			//debugText.text += "\nthe placed creature list has: " + aCreature.CreatureID;
			Debug.Log("One of all PlacedCreatures ATM: ID: "+aCreature.CreatureID);
			cID = aCreature.CreatureID;
			iID = aCreature.ItemID;
			cStartTime = aCreature.StartTime;
			cEndTime = aCreature.EndTime;
			cSpaceID = aCreature.SpaceID;
			int i = cID;
/*due to CRAZY delegate issue with creating the buttons on the fly
Is the parameter passed into the delegate stored as a reference value?
Not quite. It's actually being captured in a "closure" that contains all that is needed for the delegate/anonymous method to be called at a later time.
It's as if the variable was silently converted to a field in an object that was then silently passed to the delegates/anonymous methods 
when they were finally invoked.
So create this local, in loop "i", to be captured by the closure every time, so its value is never modified.
*/

			//creature should be removed from DB and calculated for Presents and Mementos
			if (playerScript.bait1StartValSecs<cEndTime){  //TENU - playerScript.bait1 or playerScript.bait1StartValSecs
				Debug.Log("TIME OVER: Removed creature from PlacedCreatureList ID: "+cID);
				CalculatePresents(cID);
			}
			//if CurrentBait is between StartTime and EndTime, it is visible
			//will get coordinates for space, get image for creature/item combo, will place it in visual space
			else if ((cStartTime >= playerScript.bait1StartValSecs) && (playerScript.bait1StartValSecs >= cEndTime)) { //TENU - playerScript.bait1 or playerScript.bait1StartValSecs
				Debug.Log ("TIME CURRENT: There is a VISIBLE creature in PlacedCreature with ID: " + cID + " and SPOT is: " + cSpaceID);
				string tempItemCreatureImage= "ItemImages/C"+cID+"I"+iID;
				Sprite tempSprite2 = Resources.Load <Sprite>(tempItemCreatureImage);
				visualSpaces[cSpaceID-1].GetComponent<Image>().sprite=tempSprite2;
				visualSpaces[cSpaceID-1].GetComponentInChildren<Button> ().onClick.AddListener (
					() => {CaptureCreature(i);});
			} else {
				//safe to remove from LOCAL placedCreatureList because they are either over or currently invisible
				Debug.Log("TIME FUTURE: There is a placedCreature but time not yet, ID: "+cID);
				placedCreatureList.Remove(aCreature);
			}
		} //END foreach placedCreature
	}
		
	public void ShowItemsToPlace () {
		//turn off other canvases, menus
		TurnOnCanvasGroup(PlayerItemsCanvas);
		TurnOffCanvasGroup (TrainingCanvas);

		//clear old buttons first
		itemMenu.ClearOldButtons("playerItemPanel");
	
		/* go through the PlayerItemT
		if you have no items, then nothing to show
		get the list of ones whose states are owned and/or placed [not NOT owned]
		*/
		string sql = "SELECT " + "PS.ItemID, " + "PS.ItemState, " + "T.Name AS Name " + "FROM PlayerItemsT PS " +
			"JOIN ItemsT T " + "ON PS.ItemID = T.ID "+ "WHERE PS.ItemState != -1";
		_playerItemsList = new List<PlayerItemsTTemp> (dbManager.Query<PlayerItemsTTemp>(sql));

		/* if you have one or more items, then display each as buttons to select
		by calling ItemMenuS.MakePlayerItemMakeB(itemID);
		from that method, buttons call ShowSpotsToPlace() and pass ItemID
		*/
		foreach (PlayerItemsTTemp anItem in _playerItemsList) {
			if(anItem.ItemState==0)
				itemMenu.MakePlayerItemPlaceB (anItem.ItemID, anItem.Name);
			if (anItem.ItemState == 1)
				itemMenu.MakePlayerItemRemoveB (anItem.ItemID, anItem.Name);
		}
	}

	public void ShowSpacesToPlace(int itemID) {
		/* we are here if you hit button for itemMenu.MakePlayerItemPlaceB(itemID, name)
		now we will display the Space screen 
		*/
		//turn off other canvases, menus
		TurnOffCanvasGroup(PlayerItemsCanvas);
		TurnOnCanvasGroup (SpaceCanvas);

		//clear out old buttons
		itemMenu.ClearOldButtons("spacePanel");

		/* with placed items and with highlighted Spaces [buttons]
		grab the SpaceT and get IDs and States
		if the State is empty, then show 0, if the state is filled, show X
		*/
		spaceList = new List<SpaceT> (from ps in dbManager.Table<SpaceT> () select ps);
		//Spaces are clickable buttons to call PlaceInSpace() passing ItemID and SpaceID
		foreach (SpaceT aSpace in spaceList) {
			itemMenu.MakeSpaceB (aSpace.ID, aSpace.Item, itemID);
		}
		//Debug.Log ("called ShowSpotsToPlace: "+ itemID);
	}

	public void PlaceInSpace( int SpaceID, int Item, int newItem){
		//Debug.Log ("Place in Space from clicking a space button with spaceID: " + SpaceID.ToString()+" and current Item was: "+Item.ToString()+" While NEW item to place is: "+newItem.ToString());
		/* when a Spot has been clicked from the PlaceItem menu that means that you clicked on an item already
		that item cannot already be placed or you wouldn't have it to click
		the spot may be occupied already though
		*/
		if (Item != 0) {
			Debug.Log ("removing current item to make room for new");
			//item was already in space -- TODO worry about large items with -1
			//start removing the item and doing calculations
			//call RemovePlacedItem(SpaceID, True or False) which will call RemovePlacedCreatures(CreatureID)
			RemovePlacedItem(SpaceID, Item, false);
		}
		//TODO: if Item size from ItemT for ItemID is Large AND Space size from SpaceID is Small
		//then return;

		//if here we have selected a spot and it is now free
		//it is time to add this itemID to this spaceID in SpaceT [remember to use 2 if L]
		//Debug.Log ("Updated the sapce table with ID: " + SpaceID + " and Item: " + newItem);
		SpaceT spaces = new SpaceT { ID = SpaceID, Item=newItem};
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
		//if so, remove the item from SpaceT
		//TODO [if 'large' you need to clear 2 spots/ 2 items]
		if (SpaceID == -1) {
			SpaceT tempspace = (from ps in dbManager.Table<SpaceT>() where ps.Item == Item select ps).FirstOrDefault ();
			SpaceID = tempspace.ID;
		}
		//Debug.Log ("Updated the space table with ID: " + SpaceID + " and Item as: " + newItem);
		SpaceT spaces = new SpaceT { ID = SpaceID, Item=0};
		dbManager.UpdateTable(spaces);

		//reset State value of item in PlayerItemsT
		PlayerItemsT playeritems = new PlayerItemsT {ItemID=Item,ItemState=0};
		dbManager.UpdateTable (playeritems);

		/* when you remove an item, if monsters, calculate monster presents [not mementos]
		this essentially also clears out currently visible and yet to be visible monsters on it
		go through PlacedCreaturesT for the ItemID
		*/
		placedCreatureList = new List<PlacedCreatureT> (from ps in dbManager.Table<PlacedCreatureT> () where ps.ItemID==Item select ps);
		Debug.Log ("in RemovePlacedItems and going to see if there are any creatures assoc. with it.");
		foreach (PlacedCreatureT aCreature in placedCreatureList.ToList()) {
			int cID = aCreature.CreatureID;
			//int iID = aCreature.ItemID;  //not used??
			int cStartTime = aCreature.StartTime;
			int cEndTime = aCreature.EndTime;
			int cSpaceID = aCreature.SpaceID;

			Debug.Log ("RemovedPlacedItem had this creature: " + aCreature.CreatureID);

			//creature should be removed from DB and calculated for Presents
			if (playerScript.bait1StartValSecs<cEndTime){ //TENU - playerScript.bait1 or playerScript.bait1StartValSecs
				Debug.Log("This should NOT be being called. ID: "+cID);
				CalculatePresents(cID);
			}
			//if CurrentBait is between StartTime and EndTime, it is visible
			if ((cStartTime >= playerScript.bait1StartValSecs) && (playerScript.bait1StartValSecs >= cEndTime)) {  //TENU - playerScript.bait1 or playerScript.bait1StartValSecs
				//TODO creature is visible -- should cut off some of the Presents maybe?
				Debug.Log ("We RemovedPlacedItem and there were Visible creatures to remove with ID: " + cID + " and SPOT is: " + cSpaceID);
				CalculatePresents(cID);
			} else {
				//any remaining creatures have not shown up yet and so since item being removed, creature should be removed but no Presents given
				dbManager.Delete<PlacedCreatureT>(aCreature);
				Debug.Log ("We RemovedPlacedItem and Creature removed, but No Presents Calculated");
			}
		} //End foreach placed creature at item

		//then refresh the playersItemList
		ShowItemsToPlace();
	}
		
	public void PlaceCreatures (int thisItemID, int thisSpace) {
		Debug.Log("in PLACE CREATURES!!!");
		//if Bait1 and Bait2 are false, return
		//OR if Bait1 and/or Bait2 is true, but neither percentage is >= 25%, return
		if (playerScript.bait1 <= 25 ) {
			Debug.Log("but bait less than 25% or bait1B is false -- so return");
			return;
		}
			
		//determine if item should have 0, 1, 2, or 3 creatures
		int creaturesForItem = 0;

		/*
		Size= S or L : 0,1  or 0,1,2,3    Bait Type Quality can be 1, 2, 3
		BQ=1 S [0 = 50%, 1 = 50%]
		BQ=1 L [0 = 30%, 1 = 35%, 2 = 20%, 3 = 15%]

		BQ=2 S [0 = 30%, 1 = 70%]
		BQ=2 L [0 = 10%, 1 = 45%, 2 = 30%, 3 = 15%]

		BQ=3 S [0 = 10%, 1 = 90%]
		BQ=3 L [0 = 5%, 1 = 35%, 2 = 35%, 3 = 25%]
		*/
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
		Debug.Log ("PlaceCreatures: Num of creatures for Item: " + creaturesForItem);

		if (creaturesForItem == 0)
			return;
		//else, 1, 2, 3 so select creatures

		//get the item TYPE from the item to be placed
		List<ItemsT> itemInfo = new List<ItemsT> (from ps in dbManager.Table<ItemsT> () where ps.ID == thisItemID select ps);
		int itemType=itemInfo[0].TypeID;
		Debug.Log("This item id "+thisItemID+" has this type: "+itemType);

		/* DO this for EACH CREATURE you need to place now that you have num needed and item and itemtype
		Search all Creatures and get those who have the same Item type as their FAVTYPE && Rarity matches randomly selected && NOT already placed in training area
		if length < 6? 10? then search again and add all Rarity levels?
		QualifiedCreatureList = Qualifying Creature Array
		*/
		while (creaturesForItem > 0) {
			//Rarity: choose rarity for this creature
			//thisRarity is: 0-44 filled with 1, 45-74 filled with 2, 75-89 filled with 3, 90-99 filled with 4
			int thisRarity = 1;
			int RarityLvl = UnityEngine.Random.Range (1, 101);
			if (RarityLvl >= 44 && RarityLvl <= 73)
				thisRarity = 2;
			else if (RarityLvl >= 74 && RarityLvl <= 89)
				thisRarity = 3;
			else if (RarityLvl >= 90 && RarityLvl <= 100)
				thisRarity = 4;
			
			//go through the MonsterT and select those who's FAVType==itemType and Rarity is a match
			List<MonsterT> qualifiedCreatureList = new List<MonsterT> (from ps in dbManager.Table<MonsterT> ()
			                                                         where ps.FavType == itemType && ps.Rarity == thisRarity
			                                                         select ps);
			foreach (MonsterT qMonster in qualifiedCreatureList.ToList()) {
				Debug.Log ("QUALIFIED MONSTER ID: " + qMonster.ID + "  Rarity is: " + qMonster.Rarity);
				//if qMonster.ID in PlacedCreatureT already then REMOVE as an option
				PlacedCreatureT templist = (from ps in dbManager.Table<PlacedCreatureT>() where ps.CreatureID == qMonster.ID select ps).FirstOrDefault ();
				if (templist != null) {
					Debug.Log ("CREATURE ALREDY in a Space, SO REMOVED from list of qualified creatures");
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
				if (thisRarity == 1)  //otherwise hopefully we'll place the creatures but they will all be common
					creaturesForItem--;
				
			} else {
				
			/* you have 1 or more qualified, non-placed creatures.
			  So randomly choose one,
			  try again if you already captured it, 
			  calc Start and End, 
			  and place
			 */
				//randomly choose one
				int randQC = UnityEngine.Random.Range (0, qcListLen);
				MonsterT creatureToPlace = qualifiedCreatureList [randQC];

				//50% chance, if this selectedCreature == captured, select another one from shortList
				PlayerMonstersT checkCapt = (from ps in dbManager.Table<PlayerMonstersT> ()
				                            where ps.MonsterID == creatureToPlace.ID
				                            select ps).FirstOrDefault ();
				if (checkCapt != null) {
					if (checkCapt.MonsterState > 0) {
						Debug.Log ("MONSTER already captured! Try for another");
						randQC = UnityEngine.Random.Range (0, qcListLen);
						creatureToPlace = qualifiedCreatureList [randQC];
					}
				}
					
				//Mark as placed in PlacedCreatureT, include this Item ID, calculate Start, End
				//update visits in PlayerMonstersT -- NO not yet, do that when calculating Presents so you know they visited
				//calculate Duration and StartOffset
				CalculateDuration();
				CalculateStartOffset();

				//calculate start and end times
				int tempTime=B1START;  //if playerScript.bait1 == 100

				if (playerScript.bait1 == 75)
					tempTime = (int)(B1START * .75);
				if (playerScript.bait1 == 50)
					tempTime = (int)(B1START * .50);
			
				thisStartTime = tempTime - StartOffset;  
				thisEndTime = thisStartTime - Duration; 
				PlacedCreatureT placedCreatures = new PlacedCreatureT {
					CreatureID = creatureToPlace.ID,
					ItemID = thisItemID,
					StartTime = thisStartTime,
					EndTime = thisEndTime,
					SpaceID = thisSpace
				};
				dbManager.Insert (placedCreatures);
				creaturesForItem--;

				Debug.Log ("In PlaceCreatures, Monster: " + placedCreatures.CreatureID+" w StartTime: "+thisStartTime+" w Endtime: "+thisEndTime);
				Debug.Log ("End of class. did creaturesforitem--. Creatures who need to be placed still: " + creaturesForItem);
			}//end of if QualifiedCreaturesList >0
		}//end WHILE creaturestoplace > 0 -- repeat until creaturesNeededForItem == 0
	}

	public void CalculateDuration () {
		//three options for now: 30 min, 60 min, 90 min
		//add ⅓ of overall extra time to each if we increase the overall food length above 120 minutes]
		//based on current start bait
		int numOptions = 0;
		if (playerScript.bait1StartValSecs >= 30) {  //TENU - playerScript.bait1 or playerScript.bait1StartValSecs
			numOptions++;
		}
		if (playerScript.bait1StartValSecs >= 60) { //TENU - playerScript.bait1 or playerScript.bait1StartValSecs
			numOptions++;
		}
		if (playerScript.bait1StartValSecs >= 90) { //TENU - playerScript.bait1 or playerScript.bait1StartValSecs
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
		int RanDur = UnityEngine.Random.Range (0, 10);
		if (DurArray [RanDur] == 1)
			Duration = 30;
		else if (DurArray [RanDur] == 2)
			Duration = 60;
		else if (DurArray [RanDur] == 3)
			Duration = 90;

		Debug.Log ("CalculateDuration which is: " + Duration);
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
		else if (Duration == 30 || Duration ==0) {
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

		int RanDur = UnityEngine.Random.Range (0, 10);
		if (StartArray [RanDur] == 1)
			StartOffset = 0;
		else if (StartArray [RanDur] == 2)
			StartOffset = 30;
		else if (StartArray [RanDur] == 3)
			StartOffset = 60;
		else if (StartArray [RanDur] == 4)
			StartOffset = 90;

		Debug.Log ("CalculateStartOffset which is: " + StartOffset);
	}

	public void CalculatePresents (int CreatureID) {
		Debug.Log ("Calculating Presents for creature ID: "+CreatureID);
		int totLesser, totUpper;
		int rarityLvl=1;

		//fill in their values by getting them from the MonsterT with the CreatureID
		MonsterT tempRare = (from ps in dbManager.Table<MonsterT> () where ps.ID == CreatureID select ps).FirstOrDefault();
		rarityLvl=tempRare.Rarity;

		//fill in EndTime and StartTime by searching PlacedCreatureT with CreatureID
		PlacedCreatureT tempPlaced = (from ps in dbManager.Table<PlacedCreatureT> () where ps.CreatureID == CreatureID select ps).FirstOrDefault();
		int tempStartTime = tempPlaced.StartTime;
		int tempEndTime = tempPlaced.EndTime;
		int tempItem = tempPlaced.ItemID;    //its removal will not happen until the end of this

		totLesser=(((tempStartTime-tempEndTime) * 10 + UnityEngine.Random.Range(1,50)) +CreatureID)/ (5-rarityLvl);
		//30 lesser bait = 1 upper bait, so divide by 30 to get how many upper ones
		totUpper=totLesser/30;
		//totLesser= remainder of totLesser/30 -- TODO or maybe if you get any upper coins, you just don't get any lesser ones!!!!!!
		totLesser=totLesser%30;
		Debug.Log("Upper Coins: " + totUpper + " Lower Coins: "+totLesser);

		//add CreatureID, tempItem, totLesser, totUpper to PresentToAcceptT 
		PresentsToAcceptT presents = new PresentsToAcceptT {
			CreatureID = CreatureID,
			ItemID = tempItem,
			Silver = totLesser,
			Gold = totUpper,
		};
		dbManager.Insert (presents);

		//remove this CreatureID from PlacedCreatureT
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

	public void ShowPresents () {
		/* if here then show presents screen has been activated
		this means you should be showing a list of all the presents now
		and an AcceptPresents() button to call AcceptPresents
		*/

		itemMenu.ClearOldButtons ("presentsPanel");

		//showing from PresentsToAcceptT table: Creature Image, Item Image, Lesser Given, Upper Given
		//for now, just show Creature ID and Item ID, Lesser and Upper given 
		List<PresentsToAcceptT> tempPresents = new List<PresentsToAcceptT> (from ps in dbManager.Table<PresentsToAcceptT> () select ps);


		foreach (PresentsToAcceptT tempPresent in tempPresents.ToList()) {
			Debug.Log ("Present from ID: " + tempPresent.CreatureID + "  Item: " + tempPresent.ItemID + " Silver: " + tempPresent.Silver + " Gold: " + tempPresent.Gold);
			//create a new UI element -- in this case we will do the monster and then TODO a text box with the other 3 pieces of information
			GameObject creatureImage = (GameObject)Instantiate (imagePrefab);
			creatureImage.transform.SetParent (PresentsPanel, false);

			string tempCreatureImage= "BaseMonster/c"+tempPresent.CreatureID;
			Sprite tempSprite2 = Resources.Load <Sprite>(tempCreatureImage);
			creatureImage.GetComponent<Image>().sprite=tempSprite2;
		}
		//and of course show the Accept button -- accept button part of the Presents Canvas Group
	}

	public void AcceptPresents () {
		//if called, clicked button and should add up all presents and then clear out PresentsToAcceptT table
		//be sure to update Silver and Gold and save them and update their text
		List<PresentsToAcceptT> tempPresents = new List<PresentsToAcceptT> (from ps in dbManager.Table<PresentsToAcceptT> () select ps);

		foreach (PresentsToAcceptT tempPresent in tempPresents.ToList()) {
			playerScript.SilverCoins += tempPresent.Silver;
			playerScript.GoldCoins += tempPresent.Gold;
			dbManager.Delete<PresentsToAcceptT>(tempPresent);
		}
		playerScript.menuShown = 1;
		playerScript.sceneloaded = false;
	}
}
