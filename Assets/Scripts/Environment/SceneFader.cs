using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private AnimationCurve animationCurve;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    private IEnumerator FadeIn()
    {
        float time = 1f;

        while (time > 0f)
        {
            time -= Time.deltaTime * 0.5f;
            float curve = animationCurve.Evaluate(time);
            fadeImage.color = new Color(0f, 0f, 0f, curve);
            yield return 0;
        }
    }
    private IEnumerator FadeOut(string scene)
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * 0.5f;
            float curve = animationCurve.Evaluate(time);
            fadeImage.color = new Color(0f, 0f, 0f, curve);
            yield return 0;
        }
        SceneManager.LoadScene(scene);
    }
}
