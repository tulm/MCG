using SimpleSQL;

public class MonsterT {

		// The PlayerID is the primary key and also autoincrements itself
		// the SQLite database so we reflect that here with these attributes.
		[PrimaryKey, AutoIncrement]
	public int ID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int MonsterTypeID { get; set; } 
	public int CageTypeID { get; set; } 

	public int Rarity { get; set; } 
	public int FavItem1ID { get; set; } 
	public int FavItem2ID { get; set; } 
	public int FavItem3ID { get; set; } 
	public int FavType { get; set; } 
	}

