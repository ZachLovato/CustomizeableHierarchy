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

	public static Texture2D Create(Gradient grad, int width = 32, int height = 1)
	{
		var gradTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
		gradTex.alphaIsTransparency = true;
		gradTex.filterMode = FilterMode.Bilinear;
		gradTex.EncodeToPNG();
		
		float inv = 1f / (width - 1);
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				float t = x * inv;
				Color col = grad.Evaluate(t);
				col.a = 255;
				gradTex.SetPixel(x, y, col);
			}
		}
		gradTex.Apply();
		return gradTex;
	}

	public static HierarchyItems GetPresetOne()
	{
		GameObject temp = new GameObject();
		HierarchyItems hi = temp.AddComponent<HierarchyItems>();

		hi._useDefaultBG = true;
		hi._BGColor = Color.HSVToRGB(359, 67, 87);
		hi._useDefaultText = true;
		hi._TextColor = Color.HSVToRGB(0, 0, 100);

		Object.DestroyImmediate(temp);

		return hi;
	}

	public static HierarchyItems GetPresetTwo()
	{
		GameObject temp = new GameObject();
		HierarchyItems hi = temp.AddComponent<HierarchyItems>();

		hi._useDefaultBG = true;
		hi._useFullWidth = true;
		hi._BGColor = new Color(0.108f, 0.278f, 0.534f);
		hi._useDefaultText = true;
		hi._TextColor = new Color(0, 0, 0);
		hi._textAnchor = TextAnchor.MiddleCenter;

		Object.DestroyImmediate(temp);

		return hi;
	}

	public static HierarchyItems GetPresetThree()
	{
		GameObject temp = new GameObject();
		HierarchyItems hi = temp.AddComponent<HierarchyItems>();

		hi._useDefaultText = true;
		hi._TextColor = new Color(0, 0, 0);
		hi._IconType = HierarchyItems.IconType.COMPONENT;

		Object.DestroyImmediate(temp);

		return hi;
	}
}
