using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour {
	public static Brush main {
		get {
			return _main;
		}
	}
	private static Brush _main;

	void Awake() {
		_main = this;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetUVPosition(Vector2 uv) {
		transform.localPosition = LayerManager.main.UVToWorldPoint(uv);
	}

	public float size {
		get {
			return transform.localScale.x;
		}
		set {
			transform.localScale = new Vector3(value, value, 1f);
		}
	}

	private SpriteRenderer spriteRenderer;
	public bool rendererEnabled {
		get {
			return spriteRenderer.enabled;
		}
		set {
			spriteRenderer.enabled = value;
		}
	}


}
