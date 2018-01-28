using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBrush : MonoBehaviour {
	public static LineBrush main {
		get {
			return _main;
		}
	}
	private static LineBrush _main;

	void Awake() {
		_main = this;
		lineRenderer = GetComponent<LineRenderer>();
	}

	public LineRenderer lineRenderer;

	public float size {
		get {
			return lineRenderer.widthMultiplier;
		}
		set {
			lineRenderer.widthMultiplier = value;
		}
	}

	public bool rendererEnabled {
		get {
			return lineRenderer.enabled;
		}
		set {
			lineRenderer.enabled = value;
		}
	}
	public void SetUVPositions(params Vector2[] positions) {
		lineRenderer.positionCount = positions.Length;
		for (int p=0;p<positions.Length;p++) {
			lineRenderer.SetPosition(p, LayerManager.main.UVToWorldPoint(positions[p]));
		}
	}

}
