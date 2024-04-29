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
	public Dictionary<string, List<HierarchyItems>> Presets = new Dictionary<string, List<HierarchyItems>>();

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

	public bool Contains(HierarchyItems hi)
	{
		string sceneName = SceneManager.GetActiveScene().name;

		return ItemLists[sceneName].Contains(hi);
	}

	public void PrintAllListKV()
	{
		foreach (var item in ItemLists)
		{
			Debug.Log(item.Key);
		}
	}
}
