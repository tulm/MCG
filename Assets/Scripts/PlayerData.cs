using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerData {

	public int SilverCoins, GoldCoins;
	public int bait1;
	public int bait1Q;


	public /*nothing goes here*/ PlayerData() {
		// initialize
		bait1=0;
		bait1Q = 1;
		SilverCoins = 0;
		GoldCoins = 0;
	}
}
