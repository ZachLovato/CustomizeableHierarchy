using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HierarchyChanged : ScriptableObject
{
    public List<Texture2D> Texture = new List<Texture2D>();

	// a set of classes that will be a preset for 
	public List<HierarchyItems> Presets = new List<HierarchyItems>();
	public Dictionary<string, HierarchyItems> Preset = new Dictionary<string, HierarchyItems>();

	public void AddTexture(Texture2D tex)
	{
		if (!Texture.Contains(tex)) Texture.Add(tex);
	}

	[Space, Space] // this should hold all items in multiple scenes
	public Dictionary<string, List<HierarchyItems>> ItemLists = new Dictionary<string, List<HierarchyItems>>();

	public void Add(string sceneName, HierarchyItems[] items)
	{
		if (ItemLists.ContainsKey(sceneName))
		{
			ItemLists[sceneName] = items.ToList();
		}
		else
		{
			ItemLists.Add(sceneName, items.ToList());
		}
	}

	public void Add(HierarchyItems item)
	{
		string sceneName = SceneManager.GetActiveScene().name;

		if (ItemLists.ContainsKey(sceneName))
		{
			ItemLists[sceneName].Add(item);
		}
		else
		{
			ItemLists.Add(sceneName, new List<HierarchyItems> { item });
		}
	}

	public void AddPreset(HierarchyItems preset)
	{
		Presets.Add(preset);
	}

	public void AddPreset(string presetName, HierarchyItems preset)
	{
		if (Preset.ContainsKey(presetName)) 
		{
			Debug.LogWarning("Preset Name \"" + presetName + "\" is already a preset name\nPreset Was Not Added");
		}
		else
			Preset.Add(presetName, preset);
	}

	public void RemovePreset(string presetName)
	{
		Preset.Remove(presetName);
	}

	public void Clear()
	{
		Presets.Clear();
	}

	public bool Contains(HierarchyItems hi)
	{
		string sceneName = SceneManager.GetActiveScene().name;

		return ItemLists[sceneName].Contains(hi);
	}

	public string[] GetPresetArray()
	{
		string[] output = new string[Preset.Count + 1];
		output[0] = "None";

		int index = 1;

		foreach (KeyValuePair<string, HierarchyItems> kvp in Preset)
		{
			output[index] = kvp.Key;
			index++;
		}

		return output;
	}

	public void PrintPresetsCount()
	{
		Debug.Log(Presets.Count);
	}
	public void PrintPresetCount()
	{
		string output = "";

		foreach (KeyValuePair<string, HierarchyItems> kvp in Preset)
		{
			output += kvp.Key + " ";
		}
		
		Debug.Log(output);
	}

	public void PrintAllListKV()
	{
		foreach (var item in ItemLists)
		{
			Debug.Log(item.Key);
		}
	}

	/// <summary>
	/// Converts the other HI into a preset
	/// This should be named Convert or something along those lines
	/// </summary>
	public void OneOfUs(string hiName, HierarchyItems other)
	{
		hiName = hiName.Substring(0, hiName.Length);
		if (!Preset.ContainsKey(hiName))
		{
			other._presetSelection = 0;
			other._presetSelection = 0;
		}

		HierarchyItems temp = Preset[hiName]; // this is a dumb way bit other = temp did not work

		other._Name = temp._Name;
		other._BGColor = temp._BGColor;
		other._InactiveColor = temp._InactiveColor;
		other._SelectedColor = temp._SelectedColor;
		other._useFullWidth = temp._useFullWidth;
		other._Gradient = temp._Gradient;
		other._ColorGradient = temp._ColorGradient;
		other._PrevGradient = temp._PrevGradient;

		other._TextColor = temp._TextColor;
		other._FontStyle = temp._FontStyle;
		other._font = temp._font;
		other._fontSize = temp._fontSize;
		other._textAnchor = temp._textAnchor;

		other._IconType = temp._IconType;
		other._Icon = temp._Icon;

		other._useDefaultBG = temp._useDefaultBG;
		other._useDefaultInactiveColor = temp._useDefaultInactiveColor;
		other._useDefaultSelectedColor = temp._useDefaultSelectedColor;
		other._useGradient = temp._useGradient;
		other._useDefaultText = temp._useDefaultText;
		other._useDefaultIcon = temp._useDefaultIcon;

		other._presetSelection = 0;
		other._prevPresetSelection = 0;
	}
}
