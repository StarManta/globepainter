using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
	public static Rotation main {
		get {
			return _main;
		}
	}
	private static Rotation _main;
	void Awake() {
		_main = this;
	}
	Vector2 currentRotation = Vector2.zero;
	public float rotMultiplier = 90f;
	public void Rotate (Vector2 rotationDelta) {
		currentRotation += rotationDelta;
		currentRotation.y = Mathf.Clamp(currentRotation.y, -1f, 1f);
		transform.rotation = Quaternion.identity;
		transform.Rotate(Vector3.down, currentRotation.x * rotMultiplier, Space.World);
		transform.Rotate(Vector3.right, currentRotation.y * rotMultiplier, Space.World);
	}
}
