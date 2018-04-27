// Essentially, these classes allow us to create an abstract system of customizing individual data values for
// tiles in the level without having to edit the level data file (.lvl)

public abstract class TileDataCustomizationWrapper {

	readonly string name;
	readonly string displayName;

	protected TileDataCustomizationWrapper(string name, string displayName) {
		this.name = name;
		this.displayName = displayName;
	}

	public abstract string GetPrompt();

	public virtual bool GetHasLimitedValues() {
		return false;
	}

	public virtual object[] GetValidValues() {
		return new object[0];
	}

	public abstract void Set(DataHandler dataHandler, object input);
	public abstract ISerializableData Get(DataHandler dataHandler);
	public abstract string GetSerialized(object input);
	public abstract object GetDeserialized(string serialized);

	public string GetName() {
		return name;
	}

	public string GetDisplayName() {
		return displayName;
	}

}

public abstract class TileDataCustomization<T> : TileDataCustomizationWrapper where T : ISerializableData, new() {

	protected TileDataCustomization(string name, string displayName) : base(name, displayName) {
	}

	public override object[] GetValidValues() {
		T[] vals = GetTypedValidValues();
		if (vals.Length < 1) {
			return new object[0];
		}
		object[] outArray = new object[vals.Length];
		for (int i = 0; i < vals.Length; i++) {
			outArray[i] = vals[i];
		}
		return outArray;
	}

	public virtual T[] GetTypedValidValues() {
		return new T[0];
	}

	public T GetTyped(DataHandler dataHandler) {
		return (T) Get(dataHandler);
	}

	public override void Set(DataHandler dataHandler, object input) {
		T t = new T();
		t.Set(input);
		dataHandler.Set(GetName(), t.Serialize());
	}

	public override ISerializableData Get(DataHandler dataHandler) {
		T t = new T();
		t.Deserialize(dataHandler.Get(GetName()));
		return t;
	}

	public override string GetSerialized(object input) {
		T t = new T();
		t.Set(input);
		return t.Serialize();
	}

	public override object GetDeserialized(string serialized) {
		T t = new T();
		t.Deserialize(serialized);
		return t.Get();
	}

}

public class TileDataInt : TileDataCustomization<SerializableInt> {

	public TileDataInt(string name, string displayName) : base(name, displayName) {
	}

	public override string GetPrompt() {
		return "Integer";
	}

}

public class TileDataFloat : TileDataCustomization<SerializableFloat> {

	public TileDataFloat(string name, string displayName) : base(name, displayName) {
	}

	public override string GetPrompt() {
		return "Decimal Number";
	}

}

public class TileDataString : TileDataCustomization<SerializableString> {

	public TileDataString(string name, string displayName) : base(name, displayName) {
	}

	public override string GetPrompt() {
		return "Text";
	}

}

public class TileDataBool : TileDataCustomization<SerializableBool> {

	public TileDataBool(string name, string displayName) : base(name, displayName) {
	}

	public override string GetPrompt() {
		return "";
	}

}

public class TileDataChoose : TileDataCustomization<SerializableString> {

	readonly SerializableString[] options;

	public TileDataChoose(string name, string displayName, SerializableString[] options) : base(name, displayName) {
		this.options = options;
	}

	public override string GetPrompt() {
		return "Choose";
	}

	public override bool GetHasLimitedValues() {
		return true;
	}

	public override SerializableString[] GetTypedValidValues() {
		return options;
	}

}