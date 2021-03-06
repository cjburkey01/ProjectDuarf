﻿using System.Collections.Generic;
using UnityEngine;

public class LevelPack {

	public readonly string path;
	public readonly string name;
	public readonly string version;
	readonly Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();
	public readonly LinkedList<string> levelOrder = new LinkedList<string>();

	public LevelPack(string path, string name, string version, string[] levelNames) {
		this.path = path;
		this.name = name;
		this.version = version;
		foreach (string levelName in levelNames) {
			levelOrder.AddLast(levelName);
		}
	}

	// TODO: FIX
	public void UpdateFile() {
		if (System.IO.File.Exists(path + "/Path.txt")) {
			System.IO.File.Delete(path + "/Path.txt");
		}
		string data = name + "\n" + version + "\n\n";
		foreach (string level in levelOrder) {
			data += level + "\n";
		}
		System.IO.File.WriteAllText(path + "/Pack.txt", data, System.Text.Encoding.UTF8);
	}

	public void InstantiateLevel(bool init, bool fullColliders, Transform levelParent, string name) {
		LevelData levelData = GetLevel(name);
		if (levelData == null) {
			Debug.LogError("Level " + name + " not found in level pack " + this.name);
			return;
		}
		levelData.InstantiateLevel(init, fullColliders, levelParent);
	}

	public void AddLevel(LevelData level) {
		if (levels.ContainsKey(level.Name)) {
			Debug.LogError("Level pack " + name + " already contains a level by the name " + level.Name);
			return;
		}
		levels.Add(level.Name, level);
	}

	public LevelData GetLevel(string levelName) {
		LevelData dat;
		if (!levels.ContainsKey(levelName) || !levels.TryGetValue(levelName, out dat)) {
			return null;
		}
		return dat;
	}

	public LevelData[] GetLevels() {
		LevelData[] levels = new LevelData[levelOrder.Count];
		int i = 0;
		foreach (string level in levelOrder) {
			levels[i++] = GetLevel(level);
		}
		return levels;
	}

	public bool HasLevel(string name) {
		return levels.ContainsKey(name);
	}

	public override bool Equals(object obj) {
		var pack = obj as LevelPack;
		return pack != null &&
			   name == pack.name &&
			   version == pack.version &&
			   EqualityComparer<Dictionary<string, LevelData>>.Default.Equals(levels, pack.levels);
	}

	public override int GetHashCode() {
		var hashCode = 1750720351;
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(version);
		hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, LevelData>>.Default.GetHashCode(levels);
		return hashCode;
	}

}