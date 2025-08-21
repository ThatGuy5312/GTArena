using GTArena.Main;
using GTArena.Resources;
using System.Collections;
using UnityEngine;
using GTArena.Utilities;

namespace GTArena.Game
{
    public class Commands : MonoBehaviour
    {
        public static void DivineLight(Vector3 position) =>
            Plugin.Instance.StartCoroutine(Divine_Light(position));

        static IEnumerator Divine_Light(Vector3 position)
        {
            var light = Weapon.Create(null, position, Vector3.up * 200f, 200f, AssetLoader.DivineLight, false, Command.Divine_Light);
            var source = GameUtils.PlayClip(AssetLoader.audioDict["divinelight"], position);
            source.Item2.maxDistance = 500f;
            source.Item2.maxVolume = 1f;
            yield return new WaitForSeconds(source.Item1.clip.length);
            Destroy(light);
            Destroy(source.Item1.gameObject);
        }

        public static void AngelsTouch(Vector3 position) =>
            Plugin.Instance.StartCoroutine(Angels_Touch(position));

        static IEnumerator Angels_Touch(Vector3 position)
        {
            var sword = Weapon.Create(null, position, Vector3.up * 30f, 60f, AssetLoader.AngleHand, false, Command.Angels_Touch);
            var source = GameUtils.PlayClip(AssetLoader.audioDict["angelstouch"], position);
            source.Item2.maxDistance = 50f;
            source.Item2.maxVolume = .5f;
            yield return new WaitForSeconds(source.Item1.clip.length);
            Destroy(sword);
            Destroy(source.Item1.gameObject);
        }

        public static void Fireball(Vector3 start, Vector3 end) =>
            Plugin.Instance.StartCoroutine(Magic_Missle(start, end));

        static IEnumerator Magic_Missle(Vector3 start, Vector3 end)
        {
            var fireball = Weapon.Create(start, end, null, 30f, AssetLoader.Fireball, false, Command.FireBall);
            var source = GameUtils.PlayClip(AssetLoader.audioDict["fireball"], fireball.transform);
            source.Item2.maxDistance = 150f;
            yield return new WaitForSeconds(source.Item1.clip.length);
            Destroy(fireball);
        }

        public static void ExcaliberSword(Vector3 position) =>
            Plugin.Instance.StartCoroutine(Excaliber_Sword(position));

        static IEnumerator Excaliber_Sword(Vector3 position)
        {
            var sword = Weapon.Create(null, position, Vector3.up * 30f, 60f, AssetLoader.Excaliber, false, Command.Excaliber_Sword);
            Plugin.Instance.StartCoroutine(CreateLightning(position, Color.cyan));
            var source = GameUtils.PlayClip(AssetLoader.audioDict["lightning"], position);
            source.Item2.maxDistance = 150f;
            yield return new WaitForSeconds(source.Item1.clip.length);
            Destroy(sword);
            Destroy(source.Item1.gameObject);
        }

        static IEnumerator CreateLightning(Vector3 end, Color color, int segments = 10, float jaggedness = 1f, float delay = .05f, int snaps = 5)
        {
            var line = new GameObject("Lightning Bolt").AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.material.color = color;
            line.widthMultiplier = .15f;
            line.positionCount = segments;
            line.useWorldSpace = true;

            for (int snap = 0; snap < snaps; snap++)
            {
                var points = new Vector3[segments];
                for (int i = 0; i < segments; i++)
                {
                    var t = i / (float)(segments - 1);
                    var start = end + (Vector3.up * 20);
                    var point = Vector3.Lerp(start, end, t);
                    var offset = Vector3.Cross(end - start, Random.onUnitSphere).normalized;
                    point += offset * Random.Range(-jaggedness, jaggedness) * (1f - Mathf.Abs(t - .5f) * 2f);
                    points[i] = point;
                }
                line.SetPositions(points);
                yield return new WaitForSeconds(delay);
            }

            Destroy(line.gameObject);
        }
    }
}