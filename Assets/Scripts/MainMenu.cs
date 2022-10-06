using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Span
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        static MainMenu instance;
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
