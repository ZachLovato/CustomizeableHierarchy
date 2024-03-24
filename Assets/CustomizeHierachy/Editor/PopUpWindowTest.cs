using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUpWindowTest : EditorWindow
{
	bool useCustomBGColors = true;
	bool useCustomTextColors = true;

	[MenuItem("Window/PopUpWindowTest")]

	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(PopUpWindowTest));
	}

	void OnGUI()
	{
		GUILayout.Label("Hierarchy Color Setting", EditorStyles.boldLabel);

		//background color
		useCustomBGColors = !EditorGUILayout.BeginToggleGroup("Use Custom Colors", useCustomBGColors);

			CustomHierarchy.defaultColor = EditorGUILayout.ColorField("Default Color", CustomHierarchy.defaultColor);
			CustomHierarchy.prefabColor = EditorGUILayout.ColorField("Prefab Color", CustomHierarchy.prefabColor);
			CustomHierarchy.selectedColor = EditorGUILayout.ColorField("Selected Color", CustomHierarchy.selectedColor);
			CustomHierarchy.inactiveColor = EditorGUILayout.ColorField("Inactive Color", CustomHierarchy.inactiveColor);

			CustomHierarchy.useDefaultColors = useCustomBGColors;

		EditorGUILayout.EndToggleGroup();

		//text color
		useCustomTextColors = !EditorGUILayout.BeginToggleGroup("Use Custom Colors", useCustomTextColors);



		EditorGUILayout.EndToggleGroup();

	}



	// helpful
	//myString = EditorGUILayout.TextField("Text Field", myString);
	//groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
	//	myBool = EditorGUILayout.Toggle("Toggle", myBool);
	//	myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
	//	EditorGUILayout.EndToggleGroup();
}