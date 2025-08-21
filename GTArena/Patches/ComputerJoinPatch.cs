using GorillaNetworking;
using HarmonyLib;

namespace GTArena.Patches
{
    [HarmonyPatch(typeof(GorillaComputer), "PressButton")]
    internal class ComputerJoinPatch
    {
        static void Prefix(GorillaComputer __instance, GorillaKeyboardBindings buttonPressed)
        {
            if (JoinPatch.doPatch)
            {
                if (__instance.currentState == GorillaComputer.ComputerState.Room)
                {
                    if (buttonPressed == GorillaKeyboardBindings.enter)
                    {
                        PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(__instance.roomToJoin + "_GTArena", JoinType.Solo);
                    }
                }
            }
        }
    }
}
