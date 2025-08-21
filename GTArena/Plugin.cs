using BepInEx;
using GTArena.Game;
using GTArena.Main;
using GTArena.Resources;
using HarmonyLib;
using Photon.Pun;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace GTArena
{
    internal static class PluginInfo {
        public const string Guid = "com.thatguy.gorillatag.gtarena";
        public const string Name = "Gorilla Tag Mage Arena (GTArena)";
        public const string Version = "1.0.0";
    }
    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static volatile Plugin Instance;
        void Awake() => Instance = this;

        bool reset = true;

        //string[] boardText = new string[2];

        GameObject gameModeButton;
        GameObject load;

        void Start()
        {
            new Harmony(PluginInfo.Guid).PatchAll(Assembly.GetExecutingAssembly());
            load = new GameObject("GTArena Loader");
            DontDestroyOnLoad(load);

            GorillaTagger.OnPlayerSpawned(() =>
            {
                load.AddComponent<VoiceManager>();
                load.AddComponent<AssetLoader>();
                load.AddComponent<Networking>();
                load.AddComponent<Commands>();
                load.AddComponent<HealthManager>();
            });

            if (reset)
            {
                PlayerPrefs.DeleteKey("hasFireball");
                PlayerPrefs.DeleteKey("hasExcaliber");
                PlayerPrefs.DeleteKey("hasDivine");
                PlayerPrefs.DeleteKey("hasAngel");
            }

            //ZoneManagement.instance.GetPrimaryGameObject(VRRig.LocalRig.zoneEntity.currentZone);

            //boardText[0] = GameObject.Find("CodeOfConductHeadingText").GetComponent<TMP_Text>().text;
            //boardText[1] = GameObject.Find("COCBodyText_TitleData").GetComponent<TMP_Text>().text;
        }

        void Update()
        {
            if (PhotonNetwork.CurrentRoom.Name.Contains("_GTArena") || !PhotonNetwork.InRoom)
            {
                load.SetActive(true);
                AssetLoader.Wand.SetActive(true);
            }
            else
            {
                load.SetActive(false);
                AssetLoader.Wand.SetActive(false);
            }

            var header = GameObject.Find("CodeOfConductHeadingText");
            var body = GameObject.Find("COCBodyText_TitleData");

            if (header == null || body == null)
                return;

            var headerText = header.GetComponent<TMP_Text>();
            var bodyText = body.GetComponent<TMP_Text>();

            headerText.text = "Collected Spells";

            if (VoiceManager.commands.Count == 0) bodyText.text = "You have no spells collected";
            else
            {
                bodyText.text = "";
                foreach (var com in VoiceManager.commands)
                    bodyText.text += $"{com}\n";
            }

            if (gameModeButton == null)
            {
                gameModeButton = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gameModeButton.transform.position = new Vector3(-66.9f, 12.2f, -82.6f);
                gameModeButton.transform.localScale = Vector3.one / 3;
                gameModeButton.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                Destroy(gameModeButton.GetComponent<Collider>());
                gameModeButton.AddComponent<GameModeButton>();
            }
        }

        public static Shader ParticleShader => Shader.Find("Universal Render Pipeline/Particles/Unlit");
    }
}
