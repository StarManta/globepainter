using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelTool : BaseTool {
	Label activeLabel;
	public override bool usesMapCoords {
		get {
			return true;
		}
	}
	public override void StartUsingTool (Vector2 point)
	{
		activeLabel = Label.CreateLabel("New Label", point);
		base.StartUsingTool (point);
	}
	public override void DragTool (Vector2 point, float pressure, bool isGlobeView)
	{
		activeLabel.mapPosition = point;
		base.DragTool (point, pressure, isGlobeView);
	}
	public override void EndTool (Vector2 point)
	{
		activeLabel = null;
		base.EndTool (point);
	}
}
