using UnityEngine;

public class TilePowerup : TilePlaceholder {

	public TilePowerup(string prefabPath, string name, string iconPath) : base(prefabPath, name, iconPath, false) {
	}

	public override bool DoCustomInstantiation(bool init, Vector2 pos, float z, out GameObject obj) {
		bool f = base.DoCustomInstantiation(init, pos, z, out obj);
		if (init && obj != null) {
			obj.name = "PowerUp " + GetResourceName() + " " + pos;
		}
		return f;
	}

}