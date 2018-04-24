using UnityEngine;

public class Player : PowerupHandler {

	public PlayerMovement PlayerMotor { private set; get; }

	Vector3 spawnPos;

	void Start() {
		PlayerMotor = GetComponent<PlayerMovement>();
		if (PlayerMotor == null) {
			Debug.LogError("Player lacking PlayerMovement component");
			enabled = false;
		}
		spawnPos = transform.position;
	}

	void Update() {
		HandlePowerupTick();
	}

	void OnTriggerEnter2D(Collider2D collision) {
		PowerUp pu = collision.gameObject.GetComponent<PowerUp>();
		if (pu != null) {
			AddPowerup(pu);
			Destroy(pu.gameObject);
		}
	}

	public void Kill() {
		EventObject.EventSystem.TriggerEvent(new PlayerDeathEvent(this));
		PlayerMotor.SetVelocity(Vector2.zero);
		transform.position = spawnPos;
	}

}