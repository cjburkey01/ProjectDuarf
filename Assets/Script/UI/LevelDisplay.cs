using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour {
	
	public string LevelName { private set; get; }

	public ILevelSelector levelLoader;

	private Text text;

	void Start() {
		text = GetComponentInChildren<Text>();
		if (LevelName != null) {
			text.text = LevelName;
		}
	}

	public void SetLevelName(string level) {
		LevelName = level;
		if (text != null) {
			text.text = level;
		}
	}

	public void OnClick() {
		levelLoader.SetSelected(LevelName);
	}
	
}

public interface ILevelSelector {

	void SetSelected(string level);

}