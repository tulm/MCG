using SimpleSQL;

public class SpaceT {

	[PrimaryKey][AutoIncrement]
	public int ID { get; set; }
	public int Item { get; set; }
	//-1: second part of Large item placed
	//0: no item placed
	//1-N: itemID of placed item

}

