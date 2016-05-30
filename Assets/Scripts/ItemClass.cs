using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class ItemClass {
	public string name;
	public string desc;
	public int SOP;   //0 store; 1 owned; 2 placed;

	public ItemClass (){}

	public ItemClass (string n, string d, int S) {

		name = n;
		desc = d;
		SOP = S;
	}
}
