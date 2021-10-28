/*
	This file is part of Aviation Lights /L Unleashed
		© 2021 Lisias T : http://lisias.net <support@lisias.net>
		© 2018-2020 MOARdv
		© 2012-2018 BigNose
		© 2012 RPGprayer

	Aviation Lights /L Unleashed is licensed as follows:

		* CC-BY-NC-SA 4.0i : https://creativecommons.org/licenses/by-nc-sa/4.0/

	Aviation Lights /L Unleashed is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

*/
using UnityEngine;

namespace AviationLights
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    internal class Startup : MonoBehaviour
    {
        private void Start()
        {
            Log.force("Version {0}", Version.Text);

            try
            {
                KSPe.Util.Installation.Check<Startup>();
            }
            catch (KSPe.Util.InstallmentException e)
            {
                Log.error(e, this);
                KSPe.Common.Dialogs.ShowStopperAlertBox.Show(e);
            }
        }
    }
}
