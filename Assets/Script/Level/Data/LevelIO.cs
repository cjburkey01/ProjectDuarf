using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Service.Support;

[RegisterEventHandlers]
public static class LevelIO {

	public static string LevelDir { private set; get; }

	static void RegisterEvents() {
		EventObject.EventSystem.AddListener<EventGameInit>(InitIO);
	}

	static void InitIO<T>(T e) where T : EventGameInit {
		e.GetName();
		LevelDir = Application.persistentDataPath.Replace('\\', '/') + "/Levels/";
		CreateDir(LevelDir);
	}

	[Obsolete]
	static string CreateLevelPath(string name) {
		string lvl = GetLevelFileFromName(name);
		if (!string.IsNullOrEmpty(lvl)) {
			return lvl;
		}
		return LevelDir + MD5Util.Hash(name) + ".lvl";
	}

	static void CreateDir(string dir) {
		if (!Directory.Exists(dir)) {
			Directory.CreateDirectory(dir);
		}
	}

	[Obsolete]
	static LevelData LoadLevelFromFile(string name) {
		if (name.EndsWith(".lvl", StringComparison.Ordinal)) {
			name.Substring(0, name.Length - 4);
		}
		if (!GetLevelExists(name)) {
			Debug.LogError("Level does not exist: " + name);
			return null;
		}
		string fileName = GetLevelFileFromName(name);
		Debug.Log("Loading level from file: " + fileName);
		string decoded = GetDecodedLevelFromPath(fileName);
		if (string.IsNullOrEmpty(decoded)) {
			Debug.LogError("Failed to load level from file: " + fileName);
			return null;
		}
		return LoadLevelFromString(decoded.Trim());
	}

	[Obsolete]
	static LevelData LoadLevelFromString(string serialized) {
		LevelData level = new LevelData();
		level.Deserialize(serialized);
		Debug.Log("Deserialized level");
		return level;
	}

	public static bool IsValidLevelName(string name) {
		name = name.Trim();
		return name.Length > 3 && name.Length <= 26;
	}

	[Obsolete]
	public static bool DeleteLevel(string name) {
		string path = GetLevelFileFromName(name);
		if (File.Exists(path)) {
			File.Delete(path);
			return true;
		}
		return false;
	}

	[Obsolete]
	public static LevelData LoadLevel(string levelName) {
		return LoadLevelFromFile(levelName);
	}

	[Obsolete]
	public static bool SaveLevel(LevelData level, string name) {
		if (!IsValidLevelName(name)) {
			Debug.LogError("Invalid level name: " + name);
			return false;
		}
		string path = CreateLevelPath(name);
		Debug.Log("Saving: " + path);
		File.WriteAllText(path, "encode" + Encoding.UTF8.ToBase64(level.Serialize()), Encoding.UTF8); // Saves in Base64 (UTF-8)
		return true;
	}

	// Checks whether the specied level exits
	[Obsolete]
	public static bool GetLevelExists(string name) {
		foreach (string lvl in GetLevels(false)) {
			if (lvl.Equals(name)) {
				return true;
			}
		}
		return false;
	}

	// Retrieves the path to a level from its name
	[Obsolete]
	static string GetLevelFileFromName(string name) {
		foreach (string file in GetFiles()) {
			if (GetLevelNameFromPath(file).Equals(name)) {
				return file;
			}
		}
		return null;
	}

	// Gets a list of levels in either the game or the external directory
	[Obsolete]
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

	// Lists files in the LevelPacks dir
	[Obsolete]
	static string[] GetFiles() {
		List<string> files = new List<string>();
		foreach (string path in Directory.GetFiles(LevelDir)) {
			if (path.EndsWith(".lvl", StringComparison.Ordinal)) {
				files.Add(path);
			}
		}
		return files.ToArray();
	}

	// Finds the level name for the specified path
	[Obsolete]
	static string GetLevelNameFromPath(string path) {
		string level = GetDecodedLevelFromPath(path);
		if (string.IsNullOrEmpty(level)) {
			return null;
		}
		string[] spl = level.Split(new char[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);
		if (spl.Length < 1) {
			return null;
		}
		return spl[0];
	}

	// Loads a file and contains errors
	static string ReadFile(string path) {
		string file = null;
		try {
			file = File.ReadAllText(path, Encoding.UTF8);
		} catch (Exception e) {
			Debug.LogError("Failed to load file: " + e.Message);
		}
		return file;
	}

	// Decodes a level string from the specified absolute path from base64 to plain utf-8
	static string GetDecodedLevelFromPath(string path) {
		if (!path.EndsWith(".lvl", StringComparison.Ordinal)) {
			return null;
		}
		string decoded = ReadFile(path);
		if (decoded == null) {
			return null;
		}
		// Levels can be stored in Base64 (UTF-8), so parse them if they are
		if (decoded.StartsWith("encode", StringComparison.OrdinalIgnoreCase)) {
			if (!Encoding.UTF8.TryParseBase64(decoded.Substring("encode".Length), out decoded)) {
				Debug.LogError("Failed to load level file. Unable to parse base64");
				return null;
			}
		}
		return decoded;
	}

	// Creates a list of level packs found in the LevelDir
	public static LevelPack[] GetLevelPacks() {
		List<LevelPack> packs = new List<LevelPack>();
		foreach (string pack in Directory.GetDirectories(LevelDir)) {
			if (!File.Exists(pack + "/Pack.txt")) {
				Debug.LogWarning("Level pack has no level info file: " + pack);
				continue;
			}
			if (!Directory.Exists(pack + "/Levels/")) {
				Debug.LogWarning("Level pack has no level pack folder: " + pack);
				continue;
			}
			string[] packInfo = ReadPackInfoFile(pack);
			if (packInfo == null || packInfo.Length < 2) {
				Debug.LogWarning("Invalid level pack: " + pack);
				continue;
			}
			LevelPack levelPack = new LevelPack(packInfo[0], packInfo[1]);  // packInfo: [NAME, VERSION_CREATED_IN]
			foreach (string level in Directory.GetFiles(pack + "/Levels/")) {
				if (!level.EndsWith(".lvl", StringComparison.Ordinal)) {
					continue;
				}
				LevelData levelData = ReadLevelFromAbsoluteLevelFile(level);
				levelPack.AddLevel(levelData);
			}
			packs.Add(levelPack);
		}
		return packs.ToArray();
	}

	// Loads a level from a pure file path not relative to the LevelPacks folder
	static LevelData ReadLevelFromAbsoluteLevelFile(string absoluteFile) {
		if (!File.Exists(absoluteFile)) {
			return null;
		}
		string contents = File.ReadAllText(absoluteFile);
		if (string.IsNullOrEmpty(contents)) {
			return null;
		}
		string decoded = GetDecodedLevelFromPath(absoluteFile);
		if (string.IsNullOrEmpty(decoded)) {
			return null;
		}
		return LoadLevelFromString(decoded);
	}

	static string[] ReadPackInfoFile(string packPath) {
		return File.ReadAllLines(packPath + "/Pack.txt");
	}

}