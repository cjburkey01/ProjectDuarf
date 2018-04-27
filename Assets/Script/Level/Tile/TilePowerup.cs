using UnityEngine;

public class TilePowerup : TilePlaceholder {

	public TilePowerup(string prefabPath, string name, string iconPath) : base(prefabPath, name, iconPath, false) {
		tileDatas.Add(new TileDataFloat("length", "Duration (secs)"));
	}

	public override bool DoCustomInstantiation(bool init, Vector2 pos, float z, TileData tile, out GameObject obj) {
		bool f = base.DoCustomInstantiation(init, pos, z, tile, out obj);
		if (init && obj != null) {
			obj.name = "PowerUp " + GetResourceName() + " " + pos;
		}
		return f;
	}

	public override void OnAdd(TileData self) {
		PowerUp powerUp = self.Instantiated.GetComponent<PowerUp>();
		if (powerUp == null) {
			return;
		}
		powerUp.tile = self;
		if (self.Data.GetHasKey("length")) {
			powerUp.length = self.Data.GetFloat("length");
			Debug.Log("Powerup overridden to " + powerUp.length);
		}
	}

}