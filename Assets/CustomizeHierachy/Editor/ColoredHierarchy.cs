using PrettyHierarchy;
using System;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

//
//
//			UI Panel
//
//
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

		if (GUILayout.Button("Debug"))
		{
			CustomHierarchy.PrintCIKV();
		}

		LoadItemSelected(hi);

	}

	private void SetAddRemoveBtns()
	{
		if (GUILayout.Button("Add"))
		{
			GameObject selected = Selection.activeGameObject;
			var hi = CustomHierarchyUtils.AddHierarchyItemsToObject(ref selected);
			CustomHierarchy._ItemChanges.Add(hi);
		}
		if (GUILayout.Button("Remove"))
		{
			GameObject selected = Selection.activeGameObject;
			CustomHierarchyUtils.RemoveHierarchyItemsFromObject(ref selected);
		}
	}

	private void LoadItemSelected(HierarchyItems hi)
	{
		hi._useFullWidth = EditorGUILayout.Toggle("Use Wide Highlights", hi._useFullWidth);

		ColorSelection(hi);

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

	private void LoadItemSelection(int currentID)
	{
		/*
		if (CustomHierarchy._ItemChanges.ID.Contains(currentID))
		{
			//CustomHierarchy._ItemChanges.changedItems.TryGetValue(currentID, out var CI);
			ChangeInformation CI = CustomHierarchy._ItemChanges.GetDetails(currentID);

			if (CI == null)
			{
				Debug.Log("CI is Null for: " + currentID);
				return;
			}

			if (GUILayout.Button("test go"))
			{
				GameObject temp = CustomHierarchy._ItemChanges.GetGameObject(currentID);
				if (temp != null) Debug.Log("Gameobject has been found");
				else Debug.Log("Gameobject has been found");
			}


			ColorSelection(CI);
			TextSelection(CI);
			IconSelection(CI);
		}
		*/
	}

	private void ColorSelection(HierarchyItems hi)
	{
		GUILayout.Space(spacer);
		hi._useDefaultBG = EditorGUILayout.Toggle("Custom Background Color", hi._useDefaultBG);
		if (hi._useDefaultBG) 
		{
			hi._BGColor = EditorGUILayout.ColorField("Background Color", hi._BGColor);
		}
		
		GUILayout.Space(spacer);
		hi._useDefaultInactiveColor = EditorGUILayout.Toggle("Custom Inactive Color", hi._useDefaultInactiveColor);
		if (hi._useDefaultInactiveColor) 
		{
			hi._InactiveColor = EditorGUILayout.ColorField("Inactive Color", hi._InactiveColor);
		}

		GUILayout.Space(spacer);
		hi._useDefaultSelectedColor = EditorGUILayout.Toggle("Custom Selected Color", hi._useDefaultSelectedColor);
		if (hi._useDefaultSelectedColor) 
		{
			hi._SelectedColor = EditorGUILayout.ColorField("Selected Color", hi._SelectedColor);
		}
		GradientCreation(hi);
	}

	private void TextSelection(HierarchyItems hi)
	{
		if (GUILayout.Button("Reset Font"))
		{
			hi._font = Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");
		}

		hi._fontSize = EditorGUILayout.IntField("Font Size", hi._fontSize);
		hi._font = EditorGUILayout.ObjectField("Font", hi._font, typeof(UnityEngine.Font), true) as UnityEngine.Font;
		hi._FontStyle = (UnityEngine.FontStyle)EditorGUILayout.EnumPopup("Font Style", hi._FontStyle);
		hi._TextColor = EditorGUILayout.ColorField("Text Color", hi._TextColor);
	}
	
	private void IconSeleciton(HierarchyItems hi)
	{
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

				if (go.GetComponentCount() > 1)
				{
					for (int i = 0; i < go.GetComponentCount(); i++)
					{
						Component component = go.GetComponentCount() > 1 ? go.GetComponentAtIndex(1) : go.GetComponentAtIndex(0);
						System.Type type = component.GetType();
						GUIContent cont = EditorGUIUtility.ObjectContent(null, type);
						hi._Icon = cont.image;
					}
				}

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

	private void GradientCreation(HierarchyItems hi)
	{
		if (!hi._useGradient)
		{
			hi._useGradient = EditorGUILayout.Toggle("Use Gradient overlay", hi._useGradient);
			if (hi._Gradient != null) hi._Gradient = null;
		}
        else
        {
			hi._useGradient = EditorGUILayout.Toggle("Remove Gradient overlay", hi._useGradient);
			hi._ColorGradient = (EditorGUILayout.GradientField("Gradient", hi._ColorGradient));

			if (hi._PrevGradient != hi._ColorGradient)
			{
				hi._Gradient = null;
				Debug.Log("Gradients are Different");
			}

			if (hi._Gradient == null)
			{
				hi._Gradient = CustomHierarchyUtils.Create(hi._ColorGradient);
				Debug.Log("New Gradient Created");
			}
			hi._PrevGradient = hi._ColorGradient;
		}
    }

}
