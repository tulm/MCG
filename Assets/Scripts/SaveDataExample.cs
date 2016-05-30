using UnityEngine;
using System.Collections;

public class SaveDataExample : MonoBehaviour {
	
	public string fileName;
	public string[] startNames= new string[]{"bone", "knife", "meat"};
	public string[] startDescs= new string[]{"yummy metal bone", "cut stuff up", "feed me"};
	private SaveData data;
	public ItemClass[] playerItems = new ItemClass[3];
	
	void Start ()
	{
		//Create data instance
		data = new SaveData(fileName);
		
		//Add keys with names and values
		data["Position"] = new Vector3(20, 3, -5);
		data["Rotation"] = new Quaternion(0.1f,0.1f,0.1f,1);

		int i;
		for (i = 0;  i < 3; i++)
		{
			playerItems[i].desc = startDescs [i];
			playerItems[i].name = startNames [i];
			playerItems[i].SOP = 0;
			string tempKeyName = "playerItems" + i;
			data [tempKeyName] = playerItems[i];
		}

	
		//Save the data
		data.Save();
		
		//Load the data we just saved
		data = SaveData.Load(Application.persistentDataPath+"\\"+fileName+".uml");
		//data = SaveData.Load(fileName+".uml");
		Debug.Log (Application.persistentDataPath);
		//Use data
		Debug.Log("Rotation : " + data.GetValue<Quaternion>("Rotation"));
		Debug.Log ("Player Item : " + data.GetValue<ItemClass> ("playerItems0").desc);
		Debug.Log ("Player Item : " + data.GetValue<ItemClass> ("playerItems1").name);
	}
}