using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour {
	public static SaveManager main {
		get {
			return _main;
		}
	}
	private static SaveManager _main;
	void Awake() {
		_main = this;
	}

	public void SetFolderName(string s) {
		folderName = s;
	}
	public string folderName="Untitled";
	[ContextMenu("Save")]
	public void SaveToFile() {
		string fullSavePath = Path.Combine( Application.persistentDataPath, folderName);
		if (!Directory.Exists(fullSavePath) ) {
			Directory.CreateDirectory(fullSavePath);
		}
		string mainJsonPath = Path.Combine(fullSavePath, "image.json");
		SaveData dataForJson = new SaveData();
		dataForJson.layers = LayerManager.main.GetLayerDatas();
		dataForJson.backgroundColor = LayerManager.main.compositingCamera.backgroundColor;
		// TO DO: write json. needs more LitJson

		var layers = LayerManager.main.GetLayers();
		int width = LayerManager.main.textureMapWidth;
		Texture2D readingTex = new Texture2D(width, width/2, TextureFormat.ARGB32, false);
		foreach (var layer in layers) {
			RenderTexture.active = layer.myRenderTexture;
			readingTex.ReadPixels(new Rect(0,0,width,width/2), 0, 0, false);
			byte[] bytes = readingTex.EncodeToPNG();
			string thisImagePath = Path.Combine(fullSavePath, layer.layerName+".png");
			File.WriteAllBytes(thisImagePath, bytes);
		}
	}
}

[System.Serializable]
public class SaveData {
	public SaveLayerData[] layers;
	public Color backgroundColor = Color.blue;
}

public class SaveLayerData {
	public string name = "New Layer";
	public Color color = Color.white;
	public bool renderable = true;
	public bool paintable = true;
}