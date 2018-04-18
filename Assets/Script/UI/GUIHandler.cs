public static class GUIHandler {

	public static GameGUI CurrentGUI { private set; get; }

	public static void ShowGui(GameGUI gui) {
		ReplaceCurrentGui(gui);
		if (gui != null) {
			gui.gameObject.SetActive(true);
			gui.OnShow(CurrentGUI);
		}
		CurrentGUI = gui;
	}

	public static void HideGui() {
		ShowGui(null);
	}

	static void ReplaceCurrentGui(GameGUI nextGui) {
		ReplaceSpecificGui(CurrentGUI, nextGui);
	}

	static void ReplaceSpecificGui(GameGUI gui, GameGUI nextGui) {
		if (gui != null) {
			gui.OnHide(nextGui);
			gui.gameObject.SetActive(false);
		}
	}

	public static bool IsShown(GameGUI gui) {
		return (CurrentGUI != null && CurrentGUI.GetUniqueName().Equals(gui.GetUniqueName()));
	}

	public static bool IsShown() {
		return CurrentGUI != null;
	}

}