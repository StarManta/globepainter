using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LayerManager : MonoBehaviour {
	public static LayerManager main {
		get {
			return _main;
		}
	}
	private static LayerManager _main;
	void Awake() {
		_main = this;
		OnSyncLayers += () => {};
		layers = new List<GlobeLayer>();



		gridLayer = AddNewLayer();
		gridLayer.paintable = false;
		gridLayer.renderable = true;
		gridLayer.layerName = "Grid";

		activeLayer = AddNewLayer();
		activeLayer.layerName = "Terrain";
		activeLayer.renderable = true;
		activeLayer.paintable = true;

		compositingRT = new RenderTexture(textureMapWidth, textureMapWidth/2, 32);
		compositingRT.name = "Compositing RT";
		float mapTextureAspectRatio = (5f+Mathf.Sin(Mathf.PI/6f))/(3f*Mathf.Sin(Mathf.PI/3f));
		compositingCamera.targetTexture = compositingRT;
		compositingCamera.transform.localPosition = GetCenter(-5f);

		transform.localPosition = Vector3.zero;

	}
	void Start() {
		GlobeData.main.Generate();
		GlobeData.main.SetTexture(compositingRT);
		EtchLines();
		CompositeLayers();
	}
	public GameObject layerPrefab;

	void EtchLines() {
		LineBrush.main.rendererEnabled = true;
		for (int f=0; f<GlobeFace.allFaces.Length; f++) {
			LineBrush.main.SetUVPositions(GlobeFace.allFaces[f].cornerUV);
			gridLayer.RenderLayer();
		}
		LineBrush.main.rendererEnabled = false;
	}

	public GlobeLayer[] GetLayers() {
		return layers.ToArray();
	}
	private List<GlobeLayer> layers;
	public GlobeLayer gridLayer;
	public int textureMapWidth = 4096;

	public float width = 4f;
	public float height = 2f;
	[ContextMenu("Add")]
	public GlobeLayer AddNewLayer() {
		GameObject newGO = Instantiate(layerPrefab);
		GlobeLayer rtn = newGO.GetComponent<GlobeLayer>();
		layers.Add(rtn);
		rtn.transform.SetParent(transform, false);
		rtn.Initialize(textureMapWidth, textureMapWidth/2);
		if (activeLayer == null) {
			SelectLayer(rtn);
		}
		OnSyncLayers();
		return rtn;
	}

	public GlobeLayer activeLayer;
	public void SelectLayer(GlobeLayer newLayer) {
		activeLayer = newLayer;
		OnSyncLayers();
	}
	public event Action OnSyncLayers;

	public Camera compositingCamera;
	public RenderTexture compositingRT;
	public void CompositeLayers() {
		compositingCamera.Render();
	}

	public Vector3 UVToWorldPoint(Vector2 uv) {
		return new Vector3(uv.x * width, uv.y * height, transform.position.z);
	}
	public Vector3 GetCenter(float zCoord) {
		return new Vector3(0.5f * width, 0.5f * height, transform.position.z + zCoord);
	}
	public Vector3 GetScale() {
		return new Vector3(width, height, 1f);
	}
}
