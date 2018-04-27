using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuiEditorMenu : GameGUI {

	public static GuiEditorMenu INSTANCE { private set; get; }

	public LevelEditorHandler levelEditorHandler;
	public Button buttonNew;
	public Button buttonLoad;
	public Button buttonSave;
	public Button buttonPicker;
	public Button buttonExit;
	public Button buttonExitNoSave;

	private bool init = false;

	public GuiEditorMenu() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiEditorMenu";
	}

	public override void OnShow(GameGUI previousGui) {
		if (!init) {
			init = true;
			buttonNew.onClick.AddListener(OnNewClick);
			buttonLoad.onClick.AddListener(OnLoadClick);
			buttonSave.onClick.AddListener(OnSaveClick);
			buttonPicker.onClick.AddListener(OnPickerClick);
			buttonExit.onClick.AddListener(OnExitClick);
			buttonExitNoSave.onClick.AddListener(OnNoSaveExitClick);
		}
		buttonSave.interactable = levelEditorHandler.LevelLoaded;
		buttonPicker.interactable = levelEditorHandler.LevelLoaded;
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
		SceneManager.LoadScene(0);
	}

}