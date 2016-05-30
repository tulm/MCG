using SimpleSQL;

public class MementoT {

	[PrimaryKey][AutoIncrement]
	public int ID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
}
