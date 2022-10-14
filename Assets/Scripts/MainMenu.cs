using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Span
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        public static MainMenu instance;
        private GameObject m_ui;
        private TMP_InputField m_accountInput; // �s�W��J��
        private Button m_loginButton; // �s�W�n�J���s

        private TMP_Dropdown m_mapSelector;
        private TMP_Dropdown m_gameModeSelector;
        private Button m_createGameButton;
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
            m_accountInput = transform.FindAnyChild<TMP_InputField>("AccountInput"); // �����J�ؤ���
            m_loginButton = transform.FindAnyChild<Button>("LoginButton"); // ����n�J���s����

            m_mapSelector = transform.FindAnyChild<TMP_Dropdown>("MapSelector");
            m_gameModeSelector = transform.FindAnyChild<TMP_Dropdown>("GameModeSelector");
            m_createGameButton = transform.FindAnyChild<Button>("CreateGameButton");
            m_joinGameButton = transform.FindAnyChild<Button>("JoinGameButton");

            ResetUI(); // ��XUI��l��
        }

        public override void OnEnable()
        {
            // Always call the base to add callbacks
            base.OnEnable();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            m_ui.SetActive(!PhotonNetwork.InRoom);
        }

        public override void OnConnectedToMaster() // �B�z�s�u��UI�ܤ�
        {
            m_accountInput.gameObject.SetActive(false);
            m_loginButton.gameObject.SetActive(false);

            m_mapSelector.gameObject.SetActive(true);
            m_gameModeSelector.gameObject.SetActive(true);
            m_createGameButton.gameObject.SetActive(true);
            m_joinGameButton.gameObject.SetActive(true);
        }
        private void ResetUI() // ���mUI
        {
            m_ui.SetActive(true);
            m_accountInput.gameObject.SetActive(true);
            m_loginButton.gameObject.SetActive(true);

            m_mapSelector.gameObject.SetActive(false);
            m_gameModeSelector.gameObject.SetActive(false);
            m_createGameButton.gameObject.SetActive(false);
            m_joinGameButton.gameObject.SetActive(false);

            m_accountInput.interactable = true;
            m_loginButton.interactable = true;

            m_mapSelector.interactable = true;
            m_gameModeSelector.interactable = true;
            m_createGameButton.interactable = true;
            m_joinGameButton.interactable = true;
        }

        public void Login() // �B�z�n�J���A���y�{
        {
            if (string.IsNullOrEmpty(m_accountInput.text))
            {
                Debug.Log("Please input your account!!");
                return;
            }

            m_accountInput.interactable = false;
            m_loginButton.interactable = false;

            if (!GameManager.instance.ConnectToServer(m_accountInput.text))
            {
                m_accountInput.interactable = true;
                m_loginButton.interactable = true;
                Debug.Log("Connect to PUN Failed!!");
            }
        }

        public void CreateGame()
        {
            GameManager.instance.CreateGame(m_mapSelector.value + 1, m_gameModeSelector.value + 1);
        }
        public void JoinRandomGame()
        {
            GameManager.instance.JoinRandomGame(m_mapSelector.value + 1, m_gameModeSelector.value + 1);
        }
    }
}
