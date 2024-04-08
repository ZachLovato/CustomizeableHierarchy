using Unity.VisualScripting;
using UnityEngine;

public class HierarchyItems : MonoBehaviour
{
	//TODO Hide all items in inspector
	public enum IconType { DEFAULT, NONE, COMPONENT, TREE, CUSTOM }

	[HideInInspector] public Color _BGColor;
	[HideInInspector] public Color _InactiveColor;
	[HideInInspector] public Color _SelectedColor;
	[HideInInspector] public bool _useFullWidth = false;
	public Texture2D _Gradient = null;
	[HideInInspector] public Gradient _ColorGradient = new();
	[HideInInspector] public Gradient _PrevGradient = new();

	[HideInInspector] public Color _TextColor;
	[HideInInspector] public FontStyle _FontStyle;
	[HideInInspector] public Font _font;
	[HideInInspector] public int _fontSize = 12;

	[HideInInspector] public IconType _IconType;
	[HideInInspector] public Texture _Icon;

	//this is to toggle UI Buttons
	[HideInInspector] public bool _useDefaultBG;
	[HideInInspector] public bool _useDefaultInactiveColor;
	[HideInInspector] public bool _useDefaultSelectedColor;
	[HideInInspector] public bool _useGradient;
	[HideInInspector] public bool _useDefaultText;
	[HideInInspector] public bool _useDefaultIcon;

	//public GUIStyle style;
	

	HierarchyItems()
    {
		_BGColor = CustomHierarchyUtils.ConvertFromBRGB(new(56, 56, 56), 1);
		_InactiveColor = CustomHierarchyUtils.ConvertFromBRGB(new(49, 49, 49), 1);
		_SelectedColor = CustomHierarchyUtils.ConvertFromBRGB(new(44, 93, 135), 1);
		_TextColor = Color.white;
		_IconType = IconType.DEFAULT;
	}

	public bool isAnyBGUsed()
	{
		return (_useDefaultBG || _useDefaultInactiveColor || _useDefaultSelectedColor);
	}

}
