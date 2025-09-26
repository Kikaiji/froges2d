using Godot;
using System;
using System.Collections.Generic;

public class ResourceFileLoader
{

	private static ResourceFileLoader _instance;

	public static ResourceFileLoader Instance
	{
		get { return _instance ??= new ResourceFileLoader(); }
		set => _instance = value;
	}
	
	public List<T> LoadFolder<T>(string filePath) where T : class
	{
		var dir = DirAccess.Open(filePath);
		var result = new List<T>();
		if (dir != null)
		{
			dir.ListDirBegin();
			string filename = dir.GetNext();

			while (filename != "")
			{
				GD.Print(filename);
				if (dir.CurrentIsDir())
				{
					filename = dir.GetNext();
					continue;
				}

				result.Add(GD.Load(filePath + filename) as T);
				filename = dir.GetNext();
			}
			
			return result;
		}

		return null;
	}
	
	public Dictionary<string, T> LoadFolderAsDict<T>(string filePath) where T : class{
		var dir = DirAccess.Open(filePath);
		var result = new Dictionary<string, T>();
		if (dir != null)
		{
			dir.ListDirBegin();
			string filename = dir.GetNext();

			while (filename != "")
			{
				GD.Print(filename);
				if (dir.CurrentIsDir())
				{
					filename = dir.GetNext();
					continue;
				}

				string id;
				id = filename.Remove(filename.Find('.'));
				GD.Print(id);
				result.Add(id, GD.Load(filePath + filename) as T);
				filename = dir.GetNext();
			}
			
			return result;
		}

		return null;
	}
}
