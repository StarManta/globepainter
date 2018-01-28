using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInspectorWindow : MonoBehaviour {

	void Start() {
		InspectorManager.main.RegisterWindow(this);
		gameObject.SetActive(false);
	}
	public void Activate() {
		InspectorManager.main.ActivateWindow(this);
	}
	public void Deactivate() {
		gameObject.SetActive(false);
	}
}
