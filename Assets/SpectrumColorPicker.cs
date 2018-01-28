using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpectrumColorPicker : MonoBehaviour, IPointerDownHandler {
	RectTransform rectTransform {
		get {
			return (RectTransform)transform;
		}
	}
	Vector3[] corners = new Vector3[4];
	Rect cachedRect = Rect.zero;
	Rect rect {
		get {
			if (cachedRect == Rect.zero) {
				rectTransform.GetWorldCorners(corners);
				cachedRect = new Rect(corners[0], corners[2] - corners[0]);
			}
			return cachedRect;
		}
	}

	public ColorPicker picker;

	public void OnPointerDown(PointerEventData eventData) {
		Texture2D tex = (Texture2D)GetComponent<RawImage>().texture;
		Vector2 uv = GetNormCoords(eventData.position);
		Color rtn = tex.GetPixel((int)(tex.width * uv.x), (int)(tex.height * uv.y));
		picker.PickColorAndClose(rtn);
	}

	Vector2 GetNormCoords(Vector2 screenCoords) {
		return new Vector2(	(screenCoords.x - rect.xMin) / rect.width,
			(screenCoords.y - rect.yMin) / rect.height);
	}

}
