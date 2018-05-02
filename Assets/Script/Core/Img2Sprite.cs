using System.IO;
using UnityEngine;

public static class NewBehaviourScript {
	
	public static Sprite LoadSprite(string externalFilepath, float pixelsPerUnit) {
		Texture2D texture = LoadTexture(externalFilepath);
		if (texture == null) {
			Debug.LogError("Failed to create sprite with texture: " + externalFilepath);
			return null;
		}
		return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.0f, 0.0f), pixelsPerUnit);
	}

	public static Texture2D LoadTexture(string externalFilepath) {
		Texture2D texture;
		byte[] data;
		if (File.Exists(externalFilepath)) {
			data = File.ReadAllBytes(externalFilepath);
			texture = new Texture2D(2, 2);
			if (texture.LoadImage(data)) {
				return texture;
			}
		}
		Debug.LogError("Failed to load external texture: " + externalFilepath);
		return null;
	}

}