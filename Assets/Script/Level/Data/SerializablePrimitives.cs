public abstract class SerializableData : ISerializableData {

	object value;

	protected SerializableData() {
	}

	protected SerializableData(object value) {
		this.value = value;
	}

	public void Set(object value) {
		this.value = value;
	}

	public object Get() {
		return value;
	}

	public abstract void Deserialize(string input);

	public virtual string Serialize() {
		return (Get() == null) ? "null" : Get().ToString();
	}

}

public class SerializableInt : SerializableData {

	public SerializableInt() {
	}

	public SerializableInt(int value) : base(value) {
	}

	public override void Deserialize(string input) {
		int ret;
		if (int.TryParse(input, out ret)) {
			Set(ret);
		}
	}

}

public class SerializableFloat : SerializableData {

	public SerializableFloat() {
	}

	public SerializableFloat(float value) : base(value) {
	}

	public override void Deserialize(string input) {
		float ret;
		if (float.TryParse(input, out ret)) {
			Set(ret);
		}
	}

}

public class SerializableLong : SerializableData {

	public SerializableLong() {
	}

	public SerializableLong(long value) : base(value) {
	}

	public override void Deserialize(string input) {
		long ret;
		if (long.TryParse(input, out ret)) {
			Set(ret);
		}
	}

}

public class SerializableDouble : SerializableData {

	public SerializableDouble() {
	}

	public SerializableDouble(double value) : base(value) {
	}

	public override void Deserialize(string input) {
		double ret;
		if (double.TryParse(input, out ret)) {
			Set(ret);
		}
	}

}

public class SerializableString : SerializableData {

	public SerializableString() {
	}

	public SerializableString(string value) : base(value) {
	}

	public override void Deserialize(string input) {
		Set(input);
	}

}

public class SerializableBool : SerializableData {

	public SerializableBool() {
	}

	public SerializableBool(bool value) : base(value) {
	}

	public override void Deserialize(string input) {
		Set(input != null && (input.ToLower().Trim() == "true"));
	}

	public override string Serialize() {
		return ((bool) Get()) ? "true" : "false";
	}

}