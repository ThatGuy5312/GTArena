using GTArena.Main;
using GTArena.Utilities;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GTArena.Resources
{
    public class Scroll : MonoBehaviour
    {
        public static void Create(Command command, Vector3 position)
        {
            var scroll = Instantiate(AssetLoader.Scroll);
            scroll.transform.Find("Canvas/Text").GetComponent<Text>().text = command.ToString().Replace("_", " ");
            scroll.AddComponent<Scroll>().command = command;
            scroll.transform.localScale = new Vector3(.08f, .08f, .08f);
            scroll.transform.position = position;
        }

        Command command;
        void Update()
        {
            if (Vector3.Distance(transform.position, GorillaTagger.Instance.rightHandTransform.position) < .1f)
            {
                if (ControllerInputPoller.instance.rightGrab)
                {
                    if (command == Command.FireBall)
                    {
                        VoiceManager.AddCommand(Command.FireBall);
                        PlayerPrefs.SetString("hasFireball", "");
                        GameUtils.PlayClip(AssetLoader.audioDict["scrollpickup"], transform.position);
                        Destroy(gameObject);
                    }

                    if (command == Command.Excaliber_Sword)
                    {
                        VoiceManager.AddCommand(Command.Excaliber_Sword);
                        PlayerPrefs.SetString("hasExcaliber", "");
                        GameUtils.PlayClip(AssetLoader.audioDict["scrollpickup"], transform.position);
                        Destroy(gameObject);
                    }

                    if (command == Command.Divine_Light)
                    {
                        VoiceManager.AddCommand(Command.Divine_Light);
                        PlayerPrefs.SetString("hasDivine", "");
                        GameUtils.PlayClip(AssetLoader.audioDict["scrollpickup"], transform.position);
                        Destroy(gameObject);
                    }

                    if (command == Command.Angels_Touch)
                    {
                        VoiceManager.AddCommand(Command.Angels_Touch);
                        PlayerPrefs.SetString("hasAngel", "");
                        GameUtils.PlayClip(AssetLoader.audioDict["scrollpickup"], transform.position);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}