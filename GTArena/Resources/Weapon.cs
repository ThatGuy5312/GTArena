using GTArena.Game;
using GTArena.Main;
using System.Collections;
using UnityEngine;

namespace GTArena.Resources
{
    public class Weapon : MonoBehaviour
    {
        public static GameObject Create(Vector3? start, Vector3 end, Vector3? addAmount, float speed, GameObject copy, bool moveToPos, Command weapon)
        {
            var sword = Instantiate(copy);
            var weap = sword.AddComponent<Weapon>();
            weap.targetPosition = end;
            weap.speed = speed;
            weap.weapon = weapon;
            weap.moveToPos = moveToPos;

            if (start.HasValue)
                weap.startPosition = start.Value;
            else weap.startPosition = end + addAmount.Value;

            return sword;
        }

        Vector3 startPosition;
        Vector3 targetPosition;
        float speed = 15f;
        float damageDelay = 0f;
        bool moving = true;
        bool hasHit = false;
        bool moveToPos;
        Command weapon;

        void Start() => transform.position = startPosition;

        void Update()
        {
            if (moving && !moveToPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < .01f)
                {
                    transform.position = targetPosition;
                    moving = false;
                }
            }
            else if (moveToPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }

            if (!moving && !hasHit && weapon == Command.FireBall)
            {
                hasHit = true;
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddExplosionForce(10, transform.position, 3);
            }

            if (Time.time > damageDelay)
            {
                if (Vector3.Distance(transform.position, GorillaTagger.Instance.transform.position) < 3f)
                {
                    if (weapon == Command.FireBall)
                    {
                        HealthManager.Damage(10, 0);
                    }
                    if (weapon == Command.Excaliber_Sword)
                    {
                        HealthManager.Damage(50, 0);
                    }
                    if (weapon == Command.Divine_Light)
                    {
                        HealthManager.Damage(100, 0);
                    }
                    if (weapon == Command.Angels_Touch)
                    {
                        HealthManager.SetHealth(100);
                    }
                }
                damageDelay = Time.time + .5f;
            }
        }
    }
}