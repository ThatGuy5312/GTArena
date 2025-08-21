using GTArena.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace GTArena.Main
{
    public class VoiceManager : MonoBehaviour
    {
        public static List<string> commands = new List<string>();
        static KeywordRecognizer phrase;

        public static volatile VoiceManager instance;
        void Awake() => instance = this;

        public static void AddCommand(Command command)
        {
            var clean = command.ToString().Replace("_", " ");
            if (!commands.Contains(clean))
            {
                commands.Add(clean);
                instance.RestartRecognizer();
            }
        }

        void Start() => TryStartRecognizer();

        void TryStartRecognizer()
        {
            if ((phrase == null || !phrase.IsRunning) && commands.Count > 0)
            {
                phrase = new KeywordRecognizer(commands.ToArray());
                phrase.OnPhraseRecognized += Recognition;
                phrase.Start();
            }
        }

        void StopRecognizer()
        {
            if (phrase != null && phrase.IsRunning)
            {
                phrase.OnPhraseRecognized -= Recognition;
                phrase.Stop();
                phrase.Dispose();
                phrase = null;
            }
        }

        void Recognition(PhraseRecognizedEventArgs args)
        {
            Debug.Log($"Recognized: {args.text}");

            if (GetCommand(args.text, out var command))
            {
                StopRecognizer();

                switch (command)
                {
                    case Command.FireBall:
                        Networking.FireBall();
                        break;
                    case Command.Magic_Missle:
                        //Commands.Fireball(AssetLoader.Wand.transform.up, AssetLoader.WandRay);
                        break;
                    case Command.Excaliber_Sword:
                        if (HealthManager.HasMana(75))
                            Networking.ExcaliberSword();
                        break;
                    case Command.Divine_Light:
                        Networking.DivineLight();
                        break;
                    case Command.Angels_Touch:
                        Networking.AngelsTouch();
                        break;
                }

                StartCoroutine(RestartAfterDelay(1f));
            }
        }

        void RestartRecognizer()
        {
            StopRecognizer();
            TryStartRecognizer();
        }

        IEnumerator RestartAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            TryStartRecognizer();
        }

        bool GetCommand(string input, out Command command)
        {
            if (input == "FireBall") { command = Command.FireBall; return true; }
            if (input == "Magic Missle") { command = Command.Magic_Missle; return true; }
            if (input == "Excaliber Sword") { command = Command.Excaliber_Sword; return true; }
            if (input == "Divine Light") { command = Command.Divine_Light; return true; }
            if (input == "Angels Touch") { command = Command.Angels_Touch; return true; }
            command = Command.None;
            return false;
        }
    }
}