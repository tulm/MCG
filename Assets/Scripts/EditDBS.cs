using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using SimpleSQL;

/// <summary>
/// This script shows how to call the Insert command on a database using simplified class-based
/// methods or by calling the SQL statements directly. Transactions are shown in the functions that
/// insert three times to show how to speed up your data.
/// 
/// In this example we will not overwrite the working database since we are updating the data. If
/// we were to overwrite, then changes would be lost each time the scene is run again.
/// </summary>
public class EditDBS : MonoBehaviour {

	// The list of player stats from the database
	//private List<PlayerStats> _playerStatsList;
	private List<MonsterT> _monsterList;
	private List<MonsterTypeT> _monsterTypelist;

	// These variables will be used to store data from the GUI interface
	private string _newName;
	private string _newDescription;
	private string _newMTID;
	private string _newCID;
	private string _newMEID;
	private string _newRare;
	private string _newFI1, _newFI2, _newFI3;

	// reference to our db manager object
	public SimpleSQL.SimpleSQLManager dbManager;

	// reference to our output text object
	public GUIText outputText;

	void Start()
	{
		// clear out the output text since we are using GUI in this example
		outputText.text = "";

		// reset the GUI and reload
		ResetGUI();
	}

	void OnGUI()
	{
		GUILayout.BeginVertical();

		GUILayout.Space(10.0f);

		GUILayout.BeginHorizontal();

		GUILayout.Space(10.0f);

		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Monster Name:", GUILayout.Width(100.0f));
		_newName = GUILayout.TextField(_newName, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Description:", GUILayout.Width(100.0f));
		_newDescription = GUILayout.TextField(_newDescription, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("MonsterTypeID:", GUILayout.Width(100.0f));
		_newMTID = GUILayout.TextField(_newMTID, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("CageTypeID:", GUILayout.Width(100.0f));
		_newCID = GUILayout.TextField(_newCID, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("MementoID:", GUILayout.Width(100.0f));
		_newMEID = GUILayout.TextField(_newMEID, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Rarity:", GUILayout.Width(100.0f));
		_newRare = GUILayout.TextField(_newRare, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Fav Item1", GUILayout.Width(100.0f));
		_newFI1 = GUILayout.TextField(_newFI1, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Fav Item 2:", GUILayout.Width(100.0f));
		_newFI2 = GUILayout.TextField(_newFI2, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Fav Item3:", GUILayout.Width(100.0f));
		_newFI3 = GUILayout.TextField(_newFI3, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		//int totalKills;
		//if (!int.TryParse(_newPlayerTotalKills, out totalKills))
			//totalKills = 0;


		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Insert Monster Type", GUILayout.Width(100.0f)))
		{
			SavePlayerStats_Simple(_newName, 3, 4);
			ResetGUI();
		}
		GUILayout.Label("OR", GUILayout.Width(30.0f));
		if (GUILayout.Button("Insert Monster", GUILayout.Width(100.0f)))
		{
			SavePlayerStats_Query(_newName, _newDescription, int.Parse(_newMTID), int.Parse(_newCID), int.Parse(_newMEID), int.Parse(_newRare), int.Parse(_newFI1), int.Parse(_newFI2), int.Parse(_newFI3));
			ResetGUI();
		}
		GUILayout.Label("OR", GUILayout.Width(30.0f));
		GUILayout.EndHorizontal();

		GUILayout.Space(20.0f);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Monster Name", GUILayout.Width(100.0f));
		GUILayout.Label("Description", GUILayout.Width(150.0f));
		GUILayout.Label("MonsterType ID", GUILayout.Width(100.0f));
		GUILayout.Label("Cage Type ID", GUILayout.Width(100.0f));
		GUILayout.Label("Memento ID", GUILayout.Width(100.0f));
		GUILayout.Label("Rarity", GUILayout.Width(100.0f));
		GUILayout.Label("Fav Item 1 ID", GUILayout.Width(100.0f));
		GUILayout.Label("Fav Item 2 ID", GUILayout.Width(100.0f));
		GUILayout.Label("Fav Item 3 ID", GUILayout.Width(100.0f));
		GUILayout.EndHorizontal();

		GUILayout.Label("-----------------------------------------------------------------------------------------------------------------------------------------");

		foreach (MonsterTypeT monstertypes in _monsterTypelist) {
			GUILayout.BeginHorizontal ();
			GUILayout.Label(monstertypes.Name, GUILayout.Width(200.0f));
			GUILayout.Label(monstertypes.Description, GUILayout.Width(150.0f));
			GUILayout.EndHorizontal();

		}
		foreach (MonsterT playerStats in _monsterList)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(playerStats.Name, GUILayout.Width(100.0f));
			GUILayout.Label(playerStats.Description, GUILayout.Width(150.0f));
			GUILayout.Label(playerStats.MonsterTypeID.ToString(), GUILayout.Width(100.0f));
			GUILayout.Label(playerStats.CageTypeID.ToString(), GUILayout.Width(100.0f));
			//GUILayout.Label(playerStats.MementoID.ToString(), GUILayout.Width(100.0f));
			GUILayout.Label(playerStats.Rarity.ToString(), GUILayout.Width(100.0f));
			GUILayout.Label(playerStats.FavItem1ID.ToString(), GUILayout.Width(100.0f));
			GUILayout.Label(playerStats.FavItem2ID.ToString(), GUILayout.Width(100.0f));
			GUILayout.Label(playerStats.FavItem3ID.ToString(), GUILayout.Width(100.0f));
			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	class MonsterTypeT{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; } 
	}

	private void ResetGUI()
	{
		// Reset the temporary GUI variables
		_newName = "";
		_newDescription = "";
		_newCID = "";
		_newRare = "";
		_newMEID = "";
		_newMTID = "";
		_newFI1 = "";
		_newFI2 = "";
		_newFI3 = "";

		// Loads the player stats from the database using Linq
		//_playerStatsList = new List<PlayerStats> (from ps in dbManager.Table<PlayerStats> () select ps);
		_monsterList = new List<MonsterT> (from ps in dbManager.Table<MonsterT> () select ps);
		_monsterTypelist = new List<MonsterTypeT> (from ps in dbManager.Table<MonsterTypeT> () select ps);
	}

	/// <summary>
	/// Saves the player stats by using the PlayerStats class structure. No need for SQL here.
	/// </summary>
	/// <param name='playerName'>
	/// Player name.
	/// </param>
	/// <param name='totalKills'>
	/// Total kills.
	/// </param>
	/// <param name='points'>
	/// Points.
	/// </param>
	private void SavePlayerStats_Simple(string playerName, int totalKills, int points)
	{
		// Initialize our PlayerStats class
		//PlayerStats playerStats = new PlayerStats { PlayerName = playerName, TotalKills = totalKills, Points = points };
		MonsterTypeT playerStats = new MonsterTypeT {Name= playerName, Description = totalKills.ToString() };

		// Insert our PlayerStats into the database
		dbManager.Insert(playerStats);
	}

	/// <summary>
	/// Saves the player stats by executing a SQL statement. Note that no data is returned, this only modifies the table
	/// </summary>
	/// <param name='playerName'>
	/// Player name.
	/// </param>
	/// <param name='totalKills'>
	/// Total kills.
	/// </param>
	/// <param name='points'>
	/// Points.
	/// </param>
	private void SavePlayerStats_Query(string name, string description, int mid, int cid, int meid, int rare, int fi1, int fi2, int fi3)
	{
		// Call our SQL statement using ? to bind our variables
		//dbManager.Execute("INSERT INTO PlayerStats (PlayerName, TotalKills, Points) VALUES (?, ?, ?)", playerName, totalKills, points);
		dbManager.Execute("INSERT INTO MonsterT (Name, Description, MonsterTypeID, CageTypeID, MementoID, Rarity, FavItem1ID, FavItem2ID, FavItem3ID) VALUES (?, ?, ?, ?, ? , ? , ? , ? , ?)", name, description, mid, cid, meid, rare, fi1, fi2, fi3);
	}


}
