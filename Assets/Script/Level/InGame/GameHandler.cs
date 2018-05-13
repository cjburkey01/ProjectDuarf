using UnityEngine;

[RegisterEventHandlers]
public class GameHandler : MonoBehaviour {

	public static bool IsPaused { private set; get; }
	public static bool InGame { private set; get; }
	static GameHandler instance;

	public Camera placeholderCamera;

	public GameHandler() {
		instance = this;
	}

	public bool LevelLoaded {
		get {
			return GameLevelHandler.INSTANCE.LevelLoaded;
		}
	}

	static void RegisterEvents() {
		EventObject.EventSystem.AddListener<EventGameInit>(instance.OnStart);
	}

	void OnStart<T>(T e) where T : EventGameInit {
		GUIHandler.ShowGui(GuiMainMenu.INSTANCE);
	}

	void Update() {
		if (Input.GetButtonDown("Cancel") && InGame) {
			if (GUIHandler.IsShown()) {
				GUIHandler.HideGui();
			} else {
				GUIHandler.ShowGui(GuiMainMenu.INSTANCE);
			}
		}
		IsPaused = GUIHandler.IsShown();
	}

	public void LoadLevel(bool resource, string name) {
		placeholderCamera.gameObject.SetActive(true);
		GameLevelHandler.INSTANCE.LoadLevel(resource, name);
		PlayerController f = FindObjectOfType<PlayerController>();
		if (f == null || f.Destroyed) {
			Debug.LogError("Failed to locate player, this level cannot be played");
			GUIHandler.ShowGui(GuiMainMenu.INSTANCE);
			return;
		}
		GUIHandler.HideGui();
		InGame = true;
		placeholderCamera.gameObject.SetActive(false);
	}

	public void UnloadLevel() {
		PowerupHandler.ClearPowerups();
		GameLevelHandler.INSTANCE.UnloadLevel();
		placeholderCamera.gameObject.SetActive(true);
	}

}