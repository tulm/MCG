using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistentSaving {

	public void Save(PlayerData data) {
		Debug.Log (Application.persistentDataPath);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file= File.Create( Application.persistentDataPath+"/playerInfo.dat");
		bf.Serialize(file, data);
		file.Close();
		}

	public void Load(ref PlayerData data){
		if (File.Exists(Application.persistentDataPath+"/playerinfo.dat")){
			BinaryFormatter bf= new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath+"/playerinfo.dat", FileMode.Open);
			data = (PlayerData) bf.Deserialize(file);  //CAST your serialized data!
			file.Close();
		}
	}

}
