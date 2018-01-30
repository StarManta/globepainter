using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayer : MonoBehaviour {
	public GlobeLayer myLayer;
	public Image colorIndicator;
	public Text layerNameTextObj;
	public Toggle layerEnabled;

	public void SyncWithLayer() {
		colorIndicator.color = myLayer.color;
		layerNameTextObj.text = myLayer.layerName;
		layerEnabled.isOn = myLayer.renderable;
	}
	public void SetPaintable(bool isPaintable) {
		myLayer.paintable = isPaintable;
	}
	public void SetRenderable(bool isRenderable) {
		myLayer.renderable = isRenderable;
	}
	public string layerName {
		get {
			return myLayer.layerName;
		}
		set {
			myLayer.layerName = value;
			layerNameTextObj.text = value;
		}
	}

	public void OpenInspector() {
		InspectorManager.main.ActivateWindowFor(this);
	}
}
