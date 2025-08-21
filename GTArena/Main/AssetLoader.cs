using GTArena.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GTArena.Main
{
    public class AssetLoader : MonoBehaviour
    {
        public static Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

        AssetBundle bundle;

        public static GameObject Wand { get; private set; }
        public static GameObject Fireball { get; private set; }
        public static GameObject MagicMissle { get; private set; }
        public static GameObject Excaliber { get; private set; }
        public static GameObject DivineLight { get; private set; }
        public static GameObject AngleHand { get; private set; }
        public static GameObject Scroll { get; private set; }

        public static RaycastHit WandRay { get; private set; }

        void Start()
        {
            bundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("GTArena.Resources.gtarena"));

            if (bundle == null) return;

            Wand = Instantiate(bundle.LoadAsset<GameObject>("Wand"));
            Fireball = Instantiate(bundle.LoadAsset<GameObject>("Fireball"));
            MagicMissle = Instantiate(bundle.LoadAsset<GameObject>("MagicMissle"));
            Excaliber = Instantiate(bundle.LoadAsset<GameObject>("Sword"));
            DivineLight = Instantiate(bundle.LoadAsset<GameObject>("DivineLight"));
            AngleHand = Instantiate(bundle.LoadAsset<GameObject>("Hand"));
            Scroll = Instantiate(bundle.LoadAsset<GameObject>("Scroll"));

            Wand.transform.Find("Cylinder").GetComponent<Renderer>().materials[0].shader = Shader.Find("Universal Render Pipeline/Lit");
            Wand.transform.Find("Cylinder").GetComponent<Renderer>().materials[1].shader = Shader.Find("Universal Render Pipeline/Lit");
            MagicMissle.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            Excaliber.transform.Find("default").GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            DivineLight.transform.Find("lightTube").GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            AngleHand.transform.Find("g_arm_LO").GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            Fireball.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            Scroll.transform.Find("Scroll").GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            Fireball.transform.Find("Particle System").GetComponent<ParticleSystemRenderer>().material.shader = Plugin.ParticleShader;

            foreach (var clip in bundle.LoadAllAssets<AudioClip>())
            {
                audioDict.Add(clip.name, clip);
                Debug.Log($"[GTArena] loaded clip {clip.name}");
            }

            DivineLight.transform.position = Vector3.one * 999;
            AngleHand.transform.position = Vector3.one * 999;

            Wand.transform.parent = GorillaTagger.Instance.rightHandTransform;
            Wand.transform.localPosition = new Vector3(0, 0, -.05f);
            Wand.transform.localRotation = Quaternion.Euler(new Vector3(20.28f, 58.54f, 51.09f));

            if (!PlayerPrefs.HasKey("hasFireball"))
                Resources.Scroll.Create(Command.FireBall, new Vector3(-66.9f, 11.4f, -82.6f));
            else VoiceManager.AddCommand(Command.FireBall);

            if (!PlayerPrefs.HasKey("hasExcaliber"))
                Resources.Scroll.Create(Command.Excaliber_Sword, new Vector3(-90.05f, -29.02f, -22.12f));
            else VoiceManager.AddCommand(Command.Excaliber_Sword);

            if (!PlayerPrefs.HasKey("hasDivine"))
                Resources.Scroll.Create(Command.Divine_Light, new Vector3(421.50f, -139.75f, 350.78f));
            else VoiceManager.AddCommand(Command.Divine_Light);

            if (!PlayerPrefs.HasKey("hasAngel"))
                Resources.Scroll.Create(Command.Angels_Touch, new Vector3(-1.71f, 10.40f, 26.71f));
            else VoiceManager.AddCommand(Command.Angels_Touch);
        }

        void Update()
        {
            if (Physics.Raycast(Wand.transform.position, Wand.transform.up, out var hit, GunMasks()))
                WandRay = hit;

            if (ControllerInputPoller.instance.rightControllerIndexFloat > .5f)
            {
                if (pointer == null)
                {
                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    pointer.transform.localScale = Vector3.one / 3;
                    pointer.GetComponent<Renderer>().material = new Material(Shader.Find("GorillaTag/UberShader")) { color = Color.cyan };
                    Destroy(pointer.GetComponent<Collider>());
                    pointer.AddComponent<LineRenderer>();
                }
                pointer.SetActive(true);
                pointer.transform.position = WandRay.point;
                var line = pointer.GetComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.blue };
                line.startWidth = .05f;
                line.endWidth = .05f;
                line.positionCount = 2;
                line.SetPosition(0, Wand.transform.position);
                line.SetPosition(1, WandRay.point);
            } else pointer?.SetActive(false);
        }

        int GunMasks() => GorillaLocomotion.GTPlayer.Instance.locomotionEnabledLayers + LayerMask.GetMask("Gorilla Tag Collider", "Gorilla Body Collider");

        GameObject pointer;
    }
}