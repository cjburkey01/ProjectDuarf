using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuiMainMenu : GameGUI {

	public static GuiMainMenu INSTANCE { private set; get; }

	public GameHandler gameHandler;
	public Button buttonPlay;
	public Button buttonEdit;
	public Button buttonSettings;
	public Button buttonQuit;

	bool init;

	public GuiMainMenu() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GUIMainMenu";
	}

	public override void OnShow(GameGUI previousGui) {
		if (!init) {
			init = true;
			buttonPlay.onClick.AddListener(OnPlayClick);
			buttonEdit.onClick.AddListener(OnEditClick);
			buttonSettings.onClick.AddListener(OnSettingsClick);
			buttonQuit.onClick.AddListener(OnQuitClick);
		}
	}

	public void OnPlayClick() {
		GUIHandler.ShowGui(GuiGameLoadLevel.INSTANCE);
	}

	public void OnEditClick() {
		SceneManager.LoadScene(1);
	}

	public void OnSettingsClick() {
		
	}

	public void OnQuitClick() {
		Application.Quit();
	}

}