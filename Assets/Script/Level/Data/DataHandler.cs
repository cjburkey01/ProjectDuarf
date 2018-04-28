using System.Collections.Generic;

public class DataHandler {

	readonly Dictionary<string, string> data = new Dictionary<string, string>();

	public bool GetHasKey(string key) {
		return data.ContainsKey(key);
	}

	public void Set(string key, string value) {
		if (data.ContainsKey(key)) {
			data[key] = value;
		} else {
			data.Add(key, value);
		}
	}

	public byte GetByte(string key) {
		string at = Get(key);
		byte res;
		if (at == null || !byte.TryParse(at, out res)) {
			return byte.MinValue;
		}
		return res;
	}

	public short GetShort(string key) {
		string at = Get(key);
		short res;
		if (at == null || !short.TryParse(at, out res)) {
			return short.MinValue;
		}
		return res;
	}

	public int GetInt(string key) {
		string at = Get(key);
		int res;
		if (at == null || !int.TryParse(at, out res)) {
			return int.MinValue;
		}
		return res;
	}

	public long GetLong(string key) {
		string at = Get(key);
		long res;
		if (at == null || !long.TryParse(at, out res)) {
			return long.MinValue;
		}
		return res;
	}

	public float GetFloat(string key) {
		string at = Get(key);
		float res;
		if (at == null || !float.TryParse(at, out res)) {
			return float.MinValue;
		}
		return res;
	}

	public double GetDouble(string key) {
		string at = Get(key);
		double res;
		if (at == null || !double.TryParse(at, out res)) {
			return double.MinValue;
		}
		return res;
	}

	public bool GetBool(string key) {
		string at = Get(key);
		return at != null && at.ToLower().Trim().Equals("true");
	}

	public string Get(string key) {
		string value;
		if (data.TryGetValue(key, out value)) {
			return value;
		}
		return null;
	}

	public string Serialize() {
		string ret = "";
		foreach (KeyValuePair<string, string> pair in data) {
			ret += pair.Key + "=" + pair.Value + ",";
		}
		return ret.Substring(0, (ret.Length > 0) ? ret.Length - 1 : 0);
	}

	public void Deserialize(string serialized) {
		data.Clear();
		string[] ser = serialized.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		foreach (string dat in ser) {
			string[] spl = dat.Split(new char[] { '=' }, System.StringSplitOptions.RemoveEmptyEntries);
			if (spl.Length != 2) {
				continue;
			}
			Set(spl[0], spl[1]);
		}
	}

	public override string ToString() {
		return Serialize();
	}

	public override bool Equals(object obj) {
		if (obj == null || !(obj is DataHandler)) {
			return false;
		}
		return (obj as DataHandler).data.Equals(data);
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}

}