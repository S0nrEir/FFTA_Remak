using AquilaFramework.Common.Tools;
using AquilaFramework.Common.UI;

namespace Game.UI
{
    public class TestUI : UIBase
    {
        protected override string Path => "Test";


        public override void OnLoad ()
        {
            Log.Info("TestUI.OnLoad");
            TestUIParam test = new TestUIParam()
            {
                Param = null,
                test = string.Empty,
                OnShow = () => { Log.Info( "OnShow" ); },
            };
        }

        protected override void OnShow ()
        {
            Log.Info("TestUI.OnShow");
        }
    }

    public class TestUIParam : UIParam
    {
        public string test;
    }
}
