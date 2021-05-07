using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquilaFramework.ObjectPool
{
    public interface IObject
    {
        void Release ();

        void Resycle ();

        void Gen ();
    }
}
