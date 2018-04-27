using UnityEngine;
using System.Collections.Generic;

public abstract class TileInfo {

	public abstract string GetResourceName();
	public abstract string GetIconResourceName();

	protected readonly List<TileDataCustomizationWrapper> tileDatas = new List<TileDataCustomizationWrapper>();

	protected TileInfo() {
	}

	protected TileInfo(TileDataCustomizationWrapper[] data) {
		tileDatas.AddRange(data);
	}

	/// <summary>
	///		Returns false if the default initialization is to be done
	/// </summary>
	public virtual bool DoCustomInstantiation(bool init, Vector2 pos, float z, TileData tile, out GameObject obj) {
		obj = null;
		return false;
	}

	public virtual void OnDestroy(TileData self) {
	}

	public virtual void OnUpdate(TileData self) {
	}

	public virtual void OnCreate(TileData self) {
	}

	public virtual void OnAdd(TileData self) {
	}

	/// <summary>
	///		This is extra data appended to the serialized tile data
	/// </summary>
	public virtual string Serialize(TileData self) {
		return "";
	}

	public virtual void Deserialize(string serialized, TileData data) {
	}

	/// <summary>
	/// 	Returns the list of possible editable values for this type
	/// </summary>
	public TileDataCustomizationWrapper[] GetCustomData() {
		return tileDatas.ToArray();
	}

	public virtual void OnDataUpdate(TileDataCustomizationWrapper changed, TileData self) {
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