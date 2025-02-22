using System;
using Exiled.API.Features;
using LeaderboardPlugin.Handlers;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Player;

namespace LeaderboardPlugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "Leaderboard Plugin";
        public override string Author => "MONCEF50G";
        public override Version Version => new Version("1.0.0");

        public static Plugin Instance { get; private set; }
        private LeaderboardHandler leaderboardHandler;

        public override void OnEnabled()
        {
            Instance = this;
            leaderboardHandler = new LeaderboardHandler();
            // التسجيل على أحداث اللاعبين والختام للجولة
            Exiled.Events.Handlers.Player.Died += leaderboardHandler.OnPlayerDied;
            Exiled.Events.Handlers.Server.RoundEnded += leaderboardHandler.OnRoundEnded;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Died -= leaderboardHandler.OnPlayerDied;
            Exiled.Events.Handlers.Server.RoundEnded -= leaderboardHandler.OnRoundEnded;
            leaderboardHandler = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}