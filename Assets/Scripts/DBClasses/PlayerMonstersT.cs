using SimpleSQL;

public class PlayerMonstersT {

	[PrimaryKey][AutoIncrement]
	public int MonsterID { get; set; }
	public int MonsterState { get; set; }
	//0: No Visit
	//-N: Number of visits; Not Caught
	//N: Number of visits; Caught

}

