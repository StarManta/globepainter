using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILayerManager : MonoBehaviour {
	public GameObject uiLayerPrefab;
	public Transform layerParent;

	private List<UILayer> uiLayers;

	void Start() {
		uiLayers = new List<UILayer>();
	}

	public void LateUpdate() {
		//first check length
		var layers = LayerManager.main.GetLayers();
		while (uiLayers.Count > layers.Length) {
			var l = uiLayers[0];
			uiLayers.RemoveAt(0);
			Destroy(l.gameObject);
		}
		while (uiLayers.Count < layers.Length) {
			GameObject newGO = Instantiate(uiLayerPrefab);
			newGO.transform.SetParent(layerParent, false);
			uiLayers.Add(newGO.GetComponent<UILayer>());
		}
		//then update contents
		for (int l=0;l<uiLayers.Count;l++) {
			uiLayers[l].myLayer = layers[l];
			uiLayers[l].SyncWithLayer();
		}
	}
}
