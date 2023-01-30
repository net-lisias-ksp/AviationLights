/*
	This file is part of Aviation Lights /L
		© 2021-2023 LisiasT : http://lisias.net <support@lisias.net>

	Aviation Lights /L is licensed as follows:
		* CC-BY-NC-SA 4.0i : https://creativecommons.org/licenses/by-nc-sa/4.0/

	Aviation Lights /L is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

*/
using System;
using UnityEngine;
using KSPe;

namespace AviationLights
{
	[KSPAddon(KSPAddon.Startup.EveryScene, true)]
	public class Globals : MonoBehaviour
	{
		protected static float rangeMaxValueInMeters = 150;
		public static float RangeMaxValueInMeters => rangeMaxValueInMeters;

		private void Start()
		{
			try
			{ 
				UrlDir.UrlConfig urlc = GameDatabase.Instance.GetConfigs("AVIATION_LIGHTS")[0];
				ConfigNodeWithSteroids cn = ConfigNodeWithSteroids.from(urlc.config);
				rangeMaxValueInMeters = cn.GetValue<float>("RangeMaxValueInMeters", rangeMaxValueInMeters);
			}
			catch (Exception e)
			{
				Log.error(e, "Error reading configurations. Using defaults.");
			}
		}
	}
}
