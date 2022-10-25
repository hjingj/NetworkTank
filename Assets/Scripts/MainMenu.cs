using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;

namespace Span
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        static MainMenu instance;
        private GameObject m_ui;

        private GameObject m_loginUI;
        private TMP_InputField m_accountInput; // 新增輸入框
        private Button m_loginButton; // 新增登入按鈕

        private GameObject m_lobbyUI;
        private TMP_InputField m_lobbyInput;
        private Button m_joinLobbyButton;
        private Button m_leaveLobbyButton;
        private TMP_Dropdown m_mapSelector;
        private TMP_Dropdown m_gameModeSelector;
        private Button m_createGameButton;
        private Button m_joinGameButton;

        private GameObject m_roomUI;

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

            m_loginUI = transform.FindAnyChild<Transform>("LoginUI").gameObject;
            m_accountInput = transform.FindAnyChild<TMP_InputField>("AccountInput"); // 抓取輸入框元件
            m_loginButton = transform.FindAnyChild<Button>("LoginButton"); // 抓取登入按鈕元件

            m_lobbyUI = transform.FindAnyChild<Transform>("LobbyUI").gameObject;
            m_lobbyInput = transform.FindAnyChild<TMP_InputField>("LobbyInput");
            m_joinLobbyButton = transform.FindAnyChild<Button>("JoinLobbyButton");
            m_leaveLobbyButton = transform.FindAnyChild<Button>("LeaveLobbyButton");
            m_mapSelector = transform.FindAnyChild<TMP_Dropdown>("MapSelector");
            m_gameModeSelector = transform.FindAnyChild<TMP_Dropdown>("GameModeSelector");
            m_createGameButton = transform.FindAnyChild<Button>("CreateGameButton");
            m_joinGameButton = transform.FindAnyChild<Button>("JoinGameButton");

            m_roomUI = transform.FindAnyChild<Transform>("RoomUI").gameObject;

            ResetUI(); // 抽出UI初始化
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
            if (!PhotonNetwork.InRoom)
            {
                ResetUI();
            }
            else 
            {
                m_lobbyUI.SetActive(false);
                m_roomUI.SetActive(true);
            }
        }

        public override void OnConnectedToMaster() // 處理連線後UI變化
        {
            m_loginUI.SetActive(false);
            m_lobbyUI.SetActive(true);
        }
        private void ResetUI() // 重置UI
        {
            m_ui.SetActive(true);

            m_loginUI.SetActive(true);
            m_accountInput.interactable = true;
            m_loginButton.interactable = true;

            m_lobbyUI.SetActive(false);
            m_lobbyInput.interactable = true;
            m_joinLobbyButton.interactable = true;
            m_leaveLobbyButton.interactable = false;
            m_mapSelector.interactable = true;
            m_gameModeSelector.interactable = true;
            m_createGameButton.interactable = true;
            m_joinGameButton.interactable = true;

            m_roomUI.SetActive(false);
        }

        public void Login() // 處理登入伺服器流程
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

        public void JoinLobby()
        {
            var typedLobby = new TypedLobby(m_lobbyInput.text, LobbyType.Default);
            PhotonNetwork.JoinLobby(typedLobby);
        }

        public void LeaveLobby()
        {
            PhotonNetwork.LeaveLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log($"Joined Lobby: {PhotonNetwork.CurrentLobby.Name}{PhotonNetwork.CurrentLobby.Type}");
            m_joinLobbyButton.interactable = false;
            m_leaveLobbyButton.interactable = true;
        }

        public override void OnLeftLobby()
        {
            // 離開 Lobby 的時候，會加回 Default Lobby
            Debug.Log($"Left Lobby: {PhotonNetwork.CurrentLobby.Name}{PhotonNetwork.CurrentLobby.Type}");
            m_joinLobbyButton.interactable = true;
            m_leaveLobbyButton.interactable = false;
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
