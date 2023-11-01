using BepInEx;
using BepInEx.Unity.Mono;
using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI.Modders;

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
            GameObject cheat1 = cabbyMenu.AddCheatPanel();
            ToggleButton cheat1ToggleButton = new();
            //cheat1ToggleButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            new Fitter(cheat1ToggleButton.GetGameObject()).Attach(cheat1).Anchor(new Vector2(0.07f, 0.5f), new Vector2(0.07f, 0.5f)).Size(new Vector2(120, 50));

            (GameObject cheat1TextObj, _, TextMod textMod) = TextFactory.Build("Cheat Description");
            textMod.SetAlignment(TextAnchor.MiddleLeft).SetFontStyle(FontStyle.Bold);
            new Fitter(cheat1TextObj).Attach(cheat1).Anchor(new Vector2(0.2f, 0.5f), new Vector2(0.95f, 0.5f)).Size(new Vector2(0, 50));

            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
        }

        private void BuildMapCheats()
        {
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
        }

        private void BuildHunterCheats()
        {
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
            cabbyMenu.AddCheatPanel();
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