using UnityEngine;

public class ReachedStarsUI : MonoBehaviour
{
    [SerializeField] GameObject[] stars;


    public void ShowStars(int starsAmount)
    {
        for (int i = 0; i < starsAmount; i++)
        {
            stars[i].SetActive(true);
        }
    }
}
