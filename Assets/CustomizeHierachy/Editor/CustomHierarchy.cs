using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class CustomHierarchy : MonoBehaviour
{
	public static HierarchyChanged _ItemChanges;
	private static string _ItemsChangeSOName = "Changeditems";
	private static string[] _IconDefaultNames = {"GameObject Icon","Prefab Icon", "sv_label_1", "sv_label_2", "sv_label_3" };

    private static Vector2 offset = new Vector2(0, 0);
    #region Color Variables
    public static bool useDefaultColors = true;

	public static Color defaultColor = Color.black;
    public static Color inactiveColor = Color.black;
    public static Color prefabColor = Color.black;
    public static Color selectedColor = Color.black;

    private static Vector3 baseColor = new Vector3(56, 56, 56);
    private static Vector3 baseInactiveColor = new Vector3(49, 49, 49);
    private static Vector3 baseSelectedColor = new Vector3(44, 93, 135);
	#endregion

	static CustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;

		SetChangedItems();
		GetAllChangedItems();

		LoadStartingTextures();
    }

    public static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        //UseDefaultColors(instanceID, selectionRect);
		UseDicColors(instanceID, selectionRect);
	}

	/*
    private static void UseDefaultColors(int instanceID, Rect selectionRect)
    {

	ci._Icon
		SetChangedItems();

		if (_ItemChanges == null) return;

		if (EditorUtility.InstanceIDToObject(instanceID) == null)
		{
			if (_ItemChanges.Contains(instanceID)) _ItemChanges.RemoveItem(instanceID); 
			return;
		}
		
		if (_ItemChanges.Contains(instanceID))
		{
			//Checks for a null Changeinformation
			ChangeInformation ci = _ItemChanges.GetDetails(instanceID);
			if (ci == null)
			{
				Debug.Log("CI is null for: " +  instanceID);
				return;
			}


			Color bgColor = ci._BGColor;

			if (Selection.instanceIDs.Contains(instanceID)) bgColor = ci._SelectedColor;

			var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (!go.activeInHierarchy) bgColor = ci._InactiveColor;

			//Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
			float posX = selectionRect.position.x - selectionRect.xMin;
			float posY = selectionRect.position.y;
			float sizeX = selectionRect.size.x + selectionRect.xMin + 32;
			float sizeY = selectionRect.size.y;
			Rect offsetRect = new Rect(new(posX, posY), new(sizeX, sizeY));

			var content = EditorGUIUtility.ObjectContent(EditorUtility.InstanceIDToObject(instanceID), null);

			//EditorGUI.DrawRect(selectionRect, bgColor);
			EditorGUI.DrawRect(offsetRect, bgColor);

			GetIcon(ci, ref go, ref content);
			content.text = go.name;
			content.tooltip = content.text;

			EditorGUI.LabelField(selectionRect, content, new GUIStyle()
			{
				normal = new GUIStyleState() { textColor = ci._TextColor },
				fontStyle = ci._FontStyle
			});	
		}
	}
	*/

	public static void UseDicColors(int instanceID, Rect selectionRect)
	{
		// checks if there is an object with the id
		var obj = EditorUtility.InstanceIDToObject(instanceID);
		if (obj == null) return;

		// checks if that object has a HI attached to it
		GameObject go = obj as GameObject;
		HierarchyItems hi = CustomHierarchyUtils.GetHierarchyFromObject(ref go);
		if (hi == null) return;

		bool usedBG = false;
		bool skipColor = false;

		// Background Color based Items
		Color bgColor = new Color(-1, -1, -1);
		if (go.activeInHierarchy)
		{
			if (hi._useDefaultSelectedColor && Selection.activeObject == go)
			{
				bgColor = hi._SelectedColor;
			}
			else skipColor = true;

			// will use custom colors
			if (hi._useDefaultBG && Selection.activeObject != go)
			{
				bgColor = hi._BGColor;
				skipColor = false;
			}
		}
		else
		{
			if (hi._useDefaultInactiveColor)
			{
				bgColor = hi._InactiveColor;
			}
		}

		// null checks bg color
		// and skip if not selected
		if (bgColor != new Color(-1, -1, -1) && !skipColor)
		{
			EditorGUI.DrawRect(selectionRect, bgColor);
			usedBG = true;
		}

		// text based Items
		FontStyle style = FontStyle.Normal;
		GUIStyleState styleState = new GUIStyleState();
		bool useStyle = false;

		if (hi._useDefaultText)
		{
			styleState.textColor = hi._TextColor;
			style = hi._FontStyle;
			useStyle = true;
		}

		var content = EditorGUIUtility.ObjectContent(EditorUtility.InstanceIDToObject(instanceID), null);
		if (skipColor) { content.text = ""; }

		GetIcon(hi, ref go, ref content);

		

		// last section
		// content also includes icons/ images
		if (usedBG)
		{
			EditorGUI.LabelField(selectionRect, content, new GUIStyle()
			{
				normal = styleState,
				fontStyle = style,
				fontSize = hi._fontSize,
			});
		}
		else if (useStyle)
		{
			EditorGUI.DrawRect(selectionRect, CustomHierarchyUtils.ConvertFromBRGB(baseColor, 1));

			content.text = hi.gameObject.name;

			EditorGUI.LabelField(selectionRect, content, new GUIStyle()
			{
				normal = styleState,
				fontStyle = style,
				font = hi._font,
				fontSize = hi._fontSize,
			});
		}
	}

	private static void GetBGColor()
	{

	}


	public static void GetAllChangedItems()
	{
		string sceneName = SceneManager.GetActiveScene().name;
		Object[] allItems = Resources.FindObjectsOfTypeAll(typeof(HierarchyItems));
		HierarchyItems[] allHItems = new HierarchyItems[allItems.Length];
		for (int i = 0; i < allHItems.Length; i++)
		{
			allHItems[i] = allHItems[i] as HierarchyItems;
		}
		_ItemChanges.Add(sceneName, allHItems);
	}

	#region helper
	public static Color ConvertFromBRGB(Vector3 rgb, float alpha)
    {
		Vector3 brgb = new Vector3(rgb.x / 255, rgb.y / 255, rgb.z / 255);
        return new Color(brgb.x, brgb.y, brgb.z, alpha);
	}

	private static void GetIcon(HierarchyItems hi, ref GameObject go, ref GUIContent content)
	{
		switch (hi._IconType)
		{
			case HierarchyItems.IconType.NONE: content.image = null; break;
			default: case HierarchyItems.IconType.DEFAULT: break;
			case HierarchyItems.IconType.COMPONENT:
				Component component = go.GetComponentCount() > 1 ? go.GetComponentAtIndex(1) : go.GetComponentAtIndex(0);
				System.Type type = component.GetType();
				GUIContent cont = EditorGUIUtility.ObjectContent(null, type);
				content.image = cont.image;
				break;
			case HierarchyItems.IconType.TREE: break;
			case HierarchyItems.IconType.CUSTOM:
				if (hi._Icon == null) return;
				content.image = (Texture)hi._Icon;
				break;
		}
	}

	public static void SetChangedItems()
	{
		if (_ItemChanges != null) return;
		string[] temp = AssetDatabase.FindAssets(_ItemsChangeSOName);
		string path = AssetDatabase.GUIDToAssetPath(temp[0]);
		_ItemChanges = (HierarchyChanged)AssetDatabase.LoadAssetAtPath(path, typeof(HierarchyChanged));
		EditorUtility.SetDirty(_ItemChanges);
	}

	private static void LoadStartingTextures() 
	{
		for (int i = 0; i < _IconDefaultNames.Length; i++)
		{
			_ItemChanges.AddTexture(EditorGUIUtility.IconContent(_IconDefaultNames[i]).image as Texture2D);
		}
	}

	#endregion
}