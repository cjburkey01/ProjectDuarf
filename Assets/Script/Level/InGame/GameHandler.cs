using UnityEngine;

public class GameHandler : MonoBehaviour {

	public static bool IsPaused { private set; get; }

	public Camera placeholderCamera;

	public bool LevelLoaded {
		get {
			return GameLevelHandler.INSTANCE.LevelLoaded;
		}
	}
	
	void Start() {
		TileInitialization.Init();
		LevelIO.InitIO();
		GUIHandler.ShowGui(GuiMainMenu.INSTANCE);
	}

	void Update() {
		if (Input.GetButtonDown("Cancel")) {
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
		placeholderCamera.gameObject.SetActive(false);
	}

}