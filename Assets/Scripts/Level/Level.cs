using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public GameObject Fade;
    public Material FadeMaterial;

    void Start ()
    {
        FadeMaterial = Fade.GetComponent<SpriteRenderer>().material;
        FadeIn(1.0f);
    }

    #region 페이딩

    public void FadeIn(float time)
    {
        FadeMaterial.color = Color.black;

        iTween.ColorTo(Fade, iTween.Hash("alpha", 0.0f,
            "easetype", "easeInSine", "time", time));
    }

    public void FadeOut(float time)
    {
        FadeMaterial.color = Color.clear;

        iTween.ColorTo(Fade, iTween.Hash("alpha", 1.0f,
            "easetype", "easeInSine", "time", time));
    }

    #endregion

    public void SetLevel(int level)
    {
        float time = 1.0f;

        FadeOut(time);
        Invoke("SceneChange", time);

        GameManager.Instance.Difficulty = (Difficulty)level;
    }

    private void SceneChange()
    {
        //  ★ChoosePet -> ChooseCharacter로 수정

        //  처음 시작
        if (GameManager.Instance.PreStageDifficulty == Difficulty.New)
        {
            GameManager.Instance.Round = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ingame");
        }

        //  이전 스테이지 클리어
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("ChoosePet");
    }
}
