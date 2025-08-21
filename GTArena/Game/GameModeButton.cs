using GTArena.Patches;
using UnityEngine;

namespace GTArena.Game
{
    public class GameModeButton : MonoBehaviour
    {
        float tapDelay = 0f;

        void Update()
        {
            if (Vector3.Distance(transform.position, GorillaTagger.Instance.rightHandTransform.position) < .3f)
            {
                if (Time.time > tapDelay)
                {
                    JoinPatch.doPatch = !JoinPatch.doPatch;
                    tapDelay = Time.time + .5f;
                }
            }
            GetComponent<Renderer>().material.color = JoinPatch.doPatch ? Color.green : Color.red;
        }
    }
}
