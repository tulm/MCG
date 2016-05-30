using SimpleSQL;

public class CageTypeT {

	[PrimaryKey][AutoIncrement]
	public int ID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int Cost { get; set; }
}
