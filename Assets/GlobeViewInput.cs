using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlobeViewInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
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
	public Camera globeCamera;
	public void OnPointerDown(PointerEventData eventData) {
		Vector2 normCoords = GetNormCoords(eventData.position);
		BaseTool tool = GetActiveTool(eventData);
		if (tool != null) {
			if (tool.usesMapCoords) {
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(globeCamera.ViewportPointToRay(normCoords), out hit) ) {
					Vector2 mapCoords = hit.textureCoord;
					tool.StartUsingTool(mapCoords);
				}
			}
			else {
				tool.StartUsingTool(normCoords);
			}
		}
	}
	public void OnDrag(PointerEventData eventData){
		Vector2 normCoords = GetNormCoords(eventData.position);

		BaseTool tool = GetActiveTool(eventData);
		if (tool != null) {
			if (tool.usesMapCoords) {
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(globeCamera.ViewportPointToRay(normCoords), out hit) ) {
					Vector2 mapCoords = hit.textureCoord;
					tool.DragTool(mapCoords, 1f, true);
				}
			}
			else {
				tool.DragTool(normCoords, 1f, true);
			}
		}
	}
	public void OnPointerUp(PointerEventData eventData) {
		Vector2 normCoords = GetNormCoords(eventData.pressPosition);

		BaseTool tool = GetActiveTool(eventData);
		if (tool != null) {
			if (tool.usesMapCoords) {
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(globeCamera.ViewportPointToRay(normCoords), out hit) ) {
					Vector2 mapCoords = hit.textureCoord;
					UIToolbar.main.primaryTool.EndTool(mapCoords);
				}
			}
			else {
				tool.EndTool(normCoords);
			}
		}
	}
	bool IsPrimary(PointerEventData data) {
		if (data.button == PointerEventData.InputButton.Left && !(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) )) 
		{
			return true;
		}
		return false;
	}
	Vector2 GetNormCoords(Vector2 screenCoords) {
		return new Vector2(	(screenCoords.x - rect.xMin) / rect.width,
			(screenCoords.y - rect.yMin) / rect.height);
	}
	BaseTool GetActiveTool(PointerEventData eventData) {
		if (IsPrimary(eventData) && UIToolbar.main.primaryTool != null) {
			return UIToolbar.main.primaryTool;
		}
		else if (UIToolbar.main.secondaryTool != null) {
			return UIToolbar.main.secondaryTool;
		}
		return null;
	}
}
