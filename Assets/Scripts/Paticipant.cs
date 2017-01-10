public class Participant
{
    int playerId;
    string role;
    bool alive;

    public Participant(int playerId, string role)
    {
        this.playerId = playerId;
        this.role = role;
        alive = true;
    }

    public int GetPlayerId()
    {
        return playerId;
    }

    public string GetRole()
    {
        return role;
    }

    public void die()
    {
        alive = false;
    }

    public bool isSurvive()
    {
        return alive;
    }

    public bool isWerewolf()
    {
        return role.Equals("werewolf");
    }
}