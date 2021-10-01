﻿/*
		This file is part of Distant Object Enhancement /L
			© 2021 LisiasT
			© 2019-2021 TheDarkBadger
			© 2014-2019 MOARdV
			© 2014 Rubber Ducky

	THIS FILE is ARR to LisiasT. No right other than using the generalted DLL on your machine is granted.
*/
using System;
using System.Collections.Generic;

namespace DistantObject.MeshEngine
{
	public static class Database
	{
		internal static class PartModelDB
		{ 
			private static readonly Dictionary<string, List<string>> DB = new Dictionary<string, List<string>>();

			internal static void Add(string partName, string modelName)
			{
				if (!DB.ContainsKey(partName)) DB.Add(partName, new List<string>());
				DB[partName].Add(modelName);
			}

			internal static bool ContainsKey(string partName)
			{
				return DB.ContainsKey(partName);
			}

			internal static IEnumerable<string> Get(string partName)
			{
				return DB[partName];
			}
		}

		internal static void Init()
		{
			bool sawErrors = false;
			foreach (UrlDir.UrlConfig urlConfig in GameDatabase.Instance.GetConfigs("PART"))
			{
				ConfigNode cfgNode = ConfigNode.Load(urlConfig.parent.fullPath);
				foreach (ConfigNode node in cfgNode.nodes)
				{
					if (node.GetValue("name") == urlConfig.name)
					{
						cfgNode = node;
						break;
					}
				}

				if (cfgNode.HasValue("name"))
				{
					string partName = cfgNode.GetValue("name");

					// There's no point on tryint to render the Prebuilt parts. Their meshes are not available.
					if (partName.StartsWith("kerbalEVA")) continue;
					if (partName.StartsWith("flag")) continue;

					string url = urlConfig.parent.url.Substring(0, urlConfig.parent.url.LastIndexOf("/"));
					if (cfgNode.HasValue("mesh"))
					{
						string modelName = cfgNode.GetValue("mesh");
						modelName = System.IO.Path.GetFileNameWithoutExtension(modelName);
						Log.detail("Addint {0} {1}/{2}", partName, url, modelName);
						PartModelDB.Add(partName, url + "/" + modelName);
					}
					else if (cfgNode.HasNode("MODEL"))
					{
						ConfigNode[] cna = cfgNode.GetNodes("MODEL");
						foreach (ConfigNode cn in cna)
						{ 
							string modelName = cn?.GetValue("model");
							Log.detail("Addint {0} {1}", partName, modelName);
							PartModelDB.Add(partName, modelName);
						}
					}
					else
					{
						Log.trace("Could not find a model for part {0}.  Part will not render for VesselDraw.", partName);
						sawErrors = true;
					}
				}
				else
				{
					Log.trace("Could not find ConfigNode for part {0}.  Part will not render for VesselDraw.", urlConfig.name);
					sawErrors = true;
				}
			}

			Log.dbg("VesselDraw initialized");
			if (sawErrors) Log.error("Some parts do not have ConfigNode entries in the game database.  Some distant vessels will be missing pieces.");
		}
	}
}