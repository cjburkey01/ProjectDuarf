using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GuiConfirmDelete : GameGUI {

	public static GuiConfirmDelete INSTANCE { private set; get; }

	public Button buttonDelete;
	public Button buttonCancel;
	public Text confirmation;
	public string defaultText = "Are you sure you wish to delete \"{0}\" permanently?";
	public string levelName;

	bool init;

	public GuiConfirmDelete() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiConfirmDelete";
	}

	public override void OnShow(GameGUI previousGui) {
		if (levelName == null || !LevelIO.LevelExists(false, levelName)) {
			StartCoroutine(Remove());
			return;
		}
		if (!init) {
			init = true;
			buttonDelete.onClick.AddListener(OnDeleteClick);
			buttonCancel.onClick.AddListener(Close);
			confirmation.text = string.Format(defaultText, levelName);
		}
	}

	IEnumerator Remove() {
		yield return new WaitForSeconds(0.1f);
		Close();
	}

	public void OnDeleteClick() {
		if (!LevelIO.DeleteLevel(levelName)) {
			Debug.LogError("Unable to delete level: " + levelName);
		} else {
			Debug.Log("Deleted level: " + levelName);
		}
		Close();
	}

	void Close() {
		GUIHandler.ShowGui(GuiGameLoadLevel.INSTANCE);
	}

}