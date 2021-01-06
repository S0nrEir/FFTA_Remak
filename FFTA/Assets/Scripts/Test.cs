using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private List<TempObj> TestDic;
    [SerializeField] private TempObj[] test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

}

[System.Serializable]
public class TempObj
{
    public string path;
    public Component comp;
}
