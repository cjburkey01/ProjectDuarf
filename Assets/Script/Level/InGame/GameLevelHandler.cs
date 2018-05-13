using UnityEngine;

public class GameLevelHandler : MonoBehaviour {

	public static GameLevelHandler INSTANCE { private set; get; }
	public bool LevelLoaded { private set; get; }
	public LevelData LoadedLevel { private set; get; }

	bool hasLevel;

	public GameLevelHandler() {
		INSTANCE = this;
	}

	public void LoadLevel(bool resource, string name) {
		if (resource) {
			Debug.LogError("Tried to load a resource level, but failed");
			return;
		}
		LoadedLevel = LevelIO.LoadLevel(name);
		LoadedLevel.InstantiateLevel(true, false, transform);
		hasLevel = LevelLoaded;
	}

	public void UnloadLevel() {
		if (LevelLoaded) {
			LoadedLevel.ClearWorld(transform);
			LevelLoaded = false;
			hasLevel = false;
		}
	}

	void Update() {
		if (!LevelLoaded) {
			return;
		}
		if (hasLevel) {
			LoadedLevel.OnInit();
			hasLevel = false;
		}
		LoadedLevel.OnUpdate();
	}

}