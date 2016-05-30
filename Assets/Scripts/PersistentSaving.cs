using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistentSaving : MonoBehaviour {
	/*
	public static PersistentSaving control;
	public float health;

	void Awake() {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		}
	}
	public void Save() {
		
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file= File.Create( Application.persistentDataPath+"/playerInfo.dat");
			PlayerData data = new PlayerData();
			data.health=health;
			bf.Serialize(file, data);
			file.Close();
		}

	public void Load(){
		if (File.Exists(Application.persistentDataPath+"/playerinfo.dat")){
			BinaryFormatter bf= new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath+"/playerinfo.dat", FileMode.Open);
			PlayerData data = (PlayerData) bf.Deserialize(file);  //CAST your serialized data!

			data.health = health; //shouldnt this be the reverse order?
		}
	}

	[System.Serializable]
	class PlayerData{
		public float health;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	*/
}
