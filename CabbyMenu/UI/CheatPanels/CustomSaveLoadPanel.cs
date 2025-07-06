using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.CheatPanels
{
    /// <summary>
    /// Simple panel for custom save/load functionality.
    /// The actual UI creation is handled by CustomSaveLoadPatch.
    /// </summary>
    public class CustomSaveLoadPanel : CheatPanel
    {
        /// <summary>
        /// Initializes the custom save/load panel.
        /// </summary>
        public CustomSaveLoadPanel() : base("Custom Save/Load")
        {
            // This panel doesn't create any UI elements itself
            // All UI is created through the CustomSaveLoadPatch
        }
    }

    /// <summary>
    /// Simple string reference for custom save names.
    /// </summary>
    public class CustomSaveNameReference : ISyncedReference<string>
    {
        private string value = "";

        public string Get()
        {
            return value;
        }

        public void Set(string value)
        {
            this.value = value ?? "";
        }
    }
} 