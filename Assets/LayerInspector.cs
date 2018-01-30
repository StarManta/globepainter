using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerInspector : BaseInspectorWindow {
	public override bool CanEditField (object fieldData)
	{
		return (fieldData is UILayer);
	}

	UILayer myTarget;
	public InputField nameTextField;

	public override void Activate (object target)
	{
		myTarget = (UILayer)target;
		nameTextField.text = myTarget.layerName;
		base.Activate (target);
	}

	public void SetName(string n) {
		myTarget.myLayer.layerName = n;
	}
}
