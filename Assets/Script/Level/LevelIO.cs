using System;
using System.IO;
using System.Collections.Generic;
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

	public static string[] GetLevels(bool resource) {
		if (resource) {
			TextAsset[] i = Resources.LoadAll<TextAsset>("/Level/");
			List<String> ls = new List<string>();
			foreach (TextAsset ta in i) {
				ls.Add(ta.name);
			}
			return ls.ToArray();
		}
		List<String> levels = new List<string>();
		foreach (string path in Directory.GetFiles(LevelDir)) {
			string[] spl = path.Replace('\\', '/').Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			levels.Add(spl[spl.Length - 1]);
		}
		return levels.ToArray();
	}
	
	public static bool SaveLevel(LevelData level, string name) {
		if (!IsValidName(name)) {
			Debug.LogError("Invalid level name: " + name);
			return false;
		}
		Debug.Log("Saving: " + GetFileName(name));
		File.WriteAllText(GetFileName(name), level.Serialize(), System.Text.Encoding.UTF8);
		return true;
	}

	public static bool LevelExists(bool resource, string name) {
		name = name.Trim();
		if (!IsValidName(name)) {
			return false;
		}
		if (resource) {
			return Resources.Load<TextAsset>(GetResName(name)) != null;
		}
		return File.Exists(GetFileName(name));
	}

	public static bool LoadLevel(bool init, bool resource, bool fullColliders, Transform levelParent, LevelData level, string name) {
		if (resource) {
			return LoadLevelFromResource(init, fullColliders, levelParent, level, name);
		}
		return LoadLevelFromFile(init, fullColliders, levelParent, level, name);
	}
	
	public static bool LoadLevelFromFile(bool init, bool fullColliders, Transform levelParent, LevelData level, string name) {
		if (name.EndsWith(".lvl", StringComparison.Ordinal)) {
			name.Substring(0, name.Length - 4);
		}
		if (!LevelExists(false, name)) {
			Debug.LogError("Level does not exist: " + name);
			return false;
		}
		string fileName = GetFileName(name);
		Debug.Log("Loading level from file: " + fileName);
		string file = null;
		try {
			file = File.ReadAllText(fileName, System.Text.Encoding.UTF8);
		} catch (Exception e) {
			Debug.LogError("Failed to load file: " + e.Message);
		}
		if (file == null) {
			Debug.LogError("Failed to load level file: " + fileName);
			return false;
		}
		LoadLevelFromString(init, fullColliders, levelParent, level, file);
		return true;
	}

	public static bool LoadLevelFromResource(bool init, bool fullColliders, Transform levelParent, LevelData level, string name) {
		if (!LevelExists(true, name)) {
			Debug.LogError("Level does not exist as resource: " + name);
			return false;
		}
		string resPath = GetResName(name);
		Debug.Log("Loading level from resource: " + resPath);
		TextAsset text = Resources.Load<TextAsset>(resPath);
		if (text == null) {
			Debug.LogError("Failed to load level resource: " + resPath);
			return false;
		}
		LoadLevelFromString(init, fullColliders, levelParent, level, text.text);
		return true;
	}

	public static void LoadLevelFromString(bool init, bool fullColliders, Transform levelParent, LevelData level, string serialized) {
		level.Deserialize(init, fullColliders, levelParent, serialized);
		Debug.Log("Deserialized level");
	}

	public static bool IsValidName(string name) {
		name = name.Trim();
		return name.Length > 3 && name.Length <= 26;
	}

	public static bool DeleteLevel(string name) {
		string path = GetFileName(name);
		if (File.Exists(path)) {
			File.Delete(path);
			return true;
		}
		return false;
	}

	private static string GetFileName(string level) {
		return LevelDir + level + ".lvl";
	}

	private static string GetResName(string level) {
		return "/Level/" + level;
	}

}