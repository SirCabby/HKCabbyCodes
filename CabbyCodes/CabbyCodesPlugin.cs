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
            CheatPanel cheat1 = new(80);
            ToggleButton cheat1ToggleButton = new();
            //cheat1ToggleButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            new Fitter(cheat1ToggleButton.GetGameObject()).Attach(cheat1.GetGameObject()).Anchor(new Vector2(0.07f, 0.5f), new Vector2(0.07f, 0.5f)).Size(new Vector2(120, 50));

            (GameObject cheat1TextObj, _, TextMod textMod) = TextFactory.Build("Cheat Description");
            textMod.SetAlignment(TextAnchor.MiddleLeft).SetFontStyle(FontStyle.Bold);
            new Fitter(cheat1TextObj).Attach(cheat1.GetGameObject()).Anchor(new Vector2(0.2f, 0.5f), new Vector2(0.95f, 0.5f)).Size(new Vector2(0, 50));

            cabbyMenu.AddCheatPanel(cheat1);

            for (int i = 0; i < 12; i++)
            {
                CheatPanel cheatPanel;
                if (i % 2  == 0)
                    cheatPanel = new CheatPanel(80);
                else
                    cheatPanel = new CheatPanel(100);
                cabbyMenu.AddCheatPanel(cheatPanel);
            }
            
        }

        private void BuildMapCheats()
        {
            for (int i = 0; i < 2; i++)
            {
                CheatPanel cheatPanel;
                if (i % 2 == 0)
                    cheatPanel = new CheatPanel(80);
                else
                    cheatPanel = new CheatPanel(100);
                cabbyMenu.AddCheatPanel(cheatPanel);
            }
        }

        private void BuildHunterCheats()
        {
            for (int i = 0; i < 4; i++)
            {
                CheatPanel cheatPanel;
                if (i % 2 == 0)
                    cheatPanel = new CheatPanel(80);
                else
                    cheatPanel = new CheatPanel(100);
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