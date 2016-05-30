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
public class InsertCommand : MonoBehaviour {

	// The list of player stats from the database
	//private List<PlayerStats> _playerStatsList;
	private List<MonsterT> _playerStatsList;
	private List<MonsterTypeT> _monsterTypelist;
	
	// These variables will be used to store data from the GUI interface
	private string _newPlayerName;
	private string _newPlayerTotalKills;
	private string _newPlayerPoints;
	
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
		_newPlayerName = GUILayout.TextField(_newPlayerName, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Description:", GUILayout.Width(100.0f));
		_newPlayerTotalKills = GUILayout.TextField(_newPlayerTotalKills, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("MonsterTypeID:", GUILayout.Width(100.0f));
		_newPlayerPoints = GUILayout.TextField(_newPlayerPoints, GUILayout.Width(200.0f));
		GUILayout.EndHorizontal();
		
		int totalKills;
		int points;
		
		if (!int.TryParse(_newPlayerTotalKills, out totalKills))
			totalKills = 0;
		
		if (!int.TryParse(_newPlayerPoints, out points))
			points = 0;
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Insert", GUILayout.Width(100.0f)))
		{
			SavePlayerStats_Simple(_newPlayerName, totalKills, points);
			ResetGUI();
		}
		GUILayout.Label("OR", GUILayout.Width(30.0f));
		if (GUILayout.Button("Insert Query", GUILayout.Width(100.0f)))
		{
			SavePlayerStats_Query(_newPlayerName, totalKills, points);
			ResetGUI();
		}
		GUILayout.Label("OR", GUILayout.Width(30.0f));
		if (GUILayout.Button("Insert 3 Times", GUILayout.Width(200.0f)))
		{
			SavePlayerStats_SimpleThreeTimes(_newPlayerName, totalKills, points);
			ResetGUI();
		}
		GUILayout.Label("OR", GUILayout.Width(30.0f));
		if (GUILayout.Button("Insert 3 Times Query", GUILayout.Width(200.0f)))
		{
			SavePlayerStats_QueryThreeTimes(_newPlayerName, totalKills, points);
			ResetGUI();
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Space(20.0f);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Monster Name", GUILayout.Width(200.0f));
		GUILayout.Label("Description", GUILayout.Width(150.0f));
		GUILayout.Label("MonsterType ID", GUILayout.Width(150.0f));
		GUILayout.EndHorizontal();
		
		GUILayout.Label("-----------------------------------------------------------------------------------------------------------------------------------------");
	/*	
		foreach (PlayerStats playerStats in _playerStatsList)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(playerStats.PlayerName, GUILayout.Width(200.0f));
			GUILayout.Label(playerStats.TotalKills.ToString(), GUILayout.Width(150.0f));
			GUILayout.Label(playerStats.Points.ToString(), GUILayout.Width(150.0f));
			GUILayout.EndHorizontal();
		}
		*/
		foreach (MonsterTypeT monstertypes in _monsterTypelist) {
			GUILayout.BeginHorizontal ();
			GUILayout.Label(monstertypes.Name, GUILayout.Width(200.0f));
			GUILayout.Label(monstertypes.Description, GUILayout.Width(150.0f));
			GUILayout.EndHorizontal();

		}
		foreach (MonsterT playerStats in _playerStatsList)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(playerStats.Name, GUILayout.Width(200.0f));
			GUILayout.Label(playerStats.Description, GUILayout.Width(150.0f));
			GUILayout.Label(playerStats.MonsterTypeID.ToString(), GUILayout.Width(150.0f));
			GUILayout.EndHorizontal();
		}
		
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
	}

	class MonsterTypeT{
		//public int ID;
		//public string Name;
		//public string Description;
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; } 

		//public int Points { get; set; } 
	}

	private void ResetGUI()
	{
		// Reset the temporary GUI variables
		_newPlayerName = "";
		_newPlayerTotalKills = "";
		_newPlayerPoints = "";

		// Loads the player stats from the database using Linq
		//_playerStatsList = new List<PlayerStats> (from ps in dbManager.Table<PlayerStats> () select ps);
		_playerStatsList = new List<MonsterT> (from ps in dbManager.Table<MonsterT> () select ps);
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
	private void SavePlayerStats_Query(string playerName, int totalKills, int points)
	{
		// Call our SQL statement using ? to bind our variables
		//dbManager.Execute("INSERT INTO PlayerStats (PlayerName, TotalKills, Points) VALUES (?, ?, ?)", playerName, totalKills, points);
		dbManager.Execute("INSERT INTO MonsterT (Name, Description) VALUES (?, ?)", playerName, totalKills);
	}
	
	/// <summary>
	/// Saves the player stats three times, using a collection of PlayerStats and inserting all at once. This example uses
	/// transactions which can vastly reduce the amount of time spent updating data when you have a lot of records to insert.
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
	private void SavePlayerStats_SimpleThreeTimes(string playerName, int totalKills, int points)
	{
		// set up our playerStats variable to store data
		PlayerStats playerStats;
		
		// set up our collection of playerStats to pass to the database
		List<PlayerStats> playerStatsCollection = new List<PlayerStats>();
		
		// add player stats to the collection
		playerStats = new PlayerStats { PlayerName = playerName, TotalKills = totalKills, Points = points };
		playerStatsCollection.Add (playerStats);
		
		// add player stats to the collection
		playerStats = new PlayerStats { PlayerName = playerName, TotalKills = totalKills, Points = points };
		playerStatsCollection.Add (playerStats);

		// add player stats to the collection
		playerStats = new PlayerStats { PlayerName = playerName, TotalKills = totalKills, Points = points };
		playerStatsCollection.Add (playerStats);
		
		// insert all the player stats at one time by passing the collection
		dbManager.InsertAll(playerStatsCollection);
	}	
	
	/// <summary>
	/// Saves the player stats three times using a SQL statement. The calls are made inside of a transaction
	/// by using BeginTransaction and Commit. Transactions vastly reduce the amount of time it takes
	/// to save multiple records.
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
	private void SavePlayerStats_QueryThreeTimes(string playerName, int totalKills, int points)
	{
		// set up our insert SQL statement with ? parameters
		string sql = "INSERT INTO PlayerStats (PlayerName, TotalKills, Points) VALUES (?, ?, ?)";
		
		// start a transaction
		dbManager.BeginTransaction();
		
		// execute the SQL statement three times
		dbManager.Execute(sql, playerName, totalKills, points);
		dbManager.Execute(sql, playerName, totalKills, points);
		dbManager.Execute(sql, playerName, totalKills, points);
		
		// commit our transaction to the database
		dbManager.Commit();
	}	
}
