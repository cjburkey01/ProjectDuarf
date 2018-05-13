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

	static bool LoadLevelFromFile(LevelData level, string name) {
		if (name.EndsWith(".lvl", StringComparison.Ordinal)) {
			name.Substring(0, name.Length - 4);
		}
		if (!GetLevelExists(name)) {
			Debug.LogError("Level does not exist: " + name);
			return false;
		}
		string fileName = GetLevelFileFromName(name);
		Debug.Log("Loading level from file: " + fileName);
		string decoded = GetDecodedLevelFromPath(fileName);
		if (string.IsNullOrEmpty(decoded)) {
			Debug.LogError("Failed to load level from file: " + fileName);
			return false;
		}
		LoadLevelFromString(level, decoded.Trim());
		return true;
	}

	static void LoadLevelFromString(LevelData level, string serialized) {
		level.Deserialize(serialized);
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

	public static bool LoadLevel(LevelData level, string levelName) {
		return LoadLevelFromFile(level, levelName);
	}

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

	public static bool GetLevelExists(string name) {
		foreach (string lvl in GetLevels(false)) {
			if (lvl.Equals(name)) {
				return true;
			}
		}
		return false;
	}

	static string GetLevelFileFromName(string name) {
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

	static string[] GetFiles() {
		List<string> files = new List<string>();
		foreach (string path in Directory.GetFiles(LevelDir)) {
			if (path.EndsWith(".lvl", StringComparison.Ordinal)) {
				files.Add(path);
			}
		}
		return files.ToArray();
	}

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

	static string ReadFile(string path) {
		string file = null;
		try {
			file = File.ReadAllText(path, Encoding.UTF8);
		} catch (Exception e) {
			Debug.LogError("Failed to load file: " + e.Message);
		}
		return file;
	}

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

}