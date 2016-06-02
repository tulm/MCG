using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class DataBaseS : MonoBehaviour {
	public SimpleSQL.SimpleSQLManager dbManager;
	// Use this for initialization
	void Start () {
		string sql = "SELECT * FROM MonsterTypeT";
		//string sql = "INSERT INTO MonsterTypeT"+" (Name, Description) "+" VALUES (?,?)";
		//dbManager.Execute (sql, "name2", "my descriptoin is something else");
		bool yn=false;
		dbManager.QueryFirstRecord<MonsterT>(out yn, sql);
		Debug.Log (yn);


		List<MonsterT> monsterList= dbManager.Query<MonsterT> (sql);
		Debug.Log ("something: "+ monsterList [0]);
		foreach (MonsterT monstertype in monsterList)
		{
			Debug.Log("A monster row id was: "+ monstertype.ID);
		}

		//List<MonsterTypes>mts=new List<MonsterTypes>(from w dbManager select w);
		//List<MonsterTypes> mts= new List<MonsterTypes> (from w dbManager.Table<table1>() select w);
	}

	/*
	class MonsterTypes{
		public int ID;
		public string Name;
		public string Description;
	}
	*/

	// Update is called once per frame
	void Update () {
	
	}
}
