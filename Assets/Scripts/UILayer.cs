using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayer : MonoBehaviour {
	public GlobeLayer myLayer;
	public Image colorIndicator;
	public Text layerName;
	public Toggle layerEnabled;

	public void SyncWithLayer() {
		colorIndicator.color = myLayer.color;
		layerName.text = myLayer.layerName;
		layerEnabled.isOn = myLayer.renderable;
	}
	public void SetPaintable(bool isPaintable) {
		myLayer.paintable = isPaintable;
	}
	public void SetRenderable(bool isRenderable) {
		myLayer.renderable = isRenderable;
	}
}
