using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistentSaving {

	//public static PersistentSaving control;

	public void Save(PlayerData data) {
		
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file= File.Create( Application.persistentDataPath+"/playerInfo.dat");
		bf.Serialize(file, data);
		file.Close();
		}

	public void Load(ref PlayerData data){
		if (File.Exists(Application.persistentDataPath+"/playerinfo.dat")){
			BinaryFormatter bf= new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath+"/playerinfo.dat", FileMode.Open);
			//PlayerData data = (PlayerData) bf.Deserialize(file);  //CAST your serialized data!
			data = (PlayerData) bf.Deserialize(file);  //CAST your serialized data!
			file.Close();
			//Debug.Log ("we loaded info on bait1 and stuff and gold was: "+data.GoldCoins);
			//return data;
		}
		//return null;
	}
	/*
	[System.Serializable]
	public class PlayerData{
		public int SilverCoins, GoldCoins;
		public int bait1;
		public int bait1Q;
	}

*/

}
