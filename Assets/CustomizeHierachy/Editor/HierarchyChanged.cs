using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HierarchyChanged : ScriptableObject
{
    //public List<int> ID = new List<int>();
    //public List<GameObject> Items = new List<GameObject>();
    //public List<ChangeInformation> Details = new List<ChangeInformation>();
    public List<Texture2D> Texture = new List<Texture2D>();

	/*
	
	#region Retrieval
	public bool Contains(int id)
	{
		return ID.Contains(id);
	}

	public bool Contains(GameObject go)
	{
		return Items.Contains(go);
	}

	public GameObject GetGameObject(int id)
	{
		for (int i = 0; i < ID.Count; i++)
		{
			if (ID[i] == id)
			{
				return Items[i];
			}
		}
		return null;
	}

	/// <summary>
	/// Gets ChangeInformation using InstanceID
	/// </summary>
	/// <param name="id">InstanceID</param>
	/// <returns>ChangeInformation</returns>
	public ChangeInformation GetDetails(int id)
    {
        if (DebugLength()) return null;
        for (int i = 0; i < ID.Count; i++)
        {
            if (id == ID[i]) 
                return Details[i];
        }
        return null;
    }

	/// <summary>
	/// Gets ChangeInformation using a Gameobject
	/// </summary>
	/// <param name="id">Gameobject</param>
	/// <returns>ChangeInformation</returns>
	public ChangeInformation GetDetails(ref GameObject go)
    {
		if (DebugLength()) return null;
		for (int i = 0; i < Items.Count; i++)
		{
			if (go == Items[i])
				return Details[i];
		}
		return null;
	}
	#endregion

	#region Edit Lists
	/// <summary>
	/// Add to ID and Details
	/// </summary>
	/// <param name="id">InstanceID</param>
	/// <param name="ci">Change Information</param>
	public void Add(int id, ChangeInformation ci)
    {
        ClearFalseItems();

        // Removes Dupes
        while (ID.Contains(id))
        {
            RemoveItem(id);
        }

        ID.Add(id);
        Details.Add(ci);
        var tmep = EditorUtility.InstanceIDToObject(id) as GameObject;

		Items.Add(tmep);
    }

    /// <summary>
    /// Removes entries using an instanceID in both ID and Details
    /// </summary>
    /// <param name="id">Instance ID</param>
    /// <param name="isClearing">Set True to avoid recurrsion</param>
    public void RemoveItem(int id, bool isClearing = false)
    {
        if (!isClearing) ClearFalseItems();

		if (DebugLength()) return;

		for (int i = 0; i < ID.Count; i++)
		{
            if (id == ID[i])
            {
                ID.RemoveAt(i);
                Details.RemoveAt(i);
                Items.RemoveAt(i);
                return;
            }
		}
	}
	*/


	public void AddTexture(Texture2D tex)
	{
		if (!Texture.Contains(tex)) Texture.Add(tex);
	}


	/*
	/// <summary>
	/// Removes any items that are not currently in the Hierarchy
	/// </summary>
	private void ClearFalseItems()
	{
		int pos = 0;
		while (pos < ID.Count)
		{
			//finds object by using ID and converting them to a Gameobject
			var temo = EditorUtility.InstanceIDToObject(ID[pos]) as GameObject;

			if (temo == null)
			{
				RemoveItem(ID[pos], true);
			}
			else pos++;
		}
	}
	#endregion

	#region Debug
	/// <summary>
	/// Checks to see of both ID and Details are the same length.
	/// </summary>
	/// <returns>True if not equal</returns>
	private bool DebugLength()
    {
		if (ID.Count != Details.Count)
		{
			Debug.Log("ID and Details are not equal in Length");
			Debug.Log("ID length: " + ID.Count);
			Debug.Log("Detail length: " + Details.Count);
            return true;
        }
        return false;
	}
	#endregion
	*/


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

	public void Get(HierarchyItems hi)
	{
	}
}
