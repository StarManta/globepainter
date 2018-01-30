using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenColorPicker : MonoBehaviour, IPointerDownHandler {
	public UILayer layer;
	public void OnPointerDown(PointerEventData eventData) {
		InspectorManager.main.ActivateWindowFor(this);
	}
	public void SetColor(Color col) {
		layer.myLayer.color = col;
	}
}
