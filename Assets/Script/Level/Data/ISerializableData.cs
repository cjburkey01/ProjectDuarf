public interface ISerializableData {

	void Set(object value);
	object Get();

	void Deserialize(string input);
	string Serialize();

}