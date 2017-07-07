using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AxisKeys))]
public class AxisKeysDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
	
		//ensure override works on entire property
		EditorGUI.BeginProperty(position, label, property);

		//don't indent
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		//label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		//set position rects
		Rect posLabel = new Rect(position.x, position.y, 15, position.height);
		Rect posField = new Rect(position.x + 20, position.y, 50, position.height);
		Rect negLabel = new Rect(position.x + 75, position.y, 15, position.height);
		Rect negField = new Rect(position.x + 95, position.y, 50, position.height);

		//set labels
		GUIContent posGUI = new GUIContent("+");
		GUIContent negGUI = new GUIContent ("-");

		//draw fields
		EditorGUI.LabelField(posLabel, posGUI);
		EditorGUI.PropertyField (posField, property.FindPropertyRelative ("positive"), GUIContent.none);
		EditorGUI.LabelField(negLabel, negGUI);
		EditorGUI.PropertyField (negField, property.FindPropertyRelative ("negative"), GUIContent.none);

		//reset indent
		EditorGUI.indentLevel = indent;

		//end property
		EditorGUI.EndProperty();
	
	
	}

}
