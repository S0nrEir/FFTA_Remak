using AquilaFramework.Common;
using AquilaFramework.Common.Define;
using AquilaFramework.Common.Tools;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    [SerializeField] private bool _isOpenLog = true;

    // Start is called before the first frame update
    void Start()
    {
        Log.SetLogOpen( _isOpenLog );

        //这里是为了避免懒加载获取其中的某些对象时出现“场景内GameObject过多”导致的查找消耗问题
        GlobalInstance.Init();

        //frame init
        FrameController.I.EnsureInit();

        DontDestroyOnLoad( gameObject );
    }

}
