﻿/*
		This file is part of Distant Object Enhancement /L
			© 2021 LisiasT
			© 2019-2021 TheDarkBadger
			© 2014-2019 MOARdV
			© 2014 Rubber Ducky
*/
using KSPe.Annotations;
using UnityEngine;

namespace DistantObject
{
	[KSPAddon(KSPAddon.Startup.Instantly, true)]
	public class Startup:MonoBehaviour
	{
		[UsedImplicitly]
		private void Awake()
		{
			try
			{
				KSPe.Util.Installation.Check<Startup>();
			}
			catch (KSPe.Util.InstallmentException e)
			{
				Log.error(e.ToShortMessage());
				KSPe.Common.Dialogs.ShowStopperAlertBox.Show(e);
			}

			{ 
				using (KSPe.Util.SystemTools.Assembly.Loader a = new KSPe.Util.SystemTools.Assembly.Loader<Startup>())
				{
					a.LoadAndStartup("MeshEngine");
				}
			}
		}

		[UsedImplicitly]
		private void Start()
		{
			Log.force("Version {0}", Version.Text);
		}
	}
}
