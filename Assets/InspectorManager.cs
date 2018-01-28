using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectorManager : MonoBehaviour {
	public static InspectorManager main {
		get {
			return _main;
		}
	}
	private static InspectorManager _main;
	void Awake() {
		_main = this;
		allInspectors = new List<BaseInspectorWindow>();
	}

	public List<BaseInspectorWindow> allInspectors;

	public void RegisterWindow(BaseInspectorWindow win) {
		allInspectors.Add(win);
	}

	public void ActivateWindow(BaseInspectorWindow activatingWindow) {
		foreach (BaseInspectorWindow thisWin in allInspectors) {
			thisWin.gameObject.SetActive(thisWin == activatingWindow);
		}
	}
}
