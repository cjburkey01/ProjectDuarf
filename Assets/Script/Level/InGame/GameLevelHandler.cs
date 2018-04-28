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
		LevelLoaded = LevelIO.LoadLevel(true, false, transform, level, name);
		hasLevel = LevelLoaded;
	}

	public void UnloadLevel() {
		if (LevelLoaded) {
			level.OnDestroy(transform);
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