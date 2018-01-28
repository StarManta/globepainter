using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveViewTool : BaseTool {
	private Vector2 dragStart;
	public override bool usesMapCoords {
		get {
			return false;
		}
	}
	public override void StartUsingTool (Vector2 point)
	{
		dragStart = point;
		base.StartUsingTool (point);
	}
	public override void DragTool (Vector2 point, float pressure, bool isGlobeView)
	{
		Rotation.main.Rotate(point - dragStart);
		dragStart = point;
		base.DragTool (point, pressure, isGlobeView);
	}
}
