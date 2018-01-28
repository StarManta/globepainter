using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTool : BaseTool {
	public override void StartUsingTool (Vector2 point)
	{
		DragTool(point, 1f, true);
		base.StartUsingTool (point);
	}
	public override void DragTool (Vector2 point, float pressure, bool isGlobeView)
	{
		Brush.main.SetUVPosition(point);
		Brush.main.rendererEnabled = true;
		currentLayer.RenderLayer();
		Brush.main.rendererEnabled = false;
		base.DragTool (point, pressure, isGlobeView);
	}
}
