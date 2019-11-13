using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacter : MonoBehaviour
{
    public GameObject Fade;
    public Material FadeMaterial;

    void Start()
    {
        //  페이드인
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

    //  버튼
    public void Confirm()
    {
        float time = 1.0f;

        Invoke("SceneChange", time);
        FadeOut(time);
    }

    private void SceneChange()
    {
        GameManager.Instance.Round = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ingame");
    }
}
