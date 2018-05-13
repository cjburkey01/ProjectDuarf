using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LevelPack {

	public readonly string name;
	public readonly string version;
	readonly Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();

	public LevelPack(string name, string version) {
		this.name = name;
		this.version = version;
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
		return levels.Values.ToArray();
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