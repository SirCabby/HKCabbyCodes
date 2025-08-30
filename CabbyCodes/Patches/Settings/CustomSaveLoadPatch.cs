using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.Utilities;
using GlobalEnums;
using CabbyCodes.SavedGames;
using CabbyMenu.UI.Controls.InputField;
using CabbyMenu.UI;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Handles custom save/load functionality with user-named saves in a CabbySaves subfolder.
    /// </summary>
    public class CustomSaveLoadPatch
    {
        /// <summary>
        /// Flag to track if patches have been applied.
        /// </summary>
        private static bool patchesApplied = false;

        /// <summary>
        /// Flag to prevent double-saving when the Save button triggers multiple save attempts.
        /// </summary>
        private static bool isSaving = false;

        /// <summary>
        /// Creates and returns the custom save name input field panel (for external use).
        /// </summary>
        /// <returns>The created InputFieldPanel for custom save names.</returns>
        public static InputFieldPanel<string> CreateCustomSaveNamePanel()
        {
            // Calculate width for 35 characters but allow 50 characters
            int widthFor35Chars = CalculatePanelWidth(35);
            
            // Create custom input field sync that triggers save on Enter
            var customInputFieldSync = new StringInputFieldSync(
                customSaveNameRef,
                KeyCodeMap.ValidChars.AlphaNumericWithSpaces,
                new Vector2(widthFor35Chars, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT),
                60,
                AttemptSaveCustomGame);
            
            var panel = new InputFieldPanel<string>(customInputFieldSync, "(Optional) Save game name");
            customSaveNameInputPanel = panel;
            return panel;
        }

        /// <summary>
        /// Creates the Save button panel wired to the internal SaveCustomGame logic.
        /// </summary>
        public static ButtonPanel CreateSaveButtonPanel()
        {
            return new ButtonPanel(AttemptSaveCustomGame, "Save", "Save game at current location");
        }

        /// <summary>
        /// Builds panels for each existing custom save file without attaching them.
        /// SettingsPatch injects them into the menu at the correct spot.
        /// </summary>
        /// <param name="onListChanged">Callback invoked when a save is deleted so the caller can rebuild the UI.</param>
        public static List<CheatPanel> BuildSavePanels(System.Action onListChanged)
        {
            var panels = new List<CheatPanel>();

            foreach (string saveFileName in SavedGameManager.GetCustomSaveFiles())
            {
                string displayName = SavedGameManager.GetDisplayNameFromFileName(saveFileName);

                var buttonPanel = new ButtonPanel(() =>
                {
                    var menu = CabbyCodesPlugin.cabbyMenu;
                    if (menu == null)
                    {
                        LoadCustomSave(saveFileName);
                        return;
                    }

                    string msg = $"Load the save '{displayName}'?\n\nYou will lose any unsaved progress.";
                    var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                        menu,
                        "Are you sure?",
                        msg,
                        "Load",
                        "Cancel",
                        () => { LoadCustomSave(saveFileName); },
                        null);
                    popup.Show();
                }, "Load", displayName);

                // Add destroy button with confirmation popup
                PanelAdder.AddDestroyButtonToPanel(buttonPanel, () =>
                {
                    // actual deletion logic after panel removal
                    if (SavedGameManager.DeleteCustomSave(saveFileName))
                    {
                        SaveGameAnalysisPatch.RefreshFileList();
                        onListChanged?.Invoke();
                    }
                }, "X", 60f, (confirm, cancel) =>
                {
                    var menu = CabbyCodesPlugin.cabbyMenu;
                    if (menu == null) { confirm(); return; }
                    string msg = $"Delete the save '{displayName}'?\n\n This action cannot be undone.";
                    var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                        menu,
                        "Are you sure?",
                        msg,
                        "Delete",
                        "Keep",
                        confirm,
                        cancel);
                    popup.Show();
                });

                // Add inline SAVE button between Load button and description label
                PanelAdder.AddButton(buttonPanel, 1, () =>
                {
                    // Save (overwrite) logic for this slot
                    string customName = displayName; // use display name as save name
                    string fileName = SavedGameManager.GenerateSaveFileName(customName);
                    string filePath = System.IO.Path.Combine(SavedGameManager.GetCabbySavesDirectory(), fileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        var menu = CabbyCodesPlugin.cabbyMenu;
                        if (menu != null)
                        {
                            string msg = $"Overwrite the save '{displayName}'?\n\nThis action cannot be undone.";
                            var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                                menu,
                                "Overwrite Save File?",
                                msg,
                                "Overwrite",
                                "Cancel",
                                () => { SavedGameManager.SaveCustomGame(customName); },
                                null);
                            popup.Show();
                        }
                        else
                        {
                            SavedGameManager.SaveCustomGame(customName);
                        }
                    }
                    else
                    {
                        // No file exists - just save
                        SavedGameManager.SaveCustomGame(customName);
                    }
                }, "Save", new Vector2(CabbyMenu.Constants.MIN_PANEL_WIDTH, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT));

                panels.Add(buttonPanel);
            }

            return panels;
        }

        /// <summary>
        /// Calculates the panel width based on character limit, matching the logic from InputFieldPanel.
        /// </summary>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        /// <returns>The calculated width in pixels.</returns>
        private static int CalculatePanelWidth(int characterLimit)
        {
            // Use the cursor character width for panel sizing to match visible character logic
            float estimatedCharWidth = CalculateCursorCharacterWidth(CabbyMenu.Constants.DEFAULT_FONT_SIZE);
            float uiBuffer = 10f;
            float calculatedWidth = (characterLimit * estimatedCharWidth) + uiBuffer;
            return Mathf.Max(CabbyMenu.Constants.MIN_PANEL_WIDTH, Mathf.RoundToInt(calculatedWidth));
        }

        /// <summary>
        /// Calculates the character width for cursor positioning calculations.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width for cursor positioning.</returns>
        private static float CalculateCursorCharacterWidth(int fontSize)
        {
            return fontSize * 0.65f;
        }

        /// <summary>
        /// Saves the current game state as a custom save.
        /// </summary>
        private static void SaveCustomGame(string customNameParam)
        {
            // flag already set by caller
            // customNameParam already captured, but guard nulls
            string customName = (customNameParam ?? string.Empty).Trim();
            
            SavedGameManager.SaveCustomGame(customName, (success) =>
            {
                if (success)
                {
                    CabbyCodesPlugin.BLogger.LogDebug(string.Format("Game saved successfully: {0}", customName));
                    
                    // Clear the custom name input field after saving
                    customSaveNameRef.Set("");
                    
                    // Force an update to ensure the UI reflects the cleared value
                    customSaveNameInputPanel?.ForceUpdate();
                    
                    SaveGameAnalysisPatch.RefreshFileList();
                    SettingsPatch.RefreshCustomSavePanels();
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to save game: {0}", customName));
                }

                // Reset saving flag once callback completes
                isSaving = false;
            });
        }

        /// <summary>
        /// Loads a custom save file.
        /// </summary>
        /// <param name="saveFileName">The name of the save file to load.</param>
        private static void LoadCustomSave(string saveFileName)
        {
            // Cache the current slot before leaving (in case it changed)
            if (GameManager.instance != null)
            {
                LastLoadedSlot = GameManager.instance.profileID;
            }

            // Show loading popup immediately when user clicks load
            var loadingPopup = CreateLoadingPopup();
            if (loadingPopup != null)
            {
                loadingPopup.Show();
                
                // Store the popup reference so it can be accessed later
                currentLoadingPopup = loadingPopup;
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogWarning("LoadCustomSave: Failed to create loading popup");
            }

            // Instead of loading immediately, set up QuickStartPatch to load after menu
            QuickStartPatch.CustomFileToLoad = saveFileName;

            // Start coroutine to return to main menu
            GameManager.instance?.StartCoroutine(GameManager.instance.ReturnToMainMenu(
                    GameManager.ReturnToMainMenuSaveModes.DontSave, null));
        }

        /// <summary>
        /// Creates a loading popup with "Loading Game" header and "Please Wait . . ." message.
        /// </summary>
        /// <returns>The created loading popup, or null if no menu is available.</returns>
        private static IPersistentPopup CreateLoadingPopup()
        {
            // Create a scene-independent popup that can persist across scene changes
            var popup = CreatePersistentLoadingPopup();
            return popup;
        }

        /// <summary>
        /// Creates a persistent loading popup that can survive scene changes.
        /// </summary>
        /// <returns>The created persistent popup.</returns>
        private static IPersistentPopup CreatePersistentLoadingPopup()
        {
            // Create a persistent GameObject that won't be destroyed during scene changes
            var persistentGo = new GameObject("PersistentLoadingPopup");
            Object.DontDestroyOnLoad(persistentGo);
            
            // Add Canvas components for UI rendering
            Canvas canvas = persistentGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; // This is crucial for visibility
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000; // Ensure popup appears on top of all UI
            persistentGo.AddComponent<GraphicRaycaster>();
            
            CanvasScaler scaler = persistentGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080); // Standard reference resolution
            
            // Create the popup content
            CreatePopupContent(persistentGo);
            
            // Return a simple popup wrapper that manages the persistent GameObject
            return new PersistentPopupWrapper(persistentGo);
        }

        /// <summary>
        /// Creates the visual content for the loading popup.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach content to.</param>
        private static void CreatePopupContent(GameObject parent)
        {
            // Create dimmed background
            GameObject background = DefaultControls.CreatePanel(new DefaultControls.Resources());
            background.name = "Popup Background";
            var backgroundImage = background.GetComponent<Image>();
            backgroundImage.color = new Color(0f, 0f, 0f, 0.6f);
            background.transform.SetParent(parent.transform, false);
            
            // Set background to fill the screen
            var backgroundRect = background.GetComponent<RectTransform>();
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.sizeDelta = Vector2.zero;

            // Create main popup panel
            GameObject popupPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            popupPanel.name = "Popup Panel";
            var panelImage = popupPanel.GetComponent<Image>();
            panelImage.color = new Color(0f, 0f, 0f, 1f);
            popupPanel.transform.SetParent(parent.transform, false);
            
            // Center the panel
            var panelRect = popupPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(400, 200);
            panelRect.anchoredPosition = Vector2.zero;

            // Add layout components
            VerticalLayoutGroup verticalLayout = popupPanel.AddComponent<VerticalLayoutGroup>();
            verticalLayout.padding = new RectOffset(20, 20, 20, 20);
            verticalLayout.spacing = 10;
            verticalLayout.childAlignment = TextAnchor.UpperCenter;
            verticalLayout.childControlWidth = true;
            verticalLayout.childControlHeight = true;
            verticalLayout.childForceExpandWidth = true;
            verticalLayout.childForceExpandHeight = false;

            // Create header
            GameObject headerContainer = DefaultControls.CreatePanel(new DefaultControls.Resources());
            headerContainer.name = "Header Container";
            var headerImage = headerContainer.GetComponent<Image>();
            headerImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            headerContainer.transform.SetParent(popupPanel.transform, false);
            
            var headerLayout = headerContainer.AddComponent<LayoutElement>();
            headerLayout.preferredHeight = 60;
            headerLayout.minHeight = 50;

            // Header text
            GameObject headerText = new GameObject("Header Text");
            headerText.transform.SetParent(headerContainer.transform, false);
            var headerTextComponent = headerText.AddComponent<Text>();
            headerTextComponent.text = "Loading Game";
            headerTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            headerTextComponent.fontSize = 24;
            headerTextComponent.fontStyle = FontStyle.Bold;
            headerTextComponent.color = Color.white;
            headerTextComponent.alignment = TextAnchor.MiddleCenter;
            
            var headerTextRect = headerText.GetComponent<RectTransform>();
            headerTextRect.anchorMin = Vector2.zero;
            headerTextRect.anchorMax = Vector2.one;
            headerTextRect.sizeDelta = Vector2.zero;

            // Create message text
            GameObject messageText = new GameObject("Message Text");
            messageText.transform.SetParent(popupPanel.transform, false);
            var messageTextComponent = messageText.AddComponent<Text>();
            messageTextComponent.text = "Please Wait . . .";
            messageTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            messageTextComponent.fontSize = 18;
            messageTextComponent.color = Color.white;
            messageTextComponent.alignment = TextAnchor.MiddleCenter;
            
            var messageLayout = messageText.AddComponent<LayoutElement>();
            messageLayout.preferredHeight = 80;
            messageLayout.minHeight = 60;
            
            var messageTextRect = messageText.GetComponent<RectTransform>();
            messageTextRect.anchorMin = Vector2.zero;
            messageTextRect.anchorMax = Vector2.one;
            messageTextRect.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// Gets the current loading popup instance.
        /// </summary>
        /// <returns>The current loading popup, or null if none exists.</returns>
        public static IPersistentPopup GetCurrentLoadingPopup()
        {
            return currentLoadingPopup;
        }

        /// <summary>
        /// Clears the current loading popup reference.
        /// </summary>
        public static void ClearCurrentLoadingPopup()
        {
            currentLoadingPopup = null;
        }

        /// <summary>
        /// Event handler to set the hero's position after entering the scene during custom save load.
        /// </summary>
        /// <param name="targetPosition">The target position to place the hero.</param>
        private static void OnFinishedEnteringSceneLoad(Vector2 targetPosition)
        {
            var gm = GameManager._instance;
            var hero = gm.hero_ctrl;
            
            if (hero != null)
            {
                Vector3 newPos = new Vector3(targetPosition.x, targetPosition.y, hero.transform.position.z);
                hero.transform.position = newPos;
                gm.cameraCtrl.SnapTo(newPos.x, newPos.y);
                
                // Restore the game state to fully playable
                gm.SetState(GameState.PLAYING);
                gm.inputHandler.StartAcceptingInput();
                gm.inputHandler.AllowPause();
                
                // Unpause the game
                gm.isPaused = false;
                Time.timeScale = 1f;
                TimeController.GenericTimeScale = 1f;
                
                // Transition audio snapshot
                gm.actorSnapshotUnpaused?.TransitionTo(0f);
                
                // Update UI state to match game state
                gm.ui?.SetState(UIState.PLAYING);
                
                CabbyCodesPlugin.BLogger.LogDebug(string.Format("Restored hero position to ({0}, {1}) after custom save load", targetPosition.x, targetPosition.y));
            }
            
            // Unsubscribe to avoid memory leaks
            gm.OnFinishedEnteringScene -= () => OnFinishedEnteringSceneLoad(targetPosition);
        }

        /// <summary>
        /// Attempts to save the game, prompting for overwrite if file exists.
        /// </summary>
        private static void AttemptSaveCustomGame()
        {
            // Debounce duplicate calls that can occur when input field submits then button click fires
            if (Time.realtimeSinceStartup - lastSaveAttemptTime < 0.2f)
            {
                return;
            }
            lastSaveAttemptTime = Time.realtimeSinceStartup;

            if (isSaving) return; // Another save already in progress
            // Capture the latest text directly from the UI to avoid stale reference value
            string customName = GetCurrentInputFieldText().Trim();
            
            // Immediately mark saving in progress to block any other events until callback finishes
            isSaving = true;

            string fileName = SavedGameManager.GenerateSaveFileName(customName);
            string filePath = System.IO.Path.Combine(SavedGameManager.GetCabbySavesDirectory(), fileName);
 
            if (System.IO.File.Exists(filePath))
            {
                var menu = CabbyCodesPlugin.cabbyMenu;
                if (menu == null)
                {
                    SaveCustomGame(customName);
                    return;
                }
 
                string msg = $"A save named '{customName}' already exists.\n\nOverwrite the existing file?";
                var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                    menu,
                    "Overwrite Save File?",
                    msg,
                    "Overwrite",
                    "Cancel",
                    () => { SaveCustomGame(customName); },
                    null);
                popup.Show();
            }
            else
            {
                SaveCustomGame(customName);
            }
        }

        /// <summary>
        /// Gets the latest text from the input field, even if not submitted yet.
        /// </summary>
        private static string GetCurrentInputFieldText()
        {
            if (customSaveNameInputPanel != null)
            {
                var inputField = customSaveNameInputPanel.GetInputField();
                if (inputField != null)
                {
                    return inputField.text ?? string.Empty;
                }
            }
            return customSaveNameRef.Get() ?? string.Empty;
        }

        /// <summary>
        /// Reference to the custom save name input field panel for forcing updates.
        /// </summary>
        private static InputFieldPanel<string> customSaveNameInputPanel;

        /// <summary>
        /// Synced reference for the custom save name input field.
        /// </summary>
        private static readonly BoxedReference<string> customSaveNameRef = new BoxedReference<string>("");

        /// <summary>
        /// Tracks the last loaded vanilla slot (profileID) for restoring after custom save load.
        /// </summary>
        public static int LastLoadedSlot = -1;

        /// <summary>
        /// Reference to the current loading popup for external access.
        /// </summary>
        private static IPersistentPopup currentLoadingPopup;

        /// <summary>
        /// Applies Harmony patches for custom save/load functionality.
        /// </summary>
        public static void ApplyPatches()
        {
            if (patchesApplied)
            {
                return;
            }

            patchesApplied = true;
        }

        private static float lastSaveAttemptTime = -1f;
    }
} 