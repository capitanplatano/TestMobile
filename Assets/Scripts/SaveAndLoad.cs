using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveAndLoad {

	public static List<Game> savedGames = new List<Game>();

	public static int selectedIndex = -1;
	public static void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames1.gd");
		bf.Serialize(file, SaveAndLoad.savedGames);
		file.Close();
	}

	public static void Load() {
		if(File.Exists(Application.persistentDataPath + "/savedGames1.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames1.gd", FileMode.Open);
			SaveAndLoad.savedGames = (List<Game>)bf.Deserialize(file);
			file.Close();
		}
	}
}
