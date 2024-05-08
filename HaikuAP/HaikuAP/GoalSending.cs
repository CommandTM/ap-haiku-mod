using System.Collections;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;

namespace HaikuAP;

public class GoalSending
{
    public static void Init()
    {
        On.VirusDefeated.EndingSequence += _sendGoalVer;
    }

    private static IEnumerator _sendGoalVer(On.VirusDefeated.orig_EndingSequence orig, VirusDefeated self)
    {
        orig(self);
        StatusUpdatePacket goalPacket = new StatusUpdatePacket();
        goalPacket.Status = ArchipelagoClientState.ClientGoal;
        APPlugin.apSession.Socket.SendPacket(goalPacket);
        yield break;
    }
}