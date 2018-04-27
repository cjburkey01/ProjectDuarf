using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

	public TileData tile;
	public float length = 15.0f;

	public abstract string GetUniqueName();

	public abstract void OnPickup(Player ply);

	/// <summary>
	/// 	Called when the powerup has existed for its length and is going to be removed from the player (is not called when <c>GetLength()</c> returns any value less than
	/// 	or equal to 0.0f
	/// </summary>
	public virtual void OnExpire(Player ply) {
	}

	/// <summary>
	/// 	Called for every frame that the powerup is in effect on the player
	/// </summary>
	public virtual void OnTick(Player ply) {
	}

	public abstract Sprite GetSprite();

	/// <summary>
	///		If true, then <c>OnExpire</c> and <c>OnTick</c> are not called, but the powerup is not destroyed when it is enabled
	/// </summary>
	public virtual bool GetIsReusable() {
		return false;
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