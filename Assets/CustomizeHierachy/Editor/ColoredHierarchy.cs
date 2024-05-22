using UnityEditor;
using UnityEngine;

public class ColoredHierarchy : EditorWindow
{
	
	private const int spacer = 10;

	[MenuItem("Window/ColoredHierarchy")]

	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(ColoredHierarchy));
	}

	private void OnGUI()
	{
		if (CustomHierarchy._ItemChanges == null) return;

		GameObject go = Selection.activeGameObject;
		//int currentID = -1;
		if (go == null) return;
		//currentID = go.GetInstanceID();

		// check if the scene is in the current selection
		SetAddRemoveBtns();

		// If an object does not have a HI then it stops moving forward in the script
		if (!go.TryGetComponent<HierarchyItems>(out var hi)) return;

		GUILayout.Space(spacer);
		GUILayout.Space(spacer);

		// Presets
		GUILayout.Label("Presets");

		hi._Name = GUILayout.TextField(hi._Name);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Preset"))
		{
			// add this it a list of presets
			CustomHierarchy._ItemChanges.AddPreset(hi._Name, hi);
		}
		if (GUILayout.Button("Remove Preset"))
		{
			// remove this from a set of presets
			CustomHierarchy._ItemChanges.RemovePreset(hi._Name);
		}
		if (GUILayout.Button("Clear Presets"))
		{
			// clear all user made presets
			CustomHierarchy._ItemChanges.Clear();

			hi._presetSelection = 0;
			hi._prevPresetSelection = 0;

			CustomHierarchy._ItemChanges.AddPreset("Preset_1", CustomHierarchyUtils.GetPresetOne());
			CustomHierarchy._ItemChanges.AddPreset("Preset_2", CustomHierarchyUtils.GetPresetTwo());
			CustomHierarchy._ItemChanges.AddPreset("Preset_3", CustomHierarchyUtils.GetPresetThree());
		}
		//if (GUILayout.Button("Debug"))
		//{
		//	CustomHierarchy._ItemChanges.PrintPresetCount();
		//}

		GUILayout.EndHorizontal();
		if (CustomHierarchy._ItemChanges.Preset.Count != 0)
		{
			string[] presetList = CustomHierarchy._ItemChanges.GetPresetArray();

			hi._presetSelection = EditorGUILayout.Popup(hi._presetSelection, presetList);

			if (hi._presetSelection != hi._prevPresetSelection && hi._presetSelection != 0)
			{
				hi._prevPresetSelection = hi._presetSelection;

				// do something to set the preset
				CustomHierarchy._ItemChanges.OneOfUs(presetList[hi._presetSelection], hi);
			}
		}

		GUILayout.Space(spacer);

		LoadItemSelected(hi);
	}


	private void SetAddRemoveBtns()
	{
		GUILayout.BeginHorizontal();


		if (GUILayout.Button("Add Customization"))
		{
			GameObject selected = Selection.activeGameObject;
			var hi = CustomHierarchyUtils.AddHierarchyItemsToObject(ref selected);
			CustomHierarchy._ItemChanges.Add(hi);
		}
		if (GUILayout.Button("Remove Customization"))
		{
			GameObject selected = Selection.activeGameObject;
			CustomHierarchyUtils.RemoveHierarchyItemsFromObject(ref selected);
		}

		//if (GUILayout.Button("Debug"))
		//{
		//	CustomHierarchy.PrintCIKV();
		//}

		GUILayout.EndHorizontal();
	}

	private void LoadItemSelected(HierarchyItems hi)
	{



		ColorSelection(hi);
		if (hi._useDefaultBG || hi._useGradient || hi._useDefaultInactiveColor || hi._useDefaultSelectedColor)
			hi._useFullWidth = EditorGUILayout.Toggle("Use Wide Highlights", hi._useFullWidth);

		GUILayout.Space(spacer);
		GUILayout.Space(spacer);

		if (!hi._useDefaultText)
		{
			hi._useDefaultText = EditorGUILayout.Toggle("Use Custom Text", hi._useDefaultText);
		}
		else
		{
			hi._useDefaultText = EditorGUILayout.Toggle("Use Default Text", hi._useDefaultText);
			TextSelection(hi);
		}

		IconSeleciton(hi);
	}

	private void ColorSelection(HierarchyItems hi)
	{
		GUILayout.Space(spacer);

		// set local bools for gradients and background
		// they also will be for chacking if the last frame is in the privous state
		bool useGradientPrev = hi._useDefaultBG;
		bool useBackgroundColorPrev = hi._useGradient;

		// toggle for bg
		// toggle for gradient
		if (!hi._useDefaultBG) hi._useDefaultBG = EditorGUILayout.Toggle("Use Background Color", hi._useDefaultBG);
		else
		{
			hi._useDefaultBG = EditorGUILayout.Toggle("Turn Off BGC", hi._useDefaultBG);
			hi._BGColor = EditorGUILayout.ColorField("Background Color", hi._BGColor);
		}
		if (!hi._useGradient) hi._useGradient = EditorGUILayout.Toggle("Use Gradient", hi._useGradient);
		else
		{
			hi._useGradient = EditorGUILayout.Toggle("Turn Off Gradient", hi._useGradient);
			hi._PrevGradient = (EditorGUILayout.GradientField("Gradient", hi._ColorGradient));
		}

		// show items
		if (useGradientPrev && hi._useDefaultBG) // I hate way i did this
		{
			if (hi._useGradient)
			{
				hi._useGradient = true;
				hi._useDefaultBG = false;
			}
		}
        else if (useBackgroundColorPrev && hi._useGradient)
		{
			if (hi._useDefaultBG)
			{
				hi._useGradient = !true;
				hi._useDefaultBG = !false;
			}
		}

		if (hi._PrevGradient.colorKeys.Length != hi._ColorGradient.colorKeys.Length)
		{
			hi._Gradient = null;
			Debug.Log("Gradient was changed - color Keys");
		}
		else
		{
			// check each color value
			if (hi._ColorGradient.Evaluate(10) != hi._PrevGradient.Evaluate(10) ||
				hi._ColorGradient.Evaluate(24) != hi._PrevGradient.Evaluate(24) ||
				hi._ColorGradient.Evaluate(2) != hi._PrevGradient.Evaluate(2) ||
				hi._ColorGradient.Evaluate(30) != hi._PrevGradient.Evaluate(30))
			{
				hi._Gradient = null;
				Debug.Log("Gradient was changed - color values");
			}
		}

		if (hi._Gradient == null)
		{
			hi._Gradient = CustomHierarchyUtils.Create(hi._ColorGradient);
			hi._ColorGradient = hi._PrevGradient;
		
		}


		hi._useDefaultInactiveColor = EditorGUILayout.Toggle("Custom Inactive Color", hi._useDefaultInactiveColor);
		if (hi._useDefaultInactiveColor) 
		{
			hi._InactiveColor = EditorGUILayout.ColorField("Inactive Color", hi._InactiveColor);
		}

		hi._useDefaultSelectedColor = EditorGUILayout.Toggle("Custom Selected Color", hi._useDefaultSelectedColor);
		if (hi._useDefaultSelectedColor) 
		{
			hi._SelectedColor = EditorGUILayout.ColorField("Selected Color", hi._SelectedColor);
		}
	}

	private void TextSelection(HierarchyItems hi)
	{
		if (GUILayout.Button("Reset Font"))
		{
			hi._font = Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");
		}

		hi._fontSize = EditorGUILayout.IntField("Font Size", hi._fontSize);
		hi._font = EditorGUILayout.ObjectField("Font", hi._font, typeof(UnityEngine.Font), true) as UnityEngine.Font;
		hi._TextColor = EditorGUILayout.ColorField("Text Color", hi._TextColor);
		hi._FontStyle = (UnityEngine.FontStyle)EditorGUILayout.EnumPopup("Font Style", hi._FontStyle);
		hi._textAnchor = (UnityEngine.TextAnchor)EditorGUILayout.EnumPopup("Text Position", hi._textAnchor);
	}
	
	private void IconSeleciton(HierarchyItems hi)
	{
		GUILayout.Space(spacer);
		GUILayout.Space(spacer);
		hi._IconType = (HierarchyItems.IconType)EditorGUILayout.EnumPopup("Icon Type", hi._IconType);
		switch (hi._IconType)
		{
			default:
			case HierarchyItems.IconType.NONE: break;
			case HierarchyItems.IconType.TREE: break;
			case HierarchyItems.IconType.DEFAULT: break;
			case HierarchyItems.IconType.COMPONENT:
				// gets the first non-transform or prettyobject icon
				GameObject go = hi.gameObject;

				//Component component = go.GetComponentAtIndex(0); // why is this an error
				Component component = go.GetComponents<Component>()[0];

				if (go.GetComponents<Component>().Length != 2)
				{
					component = go.GetComponents<Component>()[2];
				}

				System.Type type = component.GetType();
				GUIContent cont = EditorGUIUtility.ObjectContent(null, type);
				hi._Icon = cont.image;
				
				break;
			case HierarchyItems.IconType.CUSTOM:
				int selectedBox = -1;
				int width = 2;
				int height = CustomHierarchy._ItemChanges.Texture.Count / width;

				selectedBox = GUILayout.SelectionGrid(-1, CustomHierarchy._ItemChanges.Texture.ToArray(), width, GUILayout.Height(32 * height));

				if (selectedBox != -1)
				{
					hi._Icon = CustomHierarchy._ItemChanges.Texture[selectedBox];
				}

				hi._Icon = (EditorGUILayout.ObjectField("Add New Icon", hi._Icon, typeof(Texture2D), true)) as Texture2D;
				break;
		}
	}
}
