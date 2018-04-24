using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

	public abstract string GetUniqueName();

	public abstract void OnPickup(Player ply);

	/// <summary>
	/// 	Time, in seconds, of the effect. Set to a value less than or equal to 0.0f to disable
	/// </summary>
	public abstract float GetLength();

	/// <summary>
	/// 	Called when the powerup has existed for its length and is going to be removed from the player (is not called when GetLength() returns any value less than
	/// 	or equal to 0.0f
	/// </summary>
	public virtual void OnExpire(Player ply) {
	}

	/// <summary>
	/// 	Called for every frame that the powerup is in effect on the player
	/// </summary>
	public virtual void OnTick(Player ply) {
	}

	public override bool Equals(object other) {
		if (other == null || !(other is PowerUp)) {
			return false;
		}
		return (other as PowerUp).GetUniqueName().Equals(GetUniqueName());
	}

	public override int GetHashCode() {
		return GetUniqueName().GetHashCode();
	}

}