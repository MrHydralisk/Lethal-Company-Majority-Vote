using BepInEx.Configuration;

namespace MajorityVote
{
    public class Config
    {
        public static ConfigEntry<bool> mVoteEnabled;
        public static ConfigEntry<float> mVotePercent;
        public static ConfigEntry<int> mVoteMin;
        public static ConfigEntry<int> mVoteMax;

        public static void Load()
        {
            mVoteEnabled = Plugin.config.Bind<bool>("ShipLeaveEarly", "MVoteEnabled", true, "Will count required votes for Ship to Leave Early based on total amount of players in lobby instead of amount of dead players?\nVanilla value False.\n[Votes count functionality is fully Server-side, but Clients would need this mod too for proper value on HUD]");
            mVotePercent = Plugin.config.Bind<float>("ShipLeaveEarly", "MVotePercent", 0.5f, "The percentage of all players who need to vote for Ship to Leave Early.\nValues between 0-1. Will round amount of needed votes to the nearest integer.\nP | 1 | 0,75 | 0,65 | 0,5 | 0,35 | 0,25\n2   2     2      1     1     1      1  \n3   3     2      2     2     1      1  \n4   4     3      3     2     1      1  \n5   5     4      3     3     2      1  \n6   6     5      4     3     2      2  ");
            mVoteMin = Plugin.config.Bind<int>("ShipLeaveEarly", "MVoteMin", 1, "Minimum amount of votes for Ship to Leave Early. Useful for lobby with small amount of players.\nValues between 1-31.");
            mVoteMax = Plugin.config.Bind<int>("ShipLeaveEarly", "MVoteMax", 31, "Minimum amount of votes for Ship to Leave Early. Useful for lobby with giant amount of players.\nValues between 1-31.");
        }
    }
}
