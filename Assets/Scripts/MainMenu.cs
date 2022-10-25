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
        private TMP_InputField m_accountInput; // �s�W��J��
        private Button m_loginButton; // �s�W�n�J���s

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
            m_accountInput = transform.FindAnyChild<TMP_InputField>("AccountInput"); // �����J�ؤ���
            m_loginButton = transform.FindAnyChild<Button>("LoginButton"); // ����n�J���s����

            m_lobbyUI = transform.FindAnyChild<Transform>("LobbyUI").gameObject;
            m_lobbyInput = transform.FindAnyChild<TMP_InputField>("LobbyInput");
            m_joinLobbyButton = transform.FindAnyChild<Button>("JoinLobbyButton");
            m_leaveLobbyButton = transform.FindAnyChild<Button>("LeaveLobbyButton");
            m_mapSelector = transform.FindAnyChild<TMP_Dropdown>("MapSelector");
            m_gameModeSelector = transform.FindAnyChild<TMP_Dropdown>("GameModeSelector");
            m_createGameButton = transform.FindAnyChild<Button>("CreateGameButton");
            m_joinGameButton = transform.FindAnyChild<Button>("JoinGameButton");

            m_roomUI = transform.FindAnyChild<Transform>("RoomUI").gameObject;

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

        public override void OnConnectedToMaster() // �B�z�s�u��UI�ܤ�
        {
            m_loginUI.SetActive(false);
            m_lobbyUI.SetActive(true);
        }
        private void ResetUI() // ���mUI
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
            // ���} Lobby ���ɭԡA�|�[�^ Default Lobby
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
