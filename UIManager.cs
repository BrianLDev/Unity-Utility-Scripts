using System.Collections;
using System.Collections.Generic;
using Unity.UIElements.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace UnityUtilities {
    public class UIManager : SingletonMono<UIManager>
    {
        // Public Properties

        // UI Panel Renderers
        // Note: normally you can just use one Panel Renderer and just hide or swap in/out
        // (using ve.style.display) elements at runtime. It's a lot more efficient
        // to use a single PanelRenderer.
        public PanelRenderer m_MainMenuScreen;
        public PanelRenderer m_GameScreen;
        public PanelRenderer m_EndScreen;


        // Pre-loaded UI assets (ie. UXML/USS).
        // public VisualTreeAsset m_PlayerListItem;
        // public StyleSheet m_PlayerListItemStyles;

        // The Panel Renderer can optionally track assets to enable live
        // updates to any changes made in the UI Builder for specific UI
        // assets (ie. UXML/USS).
        private List<Object> m_TrackedAssetsForLiveUpdates;

        // We need to update the values of some UI elements so here are
        // their remembered references after being queried from the cloned
        // UXML.
        // private Label m_SpeedLabel;
        // private Label m_KillsLabel;
        // private Label m_ShotsLabel;
        // private Label m_AccuracyLabel;


        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // MonoBehaviour States

        protected new void Awake() {
            base.Awake();
            // make sure all Panel Renderers are set to off first.  Otherwise the Bind methods below will not run
            m_MainMenuScreen.enabled = false;
            m_GameScreen.enabled = false;
            m_EndScreen.enabled = false;
        }

        // OnEnable
        // Register our postUxmlReload callbacks to be notified if and when
        // the UXML or USS assets being user are changed (by the UI Builder).
        // In these callbacks, we just rebind UI VisualElements to data or
        // to click events.
        private void OnEnable()
        {
            m_MainMenuScreen.postUxmlReload = BindMainMenuScreen;
            m_GameScreen.postUxmlReload = BindGameScreen;
            m_EndScreen.postUxmlReload = BindEndScreen;

            m_TrackedAssetsForLiveUpdates = new List<Object>();
        }

        // Start
        // Just go to main menu.
        private void Start() {
    #if !UNITY_EDITOR
            if (Screen.fullScreen)
                Screen.fullScreen = false;
    #endif

            GoToMainMenu();
        }

        // Update
        private void Update() {
            // add any logic to update in game stats here
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // Bind UI to logic

        // Try to find specific elements by name in the main menu screen and
        // bind them to callbacks and data. Not finding an element is a valid
        // state.
        private IEnumerable<Object> BindMainMenuScreen() {
            var root = m_MainMenuScreen.visualTree;

            var startButton = root.Q<Button>("start-button");
            if (startButton != null) {
                startButton.clickable.clicked += () => { 
                    StartGame(); 
                };
            }

            var exitButton = root.Q<Button>("exit-button");
            if (exitButton != null) {
                exitButton.clickable.clicked += () => { 
                    Debug.Log("Quitting Game");
                    Application.Quit(); 
                };
            }

            return null;
        }

        // Try to find specific elements by name in the game screen and
        // bind them to callbacks and data. Not finding an element is a valid
        // state.
        private IEnumerable<Object> BindGameScreen() {
            var root = m_GameScreen.visualTree;

            // Stats
            // m_SpeedLabel = root.Q<Label>("_speed");
            // m_KillsLabel = root.Q<Label>("_kills");
            // m_ShotsLabel = root.Q<Label>("_shots");
            // m_AccuracyLabel = root.Q<Label>("_accuracy");

            // Buttons
            var optionsButton = root.Q<Button>("options-button");
            if (optionsButton != null) {
                optionsButton.clickable.clicked += () => {
                    GoToOptionsMenu();
                };
            }
            var backToMenuButton = root.Q<Button>("back-to-menu-button");
            if (backToMenuButton != null) {
                backToMenuButton.clickable.clicked += () => {
                    GoToMainMenu();
                };
            }
            var gameOverButton = root.Q<Button>("game-over-button");
            if (gameOverButton != null) {
                gameOverButton.clickable.clicked += () => {
                    GameOver();
                };
            }
            // var increaseSpeedButton = root.Q<Button>("increase-speed");
            // if (increaseSpeedButton != null) {
            //     increaseSpeedButton.clickable.clicked += () => {
            //         // m_Player1Movement.m_Speed += 1;
            //     };
            // }

            // var randomExplosionButton = root.Q<Button>("random-explosion");
            // if (randomExplosionButton != null) {
            //     Debug.Log("EXPLODE!");
            // }

            // m_TrackedAssetsForLiveUpdates.Clear();
            // return m_TrackedAssetsForLiveUpdates;

            return null;
        }

        // Try to find specific elements by name in the end screen and
        // bind them to callbacks and data. Not finding an element is a valid
        // state.
        private IEnumerable<Object> BindEndScreen() {
            var root = m_EndScreen.visualTree;

            root.Q<Button>("back-to-menu-button").clickable.clicked += () => { GoToMainMenu(); };

            return null;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // Screen Transition Logic
        void SetScreenEnableState(PanelRenderer screen, bool state)
        {
            if (state) {
                screen.visualTree.style.display = DisplayStyle.Flex;
                screen.enabled = true;
                screen.gameObject.GetComponent<UIElementsEventSystem>().enabled = true;
            }
            else {
                screen.visualTree.style.display = DisplayStyle.None;
                screen.enabled = false;
                screen.gameObject.GetComponent<UIElementsEventSystem>().enabled = false;
            }
        }

        IEnumerator TransitionScreens(PanelRenderer from, PanelRenderer to) {
            Debug.Log("Transitioning " + from.name + " to " + to.name);

            from.visualTree.style.display = DisplayStyle.None;
            from.gameObject.GetComponent<UIElementsEventSystem>().enabled = false;

            to.enabled = true;

            yield return null;
            yield return null;
            yield return null;

            to.visualTree.style.display = DisplayStyle.Flex;
            to.visualTree.style.visibility = Visibility.Hidden;
            to.gameObject.GetComponent<UIElementsEventSystem>().enabled = true;

            yield return null;
            yield return null;
            yield return null;

            to.visualTree.style.visibility = Visibility.Visible;

            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;

            from.enabled = false;
        }


        private void GoToMainMenu() {
            Debug.Log("Going to main menu");
            SetScreenEnableState(m_MainMenuScreen, true);
            SetScreenEnableState(m_GameScreen, false);
            SetScreenEnableState(m_EndScreen, false);
        }

        private void StartGame() {
            StartCoroutine(TransitionScreens(m_MainMenuScreen, m_GameScreen));
        }

        private void GoToOptionsMenu() {
            Debug.Log("This would load an options menu...if one existed.");
            //TODO: Add this
        }

        private void GameOver() {
            SetScreenEnableState(m_MainMenuScreen, false);
            SetScreenEnableState(m_GameScreen, false);
            SetScreenEnableState(m_EndScreen, true);
        }
    }
}