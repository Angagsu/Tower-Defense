public class GameplayAchievements 
{
    private double lowScoreThreshold;
    private double highScoreThreshold;

    private int lives;
    private int starAmount;



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
            return starAmount = 1;
        }
        else if (lives >= highScoreThreshold) 
        {
            return starAmount = 3;
        }
        else
        {
            return starAmount = 2;
        }
    }
}
