using GorillaNetworking;
using HarmonyLib;

namespace GTArena.Patches
{
    [HarmonyPatch(typeof(GorillaNetworkJoinTrigger), "OnBoxTriggered")]
    internal class JoinPatch
    {
        public static bool doPatch = true;
        static void Prefix()
        {
            if (doPatch)
            {
                var method = typeof(PhotonNetworkController).GetMethod("RandomRoomName", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var code = (string)method.Invoke(PhotonNetworkController.Instance, null) + "_GTArena";
                PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(code, JoinType.Solo);
            }
        }
    }
}
