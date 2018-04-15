using System;
using System.IO;
using UnityEngine;

public static class LevelIO {

	public static string LevelDir { private set; get; }

	public static void InitIO() {
		LevelDir = Application.persistentDataPath.Replace('\\', '/') + "/Levels/";
		CreateDir(LevelDir);
	}

	private static void CreateDir(string dir) {
		if (!Directory.Exists(dir)) {
			Directory.CreateDirectory(dir);
		}
	}
	
	public static void SaveLevel(LevelData level, string name) {
		File.WriteAllText(LevelDir + name + ".lvl", level.Serialize(), System.Text.Encoding.UTF8);
	}
	
	public static void LoadLevelFromFile(Transform levelParent, LevelData level, string name) {
		string fileName = LevelDir + name + ".lvl";
		Debug.Log("Loading level from file: " + fileName);
		string file = null;
		try {
			file = File.ReadAllText(fileName, System.Text.Encoding.UTF8);
		} catch (Exception e) {
			Debug.LogError("Failed to load file: " + e.Message);
		}
		if (file == null) {
			Debug.LogError("Failed to load level file: " + fileName);
			return;
		}
		LoadLevelFromString(levelParent, level, file);
	}

	public static void LoadLevelFromResource(Transform levelParent, LevelData level, string name) {
		string resPath = "/Level/" + name;
		Debug.Log("Loading level from resource: " + resPath);
		TextAsset text = Resources.Load<TextAsset>(resPath);
		if (text == null) {
			Debug.LogError("Failed to load level resource: " + resPath);
			return;
		}
		LoadLevelFromString(levelParent, level, text.text);
	}

	public static void LoadLevelFromString(Transform levelParent, LevelData level, string serialized) {
		Debug.Log("Deserializing level");
		level.Deserialize(levelParent, serialized);
		Debug.Log("Loaded level");
	}

}