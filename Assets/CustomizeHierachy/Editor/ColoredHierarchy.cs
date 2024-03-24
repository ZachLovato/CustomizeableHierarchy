using System.Drawing;
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
		int currentID = -1;
		if (go == null) return;
		currentID = go.GetInstanceID();

		SetAddRemoveBtns(currentID);
		/*
        GUILayout.Label("Hierarchy Color Setting", EditorStyles.boldLabel);
		
		LoadItemSelection(currentID);

		if (GUILayout.Button("Clear Details in SO"))
		{
			CustomHierarchy._ItemChanges.Details.Clear();
		}
		*/

		// If an object does not have a HI then it stops moving forward in the script
		if (!go.TryGetComponent<HierarchyItems>(out var hi)) return;

		LoadItemSelected(hi);

	}

	private void SetAddRemoveBtns(int currentID)
	{
		if (GUILayout.Button("Add"))
		{
			GameObject selected = Selection.activeGameObject;
			var hi = CustomHierarchyUtils.AddHierarchyItemsToObject(ref selected);
			CustomHierarchy._ItemChanges.Add(hi);

			ChangeInformation changeInformation = new ChangeInformation();
			changeInformation._BGColor = CustomHierarchy.ConvertFromBRGB(new(56, 56, 56), 1);
			changeInformation._InactiveColor = CustomHierarchy.ConvertFromBRGB(new(49, 49, 49), 1);
			changeInformation._SelectedColor = CustomHierarchy.ConvertFromBRGB(new(44, 93, 135), 1);
			changeInformation._TextColor = UnityEngine.Color.white;//CustomHierarchy.ConvertFromBRGB(new(1,1,1),1);
			changeInformation._IconType = ChangeInformation.IconType.COMPONENT;
		}
		if (GUILayout.Button("Remove"))
		{
			GameObject selected = Selection.activeGameObject;
			CustomHierarchyUtils.RemoveHierarchyItemsFromObject(ref selected);
		}
	}

	private void LoadItemSelected(HierarchyItems hi)
	{
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
	}

	private void ColorSelection(ChangeInformation CI)
	{
		CI._BGColor = EditorGUILayout.ColorField("Background Color", CI._BGColor);
		CI._InactiveColor = EditorGUILayout.ColorField("Inactive Color", CI._InactiveColor);
		CI._SelectedColor = EditorGUILayout.ColorField("Selected Color", CI._SelectedColor);

	}

	private void TextSelection(HierarchyItems hi)
	{
		hi._FontStyle = (UnityEngine.FontStyle)EditorGUILayout.EnumPopup("Font Style", hi._FontStyle);
		hi._TextColor = EditorGUILayout.ColorField("Text Color", hi._TextColor);
	}

	private void TextSelection(ChangeInformation CI)
	{
		GUILayout.Space(spacer);
		GUILayout.Label("Text");
		CI._FontStyle = (UnityEngine.FontStyle)EditorGUILayout.EnumPopup("Font Style", CI._FontStyle);
		CI._TextColor = EditorGUILayout.ColorField("Text Color", CI._TextColor);
	}

	private void IconSeleciton(HierarchyItems hi)
	{
		if (hi.isAnyBGUsed() == false) return;

		GUILayout.Space(spacer);
		hi._IconType = (HierarchyItems.IconType)EditorGUILayout.EnumPopup("Icon Type", hi._IconType);
		switch (hi._IconType)
		{
			default:
			case HierarchyItems.IconType.NONE: break;
			case HierarchyItems.IconType.TREE: break;
			case HierarchyItems.IconType.DEFAULT: break;
			case HierarchyItems.IconType.COMPONENT:
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

	private void IconSelection(ChangeInformation CI)
	{
		GUILayout.Space(spacer);
		GUILayout.Label("Icon");
		CI._IconType = (ChangeInformation.IconType)EditorGUILayout.EnumPopup("Icon Type", CI._IconType);
		switch (CI._IconType)
		{
			default:
			case ChangeInformation.IconType.NONE: break;
			case ChangeInformation.IconType.TREE: break;
			case ChangeInformation.IconType.GAMEOBJECT: break;
			case ChangeInformation.IconType.COMPONENT:
				break;
			case ChangeInformation.IconType.CUSTOM:
				int selectedBox = -1;
				int width = 2;
				int height = CustomHierarchy._ItemChanges.Texture.Count / width;

				selectedBox = GUILayout.SelectionGrid(-1, CustomHierarchy._ItemChanges.Texture.ToArray(), width, GUILayout.Height(32 * height));

				if (selectedBox != -1)
				{
					CI._Icon = CustomHierarchy._ItemChanges.Texture[selectedBox];
				}

				//var temp = (EditorGUILayout.ObjectField("Icon", null, typeof(Texture2D), true)) as Texture2D;
				CI._Icon = (EditorGUILayout.ObjectField("Add New Icon", CI._Icon, typeof(Texture2D), true)) as Texture2D;

				//if (temp != null)
				//{
				//	CustomHierarchy._ItemChanges.AddTexture(temp);
				//}
				break;
		}
	}
}
