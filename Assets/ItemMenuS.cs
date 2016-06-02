using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemMenuS : MonoBehaviour {
	public FormulaS formulas;
	public GameObject buttonPrefab;
	public GameObject removeButtonPrefab;
	public Transform playerItemPanel;
	public Transform spacePanel;
	public Transform presentsPanel;

	public void ClearOldButtons (string panelName){
		Transform CanvasPanel=playerItemPanel;
		if (panelName == "playerItemPanel")
			CanvasPanel = playerItemPanel;
		else if (panelName == "spacePanel")
			CanvasPanel = spacePanel;
		else if (panelName == "presentsPanel")
			CanvasPanel = presentsPanel;
		foreach (Transform child in CanvasPanel)
		{
			Destroy (child.gameObject);
		}
	}

	public void MakePlayerItemPlaceB(int itemID, string itemName)
	{
		GameObject button = (GameObject)Instantiate (buttonPrefab);
		//GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/buttonPrefab" ) )as GameObject;
		button.GetComponentInChildren<Text> ().text = itemName;
		button.GetComponentInChildren<Button> ().onClick.AddListener (
			() => {formulas.ShowSpacesToPlace (itemID);}
		);
		button.transform.SetParent (playerItemPanel, false);
		/*
		// Layout handled by Canvas Component setting actually
		int offset = playerItemPanel.transform.childCount;
		offset *= 10;
		button.transform.Translate (0, offset, 0);
		*/
	}

	public void MakePlayerItemRemoveB(int itemID, string itemName)
	{
		GameObject button = (GameObject)Instantiate (removeButtonPrefab);
		//GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/removeButtonPrefab" ) )as GameObject;
		button.GetComponentInChildren<Text> ().text = itemName;
		button.GetComponentInChildren<Button> ().onClick.AddListener (
			() => {formulas.RemovePlacedItem (-1, itemID, false);}
		);	
		button.transform.SetParent (playerItemPanel, false);
	}
	public void MakeSpaceB(int spaceID, int item, int newItem)
	{
		//Debug.Log ("made it to make a space button for all spaces in spaceT db");
		GameObject button = (GameObject)Instantiate (buttonPrefab);
		//GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/buttonPrefab" ) )as GameObject;
		button.GetComponentInChildren<Text> ().text = item.ToString();
		button.GetComponentInChildren<Button> ().onClick.AddListener (
			() => {formulas.PlaceInSpace (spaceID, item, newItem);}
		);	
		button.transform.SetParent (spacePanel, false);
	}
		

	// Update is called once per frame
	void Update () {
	
	}
}
