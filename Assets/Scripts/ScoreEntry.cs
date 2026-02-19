using Photon.Realtime;

[System.Serializable]
public class ScoreEntry
{
    public Player player;
    public int kills;
    public int deaths;

    public ScoreEntry(Player player, int kills, int deaths)
    {
        this.player = player;
        this.kills = kills;
        this.deaths = deaths;
    }
}
