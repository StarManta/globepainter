using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenColorPicker : MonoBehaviour, IPointerDownHandler {
	public UILayer layer;
	public void OnPointerDown(PointerEventData eventData) {
		ColorPicker.main.PickColor((col) => {
			Debug.Log("Picking color "+col);
			layer.myLayer.color = col;
		} 
		);
	}
}
