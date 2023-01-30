/*
	This file is part of Aviation Lights /L
		© 2021-2023 LisiasT : http://lisias.net <support@lisias.net>

	Aviation Lights /L is licensed as follows:
		* CC-BY-NC-SA 4.0i : https://creativecommons.org/licenses/by-nc-sa/4.0/

	Aviation Lights /L is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

*/
using System.Collections.Generic;
using IO = KSPe.IO;
using Directory = KSPe.IO.Directory;
using Path = KSPe.IO.Path;

namespace HLAirshipsCore
{
	public class ModuleManagerSupport : UnityEngine.MonoBehaviour
	{
		public static IEnumerable<string> ModuleManagerAddToModList()
		{
			List<string> list = new List<string>();
			if (KSPe.Util.KSP.Version.Current >= KSPe.Util.KSP.Version.FindByVersion(1,4,0))
				list.Add("AviationLights-KSP-1.4");	
			if (KSPe.Util.KSP.Version.Current >= KSPe.Util.KSP.Version.FindByVersion(1,11,0))
				list.Add("AviationLights-KSP-1.11");	
			string[] r = list.ToArray();
			return r;
		}
	}
}
