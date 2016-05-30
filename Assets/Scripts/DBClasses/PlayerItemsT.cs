using SimpleSQL;

public class PlayerItemsT {

	[PrimaryKey][AutoIncrement]
	public int ItemID { get; set; }
	public int ItemState { get; set; }
	//-1: Not Purchased
	//0: Purchased/Owned
	//1: Placed

}
