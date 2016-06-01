using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemMenuS : MonoBehaviour {
	public FormulaS formulas;
	public GameObject buttonPrefab;
	public GameObject removeButtonPrefab;
	public Transform playerItemPanel;
	public Transform spacePanel;
	//public PlayerS playerScript;

	// Use this for initialization
	void Start () {
	}

	public void VisualSpace(int spaceid, int itemid, int creatureid) {
		//there is only one image or at least one 'portion' on an item where this creature's image can be, for every Creature at each item,
		//so if you have the space, the item, and the creature HERE, you know it is to be displayed and can find the image. you also now know the location for the image based on Space
		//maybe fill in a space array here with images
		//have array of spaces
		//go to one based on spacid
		//get the image you want based on itemid and creatureid
		//set the space image based on the string you just made
		//currently all done in Formula.UpdateSpace()

	}
	/*
	public void LoadLevel (string LvlName) {
		playerScript.sceneloaded = false;
		//load the item inventory scene
		Application.LoadLevel(LvlName);
	}
*/
	public void ClearOldButtons (string panelName){
		Transform CanvasPanel=playerItemPanel;
		if (panelName == "playerItemPanel")
			CanvasPanel = playerItemPanel;
		else if (panelName == "spacePanel")
			CanvasPanel = spacePanel;
		foreach (Transform child in CanvasPanel)
		{
			Destroy (child.gameObject);
		}
	}
	public void MakePlayerItemPlaceB(int itemID, string itemName)
	{
		GameObject button = (GameObject)Instantiate (buttonPrefab); //TENU hiding for attempts at Android resource bs
		//GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/buttonPrefab" ) )as GameObject;
		//GameObject button = (GameObject)Instantiate (bP);
		button.GetComponentInChildren<Text> ().text = itemName;
		button.GetComponentInChildren<Button> ().onClick.AddListener (
			() => {formulas.ShowSpacesToPlace (itemID);}
		);
		//button.transform.parent = menuPanel;	
		button.transform.SetParent (playerItemPanel, false);
		int offset = playerItemPanel.transform.childCount;
		offset *= 10;
		button.transform.Translate (0, offset, 0);
	}

	public void MakePlayerItemRemoveB(int itemID, string itemName)
	{
		GameObject button = (GameObject)Instantiate (removeButtonPrefab);
		//GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/removeButtonPrefab" ) )as GameObject;
		//GameObject button = (GameObject)Instantiate (removeBP);
		button.GetComponentInChildren<Text> ().text = itemName;
		button.GetComponentInChildren<Button> ().onClick.AddListener (
			() => {formulas.RemovePlacedItem (-1, itemID, false);}
		);
		//button.transform.parent = menuPanel;	
		button.transform.SetParent (playerItemPanel, false);
		int offset = playerItemPanel.transform.childCount;
		offset *= 10;
		button.transform.Translate (0, offset, 0);
	}
	public void MakeSpaceB(int spaceID, int item, int newItem)
	{
		Debug.Log ("made it to make a space button for all spaces in spaceT db");
		GameObject button = (GameObject)Instantiate (buttonPrefab);
		//GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/buttonPrefab" ) )as GameObject;
		//GameObject button = (GameObject)Instantiate (bP);
		button.GetComponentInChildren<Text> ().text = item.ToString();
		button.GetComponentInChildren<Button> ().onClick.AddListener (
			() => {formulas.PlaceInSpace (spaceID, item, newItem);}
		);
		//button.transform.parent = menuPanel;	
		button.transform.SetParent (spacePanel, false);
		int offset = spacePanel.transform.childCount;
		offset *= 10;
		button.transform.Translate (0, offset, 0);
	}
		

	// Update is called once per frame
	void Update () {
	
	}
}
