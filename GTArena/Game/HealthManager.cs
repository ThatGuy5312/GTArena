using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace GTArena.Game
{
    public class HealthManager : MonoBehaviour
    {
        static int health = 100;
        static int mana = 100;

        TextMeshPro handText;

        float healthDelay = 0f;
        float manaDelay = 0f;

        static string extraText;

        public static bool isDead;

        void Start()
        {
            handText = new GameObject("HandText").AddComponent<TextMeshPro>();
            handText.fontSize = 1f;
            handText.alignment = TextAlignmentOptions.Center;
            handText.renderer.material.shader = Shader.Find("GUI/Text Shader");
            handText.transform.transform.localScale = new Vector3(.6f, .6f, .6f);
        }

        void Update()
        {
            handText.transform.position = GorillaTagger.Instance.rightHandTransform.position + (Vector3.up / 4);
            handText.transform.LookAt(Camera.main.transform.position);
            handText.transform.Rotate(0, 180, 0);

            handText.gameObject.SetActive(ControllerInputPoller.instance.rightControllerPrimaryButton);

            handText.text = $"<color=#0c4011>Health: {health}</color>\n<color=#230c40>Mana: {mana}</color>\n{extraText}";

            health = Mathf.Clamp(health, 0, 100);
            mana = Mathf.Clamp(mana, 0, 100);

            if (!isDead)
            {
                if (Time.time > healthDelay && health < 100)
                {
                    health += 1;
                    healthDelay = Time.time + 1f;
                }
            }

            if (Time.time > manaDelay && mana < 100)
            {
                mana += 1;
                manaDelay = Time.time + .5f;
            }

            isDead = health == 0;
        }

        public static void Damage(int HP, int Mana)
        {
            health -= HP;
            mana -= Mana;
        }

        public static void Heal(int HP, int Mana)
        {
            health += HP;
            mana += Mana;
        }

        public static void SetHealth(int HP)
        {
            health = HP;
        }

        public static void SetMana(int Mana)
        {
            mana += Mana;
        }

        public static bool HasMana(int cost) => mana >= cost;

        public static int GetNeededMana(int cost) => cost - mana;

        public static void SetExtraText(string text) => Plugin.Instance.StartCoroutine(Set_Extra_Text(text));

        static IEnumerator Set_Extra_Text(string text)
        {
            extraText = text;
            yield return new WaitForSeconds(5);
            extraText = "";
        }
    }
}
