using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeLayer : MonoBehaviour {
	public static GlobeLayer activeLayer;
	public static List<GlobeLayer> allLayers;

	void OnEnable() {
		if (allLayers == null) allLayers = new List<GlobeLayer>();
		allLayers.Add(this);
	}

	void OnDisable() {
		allLayers.Remove(this);
	}

	public RenderTexture myRenderTexture;
	public bool paintable = true;
	public Color color {
		set {
			_color = value;
			LayerManager.main.CompositeLayers();
		}
		get {
			return _color;
		}
	}
	private Color _color = Color.white;
	public string layerName {
		set {
			_layerName = value;
			gameObject.name = value;
			if (myRenderTexture != null) myRenderTexture.name = value;
		}
		get {
			return _layerName;
		}
	}
	private string _layerName = "New Layer";
	public bool renderable {
		get{
			return compositingLayerMesh.enabled;
		}
		set {
			compositingLayerMesh.enabled = value;
			LayerManager.main.CompositeLayers();
		}
	}
	public Camera layerCamera;
	public Transform compositingLayerParent;
	public MeshRenderer compositingLayerMesh;

	public void Initialize(int w, int h) {
		myRenderTexture = new RenderTexture(w, h, 32);
		myRenderTexture.name = layerName;
		layerCamera.targetTexture = myRenderTexture;
		compositingLayerMesh.material.mainTexture = myRenderTexture;
		layerCamera.transform.localPosition = LayerManager.main.GetCenter(-10f);
		compositingLayerParent.transform.localPosition = LayerManager.main.GetCenter(0f);
		compositingLayerParent.transform.localScale = LayerManager.main.GetScale();
	}

	public void RenderLayer() {
		layerCamera.Render();
		LayerManager.main.CompositeLayers();
	}
}
