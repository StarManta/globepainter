using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour {
	public RectTransform mapView;
	public RectTransform middleBar;
	public RectTransform globeView;
	public RectTransform toolbar;
	public RectTransform layerWindow;
	private RectTransform rectTransform;
	public int middleBarHeight = 32;

	void Start () {
		rectTransform = GetComponent<RectTransform>();
		SetWindowLayout();
	}
	[ContextMenu("Recalc")]
	public void SetWindowLayout() {
		rectTransform = GetComponent<RectTransform>();
		float mapTextureAspectRatio = (5f+Mathf.Sin(Mathf.PI/6f))/(3f*Mathf.Sin(Mathf.PI/3f));
		float screenWidth = rectTransform.sizeDelta.x;
		float screenHeight = rectTransform.sizeDelta.y;
		float mapViewHeight = screenWidth / mapTextureAspectRatio;
		float bottomPanelHeight = mapViewHeight + middleBarHeight;
		float globeViewSize = screenHeight - bottomPanelHeight - middleBarHeight;
		float toolbarWidth = (screenWidth - globeViewSize) * 0.5f;

		mapView.anchoredPosition = new Vector2(0f, 0f);
		mapView.sizeDelta = new Vector2(screenWidth, mapViewHeight);
		globeView.anchoredPosition = new Vector2(0.5f * (screenWidth - globeViewSize) , bottomPanelHeight);
		globeView.sizeDelta = new Vector2(globeViewSize, globeViewSize);
		toolbar.anchoredPosition = new Vector2(0f, mapViewHeight);
		toolbar.sizeDelta = new Vector2(globeView.anchoredPosition.x, screenHeight - mapViewHeight);
		layerWindow.anchoredPosition = new Vector2(0.5f * (screenWidth + globeViewSize), toolbar.anchoredPosition.y);
		layerWindow.sizeDelta = toolbar.sizeDelta;
		middleBar.anchoredPosition = new Vector2(toolbarWidth, mapViewHeight);
		middleBar.sizeDelta = new Vector2(screenWidth - toolbarWidth * 2, middleBarHeight);
	}

	void OnValidate() {
		SetWindowLayout();
	}
}
