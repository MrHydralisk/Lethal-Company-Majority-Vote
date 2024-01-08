using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace MajorityVote
{
    public class HarmonyPatches
    {
        private static readonly Type patchType;

        static HarmonyPatches()
        {
            patchType = typeof(HarmonyPatches);
            Harmony gObject = new Harmony("LethalCompany.MrHydralisk.MajorityVote");
            gObject.Patch(AccessTools.Method(typeof(HUDManager), "SetShipLeaveEarlyVotesText", (Type[])null, (Type[])null), transpiler: new HarmonyMethod(patchType, "MajorityVote_Transpiler", (Type[])null));
            gObject.Patch(AccessTools.Method(typeof(HUDManager), "Update", (Type[])null, (Type[])null), transpiler: new HarmonyMethod(patchType, "MajorityVote_Transpiler", (Type[])null));
            gObject.Patch(AccessTools.Method(typeof(TimeOfDay), "SetShipLeaveEarlyServerRpc", (Type[])null, (Type[])null), transpiler: new HarmonyMethod(patchType, "MajorityVote_Transpiler", (Type[])null));
        }

        public static IEnumerable<CodeInstruction> MajorityVote_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int startIndex = -1;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].LoadsField(AccessTools.Field(typeof(StartOfRound), "livingPlayers")))
                {
                    startIndex = i;
                    break;
                }
            }
            if (startIndex > -1)
            {
                List<CodeInstruction> instructionsToInsert = new List<CodeInstruction>();
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HarmonyPatches), "MajorityVote")));
                codes.InsertRange(startIndex + 2, instructionsToInsert);
            }
            return codes.AsEnumerable();
        }

        public static int MajorityVote(int orig)
        {
            if (Config.mVoteEnabled?.Value ?? true)
            {
                return Math.Max(Math.Min((int)MathF.Round((StartOfRound.Instance.connectedPlayersAmount + 1) * (Config.mVotePercent?.Value ?? 0.5f)), Math.Min(StartOfRound.Instance.connectedPlayersAmount, Config.mVoteMax?.Value ?? 31)), Math.Max(1, Config.mVoteMin?.Value ?? 1));
            }
            else
            {
                return orig;
            }
        }
    }
}
