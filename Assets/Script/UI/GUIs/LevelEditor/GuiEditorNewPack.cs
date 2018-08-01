using UnityEngine;
using UnityEngine.UI;

public class GuiEditorNewPack : GameGUI {

	public static GuiEditorNewPack INSTANCE { private set; get; }

	public Color defaultColor = new Color(0.196f, 0.196f, 0.196f, 1.0f);
	public Color errorColor = new Color(0.545f, 0.062f, 0.062f, 1.0f);
	public string defaultString = "New Level Pack";
	public string invalidString = "Please enter a valid pack name";
	public string existsString = "A pack with that name already exists";
	public string errorString = "An unknown error occurred, please try again";
	public LevelEditorHandler levelEditorHandler;
	public InputField inputName;
	public Text error;
	public Button buttonCreate;
	public Button buttonCancel;

	bool valida;
	bool validb;

	public GuiEditorNewPack() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiEditorNewPack";
	}

	public override void OnShow(GameGUI previousGui) {
		buttonCreate.onClick.AddListener(OnCreateClick);
		buttonCancel.onClick.AddListener(OnCancelClick);
		error.text = defaultString;
		error.color = defaultColor;
	}

	public void OnCreateClick() {
		if (!valida || !validb) {
			return;
		}
		inputName.text = inputName.text.Trim();
		LevelIO.CreateLevelPack(inputName.text);
		Debug.Log("Creating level pack: " + inputName.text);
		GUIHandler.ShowGui(GuiEditorPackList.INSTANCE);
	}

	void Update() {
		valida = LevelIO.IsValidLevelName(inputName.text);
		validb = !LevelIO.GetLevelPackExists(inputName.text);
		buttonCreate.interactable = valida && validb;
		if (!valida) {
			error.text = invalidString;
		} else if (!validb) {
			error.text = existsString;
		} else {
			error.text = defaultString;
			error.color = defaultColor;
			return;
		}
		error.color = errorColor;
	}

	public void OnCancelClick() {
		buttonCreate.onClick.RemoveAllListeners();
		buttonCancel.onClick.RemoveAllListeners();
		GUIHandler.ShowGui(GuiEditorPackList.INSTANCE);
	}

}