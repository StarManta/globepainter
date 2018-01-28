using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : BaseInspectorWindow {
	private static ColorPicker _main;
	public static ColorPicker main {
		get {
			return _main;
		}
	}
	void Awake() {
		_main = this;
	}

	RectTransform rectTransform {
		get {
			return (RectTransform)transform;
		}
	}


	public delegate void ColorPickCallback(Color newColor);
	public void PickColor(ColorPickCallback callback) {
		Activate();
		storedCallback = callback;
	}
	private ColorPickCallback storedCallback;

	public void PickColorAndClose(Color col) {
		if (storedCallback != null) storedCallback(col);
		storedCallback = null;
		Deactivate();
	}
}
