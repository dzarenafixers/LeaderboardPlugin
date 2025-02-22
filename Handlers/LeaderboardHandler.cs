using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using System.Linq;

namespace LeaderboardPlugin.Handlers
{
    public class LeaderboardHandler
    {
        // نقوم بتخزين إحصائيات اللاعبين باستخدام الـ UserId كمفتاح
        private readonly Dictionary<string, LeaderboardEntry> playerStats = new Dictionary<string, LeaderboardEntry>();

        // يُستدعى عند وفاة اللاعب
        public void OnPlayerDied(DiedEventArgs ev)
        {
            // إذا كان هناك قاتل، نضيف له نقطة قتل
            if (ev.Attacker != null)
            {
                if (!playerStats.ContainsKey(ev.Attacker.UserId))
                    playerStats[ev.Attacker.UserId] = new LeaderboardEntry(ev.Attacker.Nickname);
                playerStats[ev.Attacker.UserId].Kills++;
            }

            // نضيف إحصائيات الضحية (التي ماتت)
            if (!playerStats.ContainsKey(ev.Player.UserId))
                playerStats[ev.Player.UserId] = new LeaderboardEntry(ev.Player.Nickname);
            playerStats[ev.Player.UserId].Deaths++;
        }

        // عند انتهاء الجولة، نقوم بعرض لوحة المتصدرين
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            // ترتيب اللاعبين حسب عدد القتلات، وفي حالة التساوي يتم ترتيبهم حسب عدد الوفيات (أقل عدد أفضل)
            var leaderboard = playerStats.Values
                                .OrderByDescending(entry => entry.Kills)
                                .ThenBy(entry => entry.Deaths)
                                .ToList();

            string leaderboardString = "";
            int rank = 1;
            foreach (var entry in leaderboard)
            {
                leaderboardString += $"{rank}. {entry.Name} - Kills: {entry.Kills} | Deaths: {entry.Deaths}\n";
                rank++;
            }

            string message = Plugin.Instance.Config.LeaderboardMessage.Replace("{leaderboard}", leaderboardString);
            Map.Broadcast(15, message);

            // مسح الإحصائيات استعداداً للجولة التالية
            playerStats.Clear();
        }
    }

    public class LeaderboardEntry
    {
        public string Name { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }

        public LeaderboardEntry(string name)
        {
            Name = name;
            Kills = 0;
            Deaths = 0;
        }
    }
}
