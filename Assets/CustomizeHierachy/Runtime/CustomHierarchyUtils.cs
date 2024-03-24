using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomHierarchyUtils
{
	public static HierarchyItems GetHierarchyFromObject(ref GameObject go)
	{
		go.TryGetComponent<HierarchyItems>(out var hi);
		return hi;
	}

	public static HierarchyItems AddHierarchyItemsToObject(ref GameObject go)
	{
		if (go.TryGetComponent<HierarchyItems>(out var hi))
		{
			return hi;
		}
		else return go.AddComponent<HierarchyItems>();
	}

	public static void RemoveHierarchyItemsFromObject(ref GameObject go)
	{
		if (go.TryGetComponent<HierarchyItems>(out var hi))
		{
			MonoBehaviour.DestroyImmediate(hi);
		}
	}

	public static Color ConvertFromBRGB(Vector3 rgb, float alpha)
	{
		Vector3 brgb = new Vector3(rgb.x / 255, rgb.y / 255, rgb.z / 255);
		return new Color(brgb.x, brgb.y, brgb.z, alpha);
	}

	public static Color ConvertFromRGBA(Vector4 rgba)
	{
		return rgba / 255;
	}
}
