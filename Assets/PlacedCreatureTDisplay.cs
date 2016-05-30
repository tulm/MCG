using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using SimpleSQL;

public class PlacedCreatureTDisplay : MonoBehaviour {
	public SimpleSQL.SimpleSQLManager dbManager;
	private List<PlacedCreatureT> placedCreatures;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("updatin");
		placedCreatures = new List<PlacedCreatureT> (from ps in dbManager.Table<PlacedCreatureT> ()
		                                           select ps);
		foreach (PlacedCreatureT pc in placedCreatures) {
			Debug.Log ("Placed Creature: " + pc.CreatureID);
		}
	}
}
