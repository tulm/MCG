using SimpleSQL;

public class PresentsToAcceptT {

	// The PlayerID is the primary key and also autoincrements itself
		// the SQLite database so we reflect that here with these attributes.
	[PrimaryKey] [AutoIncrement]
	public int ID { get; set; }
	public int CreatureID { get; set; }
	public int ItemID { get; set; }
	public int Silver { get; set; }
	public int Gold { get; set; }

	}

