using System.Collections.Generic;
using UnityEngine;

public class LevelPack {

	public readonly string name;
	readonly Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();

	public LevelPack(string name) {
		this.name = name;
	}

	public void InstantiateLevel(bool init, bool fullColliders, Transform levelParent, string name) {
		LevelData levelData = GetLevel(name);
		if (levelData == null) {
			Debug.LogError("Level " + name + " not found in level pack " + name);
			return;
		}
		levelData.InstantiateLevel(init, fullColliders, levelParent);
	}

	public void AddLevel(string levelName, LevelData level) {
		if (levels.ContainsKey(levelName)) {
			Debug.LogError("Level pack " + name + " already contains a level by the name " + levelName);
			return;
		}
		levels.Add(levelName, level);
	}

	public LevelData GetLevel(string levelName) {
		LevelData dat;
		if (!levels.ContainsKey(levelName) || !levels.TryGetValue(levelName, out dat)) {
			return null;
		}
		return dat;
	}

}