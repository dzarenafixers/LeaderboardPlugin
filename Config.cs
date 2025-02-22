using Exiled.API.Interfaces;
using System.ComponentModel;

namespace LeaderboardPlugin
{
    public class Config : IConfig
    {
        [Description("Enable or disable the plugin.")]
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; }

        [Description("Message format for leaderboard display. Use {leaderboard} as placeholder for the list.")]
        public string LeaderboardMessage { get; set; } = "<color=green>Leaderboard:\n{leaderboard}</color>";
    }
}