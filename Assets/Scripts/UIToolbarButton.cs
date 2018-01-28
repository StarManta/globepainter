using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToolbarButton : MonoBehaviour {
	private BaseTool _tool;
	public BaseTool tool {
		get {
			return _tool;
		}
	}
	void Start() {
		UIToolbar.main.RegisterButton(this);
		_tool = GetComponent<BaseTool>();
	}
	public void OnSelect() {
		UIToolbar.main.SelectTool(this);
	}
}
