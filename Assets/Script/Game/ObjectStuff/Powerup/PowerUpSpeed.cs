using UnityEngine;

public class PowerUpSpeed : PowerUp {

	public float speed = 5.0f;

	float defaultSpeed;
	Sprite spr;

	void Awake() {
		spr = Resources.Load<Sprite>("Tile/Placeholder/PowerUp/Speed");
	}

	public override string GetUniqueName() {
		return "PowerUpSpeed";
	}

	public override void OnPickup(Player ply) {
		defaultSpeed = ply.PlayerMotor.moveSpeed;
		ply.PlayerMotor.moveSpeed = speed;
	}

	public override void OnExpire(Player ply) {
		ply.PlayerMotor.moveSpeed = defaultSpeed;
	}

	public override Sprite GetSprite() {
		return spr;
	}

}