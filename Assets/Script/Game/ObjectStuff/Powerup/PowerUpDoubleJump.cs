using UnityEngine;

public class PowerUpDoubleJump : PowerUp {

	public int jumps = 2;

	int defaultJumps;
	Sprite spr;

	void Awake() {
		spr = Resources.Load<Sprite>("Tile/Placeholder/PowerUp/DoubleJump");
	}

	public override string GetUniqueName() {
		return "PowerUpDoubleJump";
	}

	public override void OnPickup(Player ply) {
		defaultJumps = ply.PlayerMotor.maxJumps;
		ply.PlayerMotor.maxJumps = jumps;
	}

	public override void OnExpire(Player ply) {
		ply.PlayerMotor.maxJumps = defaultJumps;
	}

	public override Sprite GetSprite() {
		return spr;
	}

}