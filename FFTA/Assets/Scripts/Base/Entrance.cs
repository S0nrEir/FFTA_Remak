using Game.Common.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //这里是为了避免懒加载获取其中的某些对象时出现“场景内GameObject过多”导致的查找消耗问题
        GlobalInstance.Init();
    }
}
