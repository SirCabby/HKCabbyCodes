using BepInEx;
using BepInEx.Unity.Mono;
using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI;

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
            CabbyMenu.AttachAndAnchor(cheat1ToggleButton.GetGameObject(), cheat1.transform, new Vector2(0.07f, 0.5f), new Vector2(0.07f, 0.5f), new Vector2(120, 50));

            GameObject cheat1TextObj = DefaultControls.CreateText(new DefaultControls.Resources());
            cheat1TextObj.name = "Category Text";
            CabbyMenu.AttachAndAnchor(cheat1TextObj, cheat1.transform, new Vector2(0.2f, 0.5f), new Vector2(0.95f, 0.5f), new Vector2(0, 50));

            Text cheat1Text = cheat1TextObj.GetComponent<Text>();
            cheat1Text.text = "Select Category";
            cheat1Text.fontSize = 36;
            cheat1Text.color = Color.black;
            cheat1Text.alignment = TextAnchor.MiddleLeft;
            cheat1Text.fontStyle = FontStyle.Bold;

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