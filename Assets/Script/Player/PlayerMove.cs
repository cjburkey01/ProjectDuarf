using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float skinWidth = 0.01f;
    public int raysForBottom = 5;
    public int raysForTop = 3;
    public int raysForRight = 5;
    public int raysForLeft = 5;

    private BoxCollider2D collider;
    private List<Vector2> rayPosBottom;
    private List<Vector2> rayPosTop;
    private List<Vector2> rayPosRight;
    private List<Vector2> rayPosLeft;

    void Start() {
        collider = GetComponent<BoxCollider2D>();
        UpdateCollisionDetection();
    }

    void Update() {
        
    }

    public void UpdateCollisionDetection() {
        rayPosBottom.Clear();
        rayPosTop.Clear();
        rayPosRight.Clear();
        rayPosLeft.Clear();

        float bottomSpacing = ((raysForBottom - 1.0f) / raysForBottom) * (collider.size.x - skinWidth * 2.0f);
        float pos = skinWidth;
        for (int i = 0; i < raysForBottom; i ++) {
            rayPosBottom.Add(new Vector2(pos, collider.max.y - skinWidth));
            pos += bottomSpacing;
            Debug.DrawRay();
            Debug.DrawLine(new Vector3(pos, collider.max.y - skinWidth, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), Color.red, 10.0f, false);
        }
    }

}