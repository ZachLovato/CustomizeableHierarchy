using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
}
