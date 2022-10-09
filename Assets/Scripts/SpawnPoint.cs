using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace span
{
    public class SpawnPoint : MonoBehaviour
    {
        void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
