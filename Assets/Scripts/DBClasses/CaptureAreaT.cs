using SimpleSQL;

public class NewBehaviourScript  {
// the SQLite database so we reflect that here with these attributes.
	[PrimaryKey][AutoIncrement]
	public int ID { get; set; }
	public int CageTypeID { get; set; }
	public int State { get; set; }
	public int Size { get; set; }

}
