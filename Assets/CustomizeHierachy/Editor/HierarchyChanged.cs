using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HierarchyChanged : ScriptableObject
{
    public List<Texture2D> Texture = new List<Texture2D>();

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

		ItemLists[sceneName].Add(item);
	}

	public bool Contains(HierarchyItems hi)
	{
		string sceneName = SceneManager.GetActiveScene().name;

		return ItemLists[sceneName].Contains(hi);
	}
}
