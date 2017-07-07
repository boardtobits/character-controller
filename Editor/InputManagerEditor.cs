using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor {

	public override void OnInspectorGUI(){

		InputManager im = target as InputManager;
	
		EditorGUI.BeginChangeCheck ();

		base.OnInspectorGUI ();

		if (EditorGUI.EndChangeCheck ()) {
			im.RefreshTracker ();
		}
	}
}
