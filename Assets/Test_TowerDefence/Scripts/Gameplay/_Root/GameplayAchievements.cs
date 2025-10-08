[System.Serializable]
public class GameplayAchievements 
{
    private double lowScoreThreshold;
    private double highScoreThreshold;

    private int lives;
     

    public void SetPlayerStartLives(int lives)
    {
        this.lives = lives;
        SetScores();
    }

    private void SetScores()
    {
        lowScoreThreshold = ((double)lives / 100) * 35;
        highScoreThreshold = ((double)lives / 100) * 70;
    }

    public int CalculateStars(int lives)
    {
        if (lives < lowScoreThreshold)
        {
            return 1;
        }
        else if (lives >= highScoreThreshold) 
        {
            return 3;
        }
        else
        {
            return 2;
        }
    }
}
