using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Span
{
    public class HUD : MonoBehaviour
    {
        static HUD instance;
        void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}