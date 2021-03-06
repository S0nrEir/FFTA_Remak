﻿using UnityEngine;
using AquilaFramework.Common;
using AquilaFramework.Common.Define;
using AquilaFramework.Common.Tools;
using AquilaFramework.ObjectPool;

public class Entrance : MonoBehaviour
{
    [Header( "log开关" )]
    [SerializeField] private bool _isOpenLog = true;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad( gameObject );
        Log.SetLogOpen( _isOpenLog );

        //这里是为了避免懒加载获取其中的某些对象时出现“场景内GameObject过多”导致的查找消耗问题
        GlobalInstance.Init();
        FrameController.I.EnsureInit();
        ObjectPoolMgr.I.Init();

        TestPool pool = ObjectPoolMgr.I.CreateDefaultObjectPoolTest<TestPool>();
        pool.Do();
    }

}

public class TestPool : ObjectPoolBaseTest<ObjectPoolItem>
{
    public TestPool ()
    { }

    public void Do ()
    { }
}

public class ObjectPoolItem : IObject
{
    public void Gen ()
    {
        Log.Info("Gen");
    }

    public void Release ()
    {
        Log.Info( "Release" );
    }

    public void Resycle ()
    {
        Log.Info( "Resycle" );
    }
}
