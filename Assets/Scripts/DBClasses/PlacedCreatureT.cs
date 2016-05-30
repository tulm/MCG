using SimpleSQL;

public class PlacedCreatureT {

	// The PlayerID is the primary key and also autoincrements itself
		// the SQLite database so we reflect that here with these attributes.
	[PrimaryKey]
	public int CreatureID { get; set; }
	public int ItemID { get; set; }
	public int StartTime { get; set; }
	public int EndTime { get; set; }
	public int SpaceID { get; set; }
		//public string Name { get; set; }
		//public string Description { get; set; }

		//public int MonsterTypeID { get; set; } 
	//public int CageTypeID { get; set; } 
	//public int MementoID { get; set; } 
	//public int Rarity { get; set; } 
		//public int FavItem1ID { get; set; } 
	//public int FavItem2ID { get; set; } 
	//public int FavItem3ID { get; set; } 
	}

