using UnityEngine;

public abstract class TileInfo {

	public abstract string GetResourceName();
	public abstract string GetIconResourceName();

	/// <summary>
	///		Returns false if the default initialization is to be done
	/// </summary>
	public virtual bool DoCustomInstantiation(bool init, Vector2 pos, float z, out GameObject obj) {
		obj = null;
		return false;
	}

	public virtual void OnDestroy(TileData self) {
	}

	public virtual void OnUpdate(TileData self) {
	}

	public virtual void OnCreate(TileData self) {
	}

	/// <summary>
	///		This is extra data appended to the serialized tile data
	/// </summary>
	public virtual string Serialize(TileData self) {
		return "";
	}

	public virtual void Deserialize(string serialized, TileData data) {
	}

	public override bool Equals(object obj) {
		if (obj == null || !(obj is TileInfo)) {
			return false;
		}
		TileInfo other = obj as TileInfo;
		return other.GetResourceName().Equals(GetResourceName());
	}

	public override int GetHashCode() {
		return base.GetHashCode() + GetResourceName().GetHashCode() + GetIconResourceName().GetHashCode();
	}

	public override string ToString() {
		return base.ToString();
	}
}