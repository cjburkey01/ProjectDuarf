using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuiEditorMenu : GameGUI {

	public static GuiEditorMenu INSTANCE { private set; get; }

	public LevelEditorHandler levelEditorHandler;
	public Button buttonLevels;
	public Button buttonNew;
	public Button buttonLoad;
	public Button buttonSave;
	public Button buttonExit;
	public Button buttonExitNoSave;

	bool init;

	public GuiEditorMenu() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiEditorMenu";
	}

	public override void OnShow(GameGUI previousGui) {
		if (!init) {
			init = true;
			buttonLevels.onClick.AddListener(OnLevelsClick);
			buttonNew.onClick.AddListener(OnNewClick);
			buttonLoad.onClick.AddListener(OnLoadClick);
			buttonSave.onClick.AddListener(OnSaveClick);
			buttonExit.onClick.AddListener(OnExitClick);
			buttonExitNoSave.onClick.AddListener(OnNoSaveExitClick);
		}
		buttonSave.interactable = levelEditorHandler.LevelLoaded;
		buttonExit.interactable = levelEditorHandler.LevelLoaded;
	}

	public void OnNewClick() {
		GUIHandler.ShowGui(GuiEditorNewLevel.INSTANCE);
	}

	public void OnLoadClick() {
		GUIHandler.ShowGui(GuiEditorLoadLevel.INSTANCE);
	}

	public void OnSaveClick() {
		if (!levelEditorHandler.LevelLoaded) {
			return;
		}
		levelEditorHandler.SaveLevel();
	}

	public void OnPickerClick() {
		if (!levelEditorHandler.LevelLoaded) {
			return;
		}
		GUIHandler.ShowGui(GuiEditorPicker.INSTANCE);
	}

	public void OnExitClick() {
		if (levelEditorHandler.LevelLoaded) {
			levelEditorHandler.SaveLevel();
		}
		OnNoSaveExitClick();
	}

	public void OnNoSaveExitClick() {
		if (levelEditorHandler.LevelLoaded) {
			GuiEditorLoadLevel.INSTANCE.levelPack = levelEditorHandler.LoadedLevel.LevelPack;
			levelEditorHandler.LoadLevel(null);
			GUIHandler.ShowGui(GuiEditorLoadLevel.INSTANCE);
		} else {
			SceneManager.LoadScene(0);
		}
	}

	public void OnLevelsClick() {
		GUIHandler.ShowGui(GuiEditorPackList.INSTANCE);
	}

}