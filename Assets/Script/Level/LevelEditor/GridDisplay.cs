using System.Collections.Generic;
using UnityEngine;

public class GridDisplay : MonoBehaviour {
	
	public Sprite gridPrefab;
	public float opacity = 0.25f;
	public Color previewColor = new Color(0.6f, 1.0f, 0.6f, 0.5f);
	public Color errorColor = new Color(1.0f, 0.6f, 0.6f, 0.5f);
	public float size;

	private int l = 0;
	private Dictionary<int, GameObject> grid = new Dictionary<int, GameObject>();
	private GameObject selected;
	private TileInfo tile;

	public void UpdateGrid(int width, float size) {
		int s = Mathf.FloorToInt(l / 2.0f);
		for (int x = -s; x <= s; x++) {
			for (int y = -s; y <= s; y++) {
				if (grid.ContainsKey(y * l + x)) {
					Destroy(grid[y * l + x]);
				}
			}
		}
		l = width;
		s = Mathf.FloorToInt(l / 2.0f);
		for (int x = -s; x <= s; x ++) {
			for (int y = -s; y <= s; y ++) {
				AddGridPiece(x, y, size);
			}
		}
	}

	public void MarkErrorColor() {
		if (selected == null) {
			return;
		}
		SpriteRenderer sprren = selected.GetComponent<SpriteRenderer>();
		if (sprren != null) {
			sprren.color = errorColor;
		}
	}

	public void ResetErrorColor() {
		if (selected == null) {
			return;
		}
		SpriteRenderer sprren = selected.GetComponent<SpriteRenderer>();
		if (sprren != null) {
			sprren.color = previewColor;
		}
	}

	private void AddGridPiece(int x, int y, float size) {
		this.size = size;
		GameObject obj = new GameObject("Grid " + x + ", " + y);
		SpriteRenderer render = obj.AddComponent<SpriteRenderer>();
		Color c = render.material.color;
		c.a = opacity;
		render.material.color = c;
		render.sprite = gridPrefab;
		obj.transform.localScale = new Vector3(size, size, 1.0f);
		obj.transform.parent = transform;
		obj.transform.localPosition = new Vector3(x * size, y * size, -9.0f);
		if (grid.ContainsKey(y * l + x)) {
			grid[y * l + x] = obj;
		} else {
			grid.Add(y * l + x, obj);
		}
	}

	public void SetTile(LevelData data, TileInfo tile) {
		this.tile = tile;
		if (selected != null) {
			Destroy(selected);
		}
		if (tile == null) {
			return;
		}
		selected = data.InstantiateTile(false, transform, new TileData(Vector2.zero, 0.0f, tile));
		if (selected == null) {
			return;
		}
		Destroy(selected.GetComponent<BoxCollider2D>());
		selected.transform.localPosition = new Vector3(0.0f, 0.0f, -8.0f);
		SpriteRenderer sprren = selected.GetComponent<SpriteRenderer>();
		if (sprren != null) {
			sprren.color = previewColor;
		}
	}

	public void Place(LevelData data, LevelEditorHandler leh) {
		Place(new Vector2(transform.position.x, transform.position.y), data, leh);
	}

	public void Place(Vector2 pos, LevelData data, LevelEditorHandler leh) {
		if (tile != null) {
			TileData dat;
			if (!data.AddTile(pos, transform.position.z, tile, out dat)) {
				MarkErrorColor();
				return;
			}
			data.InstantiateTile(false, leh.transform, dat);
			BoxCollider2D c = dat.Instantiated.GetComponent<BoxCollider2D>();
			if (c == null) {
				c = dat.Instantiated.AddComponent<BoxCollider2D>();
			}
			c.offset = new Vector2(0.0f, 0.0f);
			c.size = new Vector2(1.0f, 1.0f);
		}
	}

}