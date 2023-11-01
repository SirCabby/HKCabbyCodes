using BepInEx;
using BepInEx.Unity.Mono;
using UnityEngine;
using CabbyCodes.UI;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI.Modders;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class CabbyCodesPlugin : BaseUnityPlugin
    {
        const string GUID = "cabby.cabbycodes";
        const string NAME = "Cabby Codes";
        const string VERSION = "0.0.1";

        private CabbyMenu cabbyMenu;

        private void BuildPlayerCheats()
        {
            for (int i = 0; i < 20; i++)
            {
                CheatPanel cheatPanel;
                if (i % 2  == 0)
                    cheatPanel = new TogglePanel("Cheat " + i, 80);
                else
                    cheatPanel = new CheatPanel("Cheat " + i, 100);
                cabbyMenu.AddCheatPanel(cheatPanel);
            }
            
        }

        private void BuildMapCheats()
        {
            for (int i = 0; i < 2; i++)
            {
                CheatPanel cheatPanel;
                if (i % 2 == 0)
                    cheatPanel = new TogglePanel("Cheat " + i, 80);
                else
                    cheatPanel = new CheatPanel("Cheat " + i, 100);
                cabbyMenu.AddCheatPanel(cheatPanel);
            }
        }

        private void BuildHunterCheats()
        {
            for (int i = 0; i < 4; i++)
            {
                CheatPanel cheatPanel;
                if (i % 2 == 0)
                    cheatPanel = new CheatPanel("Cheat " + i, 80);
                else
                    cheatPanel = new TogglePanel("Cheat " + i, 100);
                cabbyMenu.AddCheatPanel(cheatPanel);
            }
        }

        private void Awake()
        {
            Logger.LogInfo("Plugin cabby.cabbycodes is loaded!");
        }

        private void Start()
        {
            UnityExplorer.ExplorerStandalone.CreateInstance();

            cabbyMenu = new CabbyMenu(NAME, VERSION);
            cabbyMenu.RegisterCategory("Player", BuildPlayerCheats);
            cabbyMenu.RegisterCategory("Map", BuildMapCheats);
            cabbyMenu.RegisterCategory("Hunter", BuildHunterCheats);
        }

        private void Update()
        {
            cabbyMenu.Update();
        }
    }
}