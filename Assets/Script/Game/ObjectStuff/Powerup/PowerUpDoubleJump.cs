public class PowerUpDoubleJump : PowerUp {

	public int jumps = 2;
	public float time = 15.0f;

	int defaultJumps;

	public override string GetUniqueName() {
		return "PowerUpDoubleJump";
	}

	public override void OnPickup(Player ply) {
		defaultJumps = ply.PlayerMotor.maxJumps;
		ply.PlayerMotor.maxJumps = jumps;
	}

	public override float GetLength() {
		return time;
	}

	public override void OnExpire(Player ply) {
		ply.PlayerMotor.maxJumps = defaultJumps;
	}

}