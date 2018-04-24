public class PowerUpSpeed : PowerUp {

	public float time = 15.0f;
	public float speed = 5.0f;

	float defaultSpeed;

	public override string GetUniqueName() {
		return "PowerUpSpeed";
	}

	public override void OnPickup(Player ply) {
		defaultSpeed = ply.PlayerMotor.moveSpeed;
		ply.PlayerMotor.moveSpeed = speed;
	}

	public override float GetLength() {
		return time;
	}

	public override void OnExpire(Player ply) {
		ply.PlayerMotor.moveSpeed = defaultSpeed;
	}

}