using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTool : BaseTool {
	Vector2 lastPoint = Vector2.zero;
	Vector2[] lastPointsCycle = new Vector2[2];
	int currentLPIndex = 0;
	public float interval = 0.01f;
	public override void StartUsingTool (Vector2 point)
	{
		lastPoint = point;
		for (int p=0;p<lastPointsCycle.Length;p++) {
			lastPointsCycle[p] = point;
		}
		DragTool(point, 1f, true);
		base.StartUsingTool (point);
	}
	public override void DragTool (Vector2 point, float pressure, bool isGlobeView)
	{
		float dist = (point - lastPoint).magnitude;
		if (point.x > lastPoint.x + 0.5f) {
			DragTool(point - new Vector2(1f, 0f) , pressure, isGlobeView);
			lastPoint += new Vector2(1f, 0f);
		}
		if (point.x < lastPoint.x - 0.5f) {
			DragTool(point + new Vector2(1f, 0f) , pressure, isGlobeView);
			lastPoint -= new Vector2(1f, 0f);
		}
		LineBrush.main.rendererEnabled = true;
		LineBrush.main.SetUVPositions(lastPointsCycle);
		currentLayer.RenderLayer();
		LineBrush.main.rendererEnabled = false;
		lastPoint = point;
		lastPointsCycle[currentLPIndex] = point;
		currentLPIndex = (currentLPIndex + 1) % lastPointsCycle.Length;
		base.DragTool (point, pressure, isGlobeView);
	}
}
