using GTArena.Main;
using GTArena.Utilities;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

namespace GTArena.Game
{
    public class Networking : MonoBehaviourPun
    {
        public static volatile Networking instance;
        void Awake() => instance = this;

        static void SendRPC(string method, RpcTarget target, object[] data)
        {
            if (PhotonNetwork.InRoom)
                instance.photonView.RPC(method, target, data);
            else instance.Invoke(method, data);
        }

        public static void FireBall()
        {
            if (HealthManager.isDead) return;

            if (HealthManager.HasMana(10))
            {
                HealthManager.Damage(0, 10);
                SendRPC("Fire_Ball", RpcTarget.All, new object[]
                {
                    AssetLoader.Wand.transform.position,
                    AssetLoader.WandRay.point
                });
            }
            else
            {
                HealthManager.SetExtraText($"Insufficient mana, mana needed : {HealthManager.GetNeededMana(5)}");
            }
        }

        public static void ExcaliberSword()
        {
            if (HealthManager.isDead) return;

            if (HealthManager.HasMana(50))
            {
                HealthManager.Damage(0, 50);
                SendRPC("Excaliber_Sword", RpcTarget.All, new object[] { AssetLoader.WandRay.point });
            }
            else
            {
                HealthManager.SetExtraText($"Insufficient mana, mana needed : {HealthManager.GetNeededMana(50)}");
            }
        }

        public static void DivineLight()
        {
            if (HealthManager.isDead) return;

            if (HealthManager.HasMana(100))
            {
                HealthManager.Damage(0, 100);
                SendRPC("Divine_Light", RpcTarget.All, new object[] { AssetLoader.WandRay.point });
            }
            else
            {
                HealthManager.SetExtraText($"Insufficient mana, mana needed : {HealthManager.GetNeededMana(100)}");
            }
        }

        public static void AngelsTouch()
        {
            if (HealthManager.HasMana(75))
            {
                HealthManager.Damage(0, 75);
                SendRPC("Angels_Touch", RpcTarget.All, new object[] { AssetLoader.WandRay.point });
            }
            else
            {
                HealthManager.SetExtraText($"Insufficient mana, mana needed : {HealthManager.GetNeededMana(75)}");
            }
        }

        [PunRPC]
        void Fire_Ball(Vector3 start, Vector3 end)
        {
            Commands.Fireball(start, end);
        }

        [PunRPC]
        void Excaliber_Sword(Vector3 position)
        {
            Commands.ExcaliberSword(position);
        }

        [PunRPC]
        void Divine_Light(Vector3 position)
        {
            Commands.DivineLight(position);
        }

        [PunRPC]
        void Angels_Touch(Vector3 position)
        {
            Commands.AngelsTouch(position);
        }
    }
}