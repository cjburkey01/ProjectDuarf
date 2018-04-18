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

	private static string CreateLevelPath(string name) {
		return LevelDir + name + ".lvl";
	}

	private static void CreateDir(string dir) {
		if (!Directory.Exists(dir)) {
			Directory.CreateDirectory(dir);
		}
	}

	private static bool LoadLevelFromFile(bool init, bool fullColliders, Transform levelParent, LevelData level, string name) {
		if (name.EndsWith(".lvl", StringComparison.Ordinal)) {
			name.Substring(0, name.Length - 4);
		}
		if (!GetLevelExists(name)) {
			Debug.LogError("Level does not exist: " + name);
			return false;
		}
		string fileName = GetLevelFileFromName(name);
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

	private static void LoadLevelFromString(bool init, bool fullColliders, Transform levelParent, LevelData level, string serialized) {
		level.Deserialize(init, fullColliders, levelParent, serialized);
		Debug.Log("Deserialized level");
	}

	public static bool IsValidLevelName(string name) {
		name = name.Trim();
		return name.Length > 3 && name.Length <= 26;
	}

	public static bool DeleteLevel(string name) {
		string path = GetLevelFileFromName(name);
		if (File.Exists(path)) {
			File.Delete(path);
			return true;
		}
		return false;
	}

	public static bool LoadLevel(bool init, bool fullColliders, Transform levelParent, LevelData level, string levelName) {
		return LoadLevelFromFile(init, fullColliders, levelParent, level, levelName);
	}

	public static bool SaveLevel(LevelData level, string name) {
		if (!IsValidLevelName(name)) {
			Debug.LogError("Invalid level name: " + name);
			return false;
		}
		Debug.Log("Saving: " + CreateLevelPath(name));
		File.WriteAllText(CreateLevelPath(name), level.Serialize(), System.Text.Encoding.UTF8);
		return true;
	}

	public static bool GetLevelExists(string name) {
		foreach (string lvl in GetLevels(false)) {
			if (lvl.Equals(name)) {
				return true;
			}
		}
		return false;
	}

	private static string GetLevelFileFromName(string name) {
		foreach (string file in GetFiles()) {
			if (GetLevelNameFromPath(file).Equals(name)) {
				return file;
			}
		}
		return null;
	}

	public static string[] GetLevels(bool builtin) {
		if (builtin) {
			Debug.LogWarning("Refusing to load resource levels, not implemented yet");
			return new string[0];
		}
		List<string> levels = new List<string>();
		foreach (string path in Directory.GetFiles(LevelDir)) {
			string name = GetLevelNameFromPath(path);
			if (name != null) {
				levels.Add(name);
			}
		}
		return levels.ToArray();
	}

	private static string[] GetFiles() {
		List<string> files = new List<string>();
		foreach (string path in Directory.GetFiles(LevelDir)) {
			if (path.EndsWith(".lvl", StringComparison.Ordinal)) {
				files.Add(path);
			}
		}
		return files.ToArray();
	}

	private static string GetLevelNameFromPath(string path) {
		string inf = File.ReadAllText(path);
		if (inf == null) {
			return null;
		}
		string[] txt = inf.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		if (txt.Length < 1) {
			return null;
		}
		return txt[0];
	}

}