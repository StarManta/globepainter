using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour {
	public static Label CreateLabel(string text, Vector2 mapPos) {
		GameObject newGO = new GameObject("Label: "+text);
		newGO.transform.SetParent(LayerManager.main.transform, false);
		Label newLabel = newGO.AddComponent<Label>();
		newLabel.labelText = text;
		newLabel.mapPosition = mapPos;
		newLabel.icon = newLabelIcon;
		return newLabel;
	}
	public Vector2 mapPosition {
		get {
			return _mapPosition;
		}
		set {
			_mapPosition = value;
			transform.position = LayerManager.main.UVToWorldPoint(_mapPosition);
		}
	}
	private Vector2 _mapPosition;
	public string labelText {
		get {
			return _labelText;
		}
		set {
			_labelText = value;
		}
	}
	private string _labelText;

	public SpriteRenderer spriteRenderer {
		get {
			if (_spriteRenderer == null) {
				_spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); 
			}
			if (_spriteRenderer == null) {
				_spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				_spriteRenderer.sprite = newLabelIcon;
			}
			return _spriteRenderer;
		}
	}
	private SpriteRenderer _spriteRenderer;

	public Sprite icon {
		get {
			return spriteRenderer.sprite;
		}
		set {
			spriteRenderer.sprite = value;
			newLabelIcon = value;
		}
	}
	public static Sprite newLabelIcon;
}
