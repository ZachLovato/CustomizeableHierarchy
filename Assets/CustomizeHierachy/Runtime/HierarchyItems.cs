using Unity.VisualScripting;
using UnityEngine;

public class HierarchyItems : MonoBehaviour
{
	//TODO Hide all items in inspector

	public enum IconType { DEFAULT, NONE, COMPONENT, TREE, CUSTOM }

	public Color _BGColor;
	public Color _InactiveColor;
	public Color _SelectedColor;
	public bool _useFullWidth = false;

	public Color _TextColor;
	public FontStyle _FontStyle;

	public IconType _IconType;
	public Texture _Icon;

	//this is to toggle UI Buttons
	public bool _useDefaultBG;
	public bool _useDefaultInactiveColor;
	public bool _useDefaultSelectedColor;
	public bool _useDefaultText;
	public bool _useDefaultIcon;

	HierarchyItems()
    {
		_BGColor = CustomHierarchyUtils.ConvertFromBRGB(new(56, 56, 56), 1);
		_InactiveColor = CustomHierarchyUtils.ConvertFromBRGB(new(49, 49, 49), 1);
		_SelectedColor = CustomHierarchyUtils.ConvertFromBRGB(new(44, 93, 135), 1);
		_TextColor = Color.black;
		_IconType = IconType.NONE;
	}	

	public bool isAnyBGUsed()
	{
		return (_useDefaultBG || _useDefaultInactiveColor || _useDefaultSelectedColor);
	}

}
