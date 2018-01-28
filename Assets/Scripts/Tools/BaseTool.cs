using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTool : MonoBehaviour {
	protected GlobeLayer currentLayer {
		get {
			return LayerManager.main.activeLayer;
		}
	}
	public virtual bool usesMapCoords {
		get {
			return true;
		}
	}
	public virtual void StartUsingTool(Vector2 point) {
	}
	public virtual void DragTool(Vector2 point, float pressure, bool isGlobeView) {
	}
	public virtual void EndTool(Vector2 point) {
	}
}
