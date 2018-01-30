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

	public override bool CanEditField (object fieldData)
	{
		return (fieldData is OpenColorPicker);
	}
	OpenColorPicker myTarget;
	public override void Activate (object target)
	{
		myTarget = (OpenColorPicker)target;
		base.Activate (target);
	}


	public void PickColorAndClose(Color col) {
		myTarget.SetColor(col);
		Deactivate();
	}
}
