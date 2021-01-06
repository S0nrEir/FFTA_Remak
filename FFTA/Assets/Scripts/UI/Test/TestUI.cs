using Game.Common.Tools;
using Game.Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class TestUI : UIBase
    {
        protected override string Path => "Test";


        public override void OnLoad ()
        {
            Log.Info("TestUI.OnLoad");
        }

        protected override void OnShow ()
        {
            Log.Info("TestUI.OnShow");
        }
    }
}
