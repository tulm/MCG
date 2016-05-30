using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class testResImg : MonoBehaviour {
	public GameObject buttonPrefab;
	public Transform playerItemPanel;
	// Use this for initialization
	void Start () {
		GameObject button = (GameObject)Instantiate (buttonPrefab); 
		button.GetComponentInChildren<Text> ().text = "MADE a Button";
		button.transform.SetParent (playerItemPanel, false);

		Sprite tempSprite = Resources.Load<Sprite>("ItemImages/i3");
		//gameObject.GetComponent<Image>().sprite = tempSprite;
		gameObject.GetComponentInChildren<Image>().sprite = tempSprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
