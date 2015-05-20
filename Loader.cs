using System;
using System.Collections.Generic;
using System.Threading;

using ICities;
using ColossalFramework;
using ColossalFramework.Math;
using ColossalFramework.UI;
using UnityEngine;

namespace RemoveSeagulls
{
    public class Loader : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame)
                Helper.Instance.GameLoaded = true;
        }

        public override void OnLevelUnloading ()
		{
			Helper.Instance.GameLoaded = false;
        }
    }
}