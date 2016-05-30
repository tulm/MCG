using SimpleSQL;

public class ItemsT {

	[PrimaryKey][AutoIncrement]
	public int ID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int TypeID { get; set; }
	public int Cost { get; set; }
}
