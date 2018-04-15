using UnityEngine;

public class GameLevelHandler : MonoBehaviour {

	public static GameLevelHandler INSTANCE { private set; get; }
	public bool LevelLoaded { private set; get; }

	private readonly LevelData level = new LevelData("LevelUnknown");
	private bool hasLevel = false;

	public GameLevelHandler() {
		INSTANCE = this;
	}

	public void LoadLevel(bool resource, string name) {
		if (hasLevel) {
			level.OnDestroy();
		}
		LevelLoaded = LevelIO.LoadLevel(true, resource, false, transform, level, name);
		hasLevel = true;
	}

	void Update() {
		if (!hasLevel) {
			return;
		}
		if (LevelLoaded) {
			level.OnInit();
			LevelLoaded = false;
		}
		level.OnUpdate();
	}

}