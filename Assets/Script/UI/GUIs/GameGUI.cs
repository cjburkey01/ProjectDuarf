using UnityEngine;

public abstract class GameGUI : MonoBehaviour {
	
	public abstract string GetUniqueName();

	public virtual void OnShow(GameGUI previousGui) {
	}

	public virtual void OnHide(GameGUI nextGui) {
	}

	public override string ToString() {
		return GetUniqueName();
	}

	public override int GetHashCode() {
		return GetUniqueName().GetHashCode();
	}

	public override bool Equals(object other) {
		return base.Equals(other);
	}
}