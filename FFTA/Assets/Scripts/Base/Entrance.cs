using UnityEngine;

public class Entrance : MonoBehaviour
{
    [Header( "log开关" )]
    [SerializeField] private bool _isOpenLog = true;

    // Start is called before the first frame update
    void Start()
    {
        AquilaFramework.Common.Tools.Log.SetLogOpen( _isOpenLog );

        //这里是为了避免懒加载获取其中的某些对象时出现“场景内GameObject过多”导致的查找消耗问题
        AquilaFramework.Common.Define.GlobalInstance.Init();

        //frame init
        AquilaFramework.Common.FrameController.I.EnsureInit();

        //objectPoolInit
        AquilaFramework.ObjectPool.ObjectPoolMgr.I.Init();

        DontDestroyOnLoad( gameObject );
    }

}
