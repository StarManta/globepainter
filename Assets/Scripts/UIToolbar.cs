using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToolbar : MonoBehaviour {
	public static UIToolbar main {
		get {
			return _main;
		}
	}
	private static UIToolbar _main;
	void Awake() {
		_main = this;
	}

	void Start() {
		SetBrushSize(0f);
		RefreshToolbarDisplay();
	}
		
	private List<UIToolbarButton> allToolbarButtons;
	public UIToolbarButton selectedToolButton;
	public BaseTool primaryTool {
		get {
			if (selectedToolButton == null) return null;
			return selectedToolButton.tool;
		}
	}
	public BaseTool secondaryTool;

	public void RegisterButton(UIToolbarButton button) {
		if (allToolbarButtons == null) allToolbarButtons = new List<UIToolbarButton>();
		allToolbarButtons.Add(button);
	}
	public void SelectTool(UIToolbarButton button) {
		selectedToolButton = button;
		RefreshToolbarDisplay();
	}

	public void RefreshToolbarDisplay() {
		foreach (var but in allToolbarButtons) {
			but.GetComponent<CanvasGroup>().alpha = (but == selectedToolButton ? 1f : 0.75f);
		}
	}

	public float minBrushSize = 0.001f;
	public float maxBrushSize = 0.1f;
	public void SetBrushSize(float sliderInput) {
		Brush.main.size = Mathf.Lerp(minBrushSize, maxBrushSize, sliderInput);
		LineBrush.main.size = Mathf.Lerp(minBrushSize, maxBrushSize, sliderInput);
	}
}
