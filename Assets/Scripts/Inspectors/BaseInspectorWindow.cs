using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInspectorWindow : MonoBehaviour {

	void Start() {
		InspectorManager.main.RegisterWindow(this);
		gameObject.SetActive(false);
	}
	public virtual void Activate(object target) {
		gameObject.SetActive(true);
	}
	public void Deactivate() {
		gameObject.SetActive(false);
	}

	public abstract bool CanEditField(object fieldData);
}
