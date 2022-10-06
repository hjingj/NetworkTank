using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Span
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        static MainMenu instance;
        private GameObject m_ui;
        private Button m_joinGameButton;

        void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);

            m_ui = transform.FindAnyChild<Transform>("UI").gameObject;
            m_joinGameButton = transform.FindAnyChild<Button>("JoinGameButton");

            m_ui.SetActive(true);
            m_joinGameButton.interactable = false;
        }
    }
}
