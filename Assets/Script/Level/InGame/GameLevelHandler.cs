using UnityEngine;

public class GameLevelHandler : MonoBehaviour {

	public static GameLevelHandler INSTANCE { private set; get; }
	public bool LevelLoaded { private set; get; }

	readonly LevelData level = new LevelData();
	bool hasLevel;

	public GameLevelHandler() {
		INSTANCE = this;
	}

	public void LoadLevel(bool resource, string name) {
		if (resource) {
			Debug.LogError("Tried to load a resource level, but failed");
			return;
		}
		LevelLoaded = LevelIO.LoadLevel(level, name);
		level.InstantiateLevel(true, false, transform);
		hasLevel = LevelLoaded;
	}

	public void UnloadLevel() {
		if (LevelLoaded) {
			level.ClearWorld(transform);
			LevelLoaded = false;
			hasLevel = false;
		}
	}

	void Update() {
		if (!LevelLoaded) {
			return;
		}
		if (hasLevel) {
			level.OnInit();
			hasLevel = false;
		}
		level.OnUpdate();
	}

}