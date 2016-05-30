using SimpleSQL;

public class PlayerItemsTTemp {

	[PrimaryKey][AutoIncrement]
	public int ItemID { get; set; }
	public int ItemState { get; set; }
	//-1: Not Purchased
	//0: Purchased/Owned
	//1: Placed

	//ones that are just from a JOIN
	public string Name { get; set; }
	public string Description { get; set; }
	public int TypeID { get; set; }
	public int Cost { get; set; }
}
