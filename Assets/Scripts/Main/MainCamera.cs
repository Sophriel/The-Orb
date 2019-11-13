using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public enum MainSceneStatus
{
    Intro_O2Cube,
    Intro_KkoryByul,
    Intro_TheOrb,
    Main
}

public class MainCamera : MonoBehaviour
{
    public MainSceneStatus Now = MainSceneStatus.Intro_O2Cube;

    public VideoPlayer[] Intro;

    public GameObject Fade;
    public Material FadeMaterial;

    public Image choice1, choice2, choice3, choice4;

    void Start()
    {
        IgnoreAlpha();

        Intro[(int)Now].Prepare();

        //  게임 시작 페이드인
        FadeMaterial = Fade.GetComponent<SpriteRenderer>().material;
        
        StartCoroutine("FadeIntro");
    }

    //  알파영역에 대한 이벤트 제거
    void IgnoreAlpha()
    {
        float Alpha = 0.1f;

        choice1.alphaHitTestMinimumThreshold = Alpha;
        choice2.alphaHitTestMinimumThreshold = Alpha;
        choice3.alphaHitTestMinimumThreshold = Alpha;
        choice4.alphaHitTestMinimumThreshold = Alpha;
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

    #region 인트로 연출

    IEnumerator FadeIntro()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            FadeIn(1.0f);

            yield return new WaitForSeconds((float)Intro[(int)Now].clip.length - 1.0f);

            FadeOut(1.0f);

            yield return new WaitForSeconds(1.0f);

            SkipIntro();

            yield break;
        }
    }

    //  인트로 영상 스킵 연출
    public void SkipIntro()
    {
        switch (Now)
        {
            case MainSceneStatus.Intro_O2Cube:
            case MainSceneStatus.Intro_KkoryByul:
                StopCoroutine("FadeIntro");
                Intro[(int)Now].gameObject.SetActive(false);
                Now++;
                Intro[(int)Now].Prepare();
                Intro[(int)Now].gameObject.SetActive(true);
                StartCoroutine("FadeIntro");
                break;
            case MainSceneStatus.Intro_TheOrb:
                StopCoroutine("FadeIntro");
                Intro[(int)Now].gameObject.SetActive(false);
                Now++;
                FadeIn(1.0f);
                choice1.gameObject.SetActive(true);
                choice2.gameObject.SetActive(true);
                choice3.gameObject.SetActive(true);
                choice4.gameObject.SetActive(true);
                break;
            case MainSceneStatus.Main:
                break;
            default:
                break;
        }
    }

    #endregion

    #region 버튼 클릭시 호출 함수

    //  PLAY 버튼 연출
    public void PlayButton()
    {
        //  진행중인 게임이 있음
        if (GameManager.Instance.Difficulty != Difficulty.New)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", Vector3.forward * 100.0f,
                "easetype", "easeInCirc", "time", 1.5f, "oncomplete", "SceneChange"));

            FadeOut(1.5f);

            //  ★팝업창 띄워서 이전 게임을 불러올지 물어봄
        }

        else
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", Vector3.forward * 100.0f,
                "easetype", "easeInCirc", "time", 1.5f, "oncomplete", "SceneChange"));

            FadeOut(1.5f);
        }
    }

    //  OPTION 버튼 연출
    public void OptionButton()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", Vector3.forward * 100.0f,
            "easetype", "easeInCirc", "time", 2.0f));

        FadeOut(2.0f);
    }

    //  EXIT 버튼
    public void ExitButton()
    {
        Application.Quit();
    }

    #endregion

    //  난이도 선택 씬 이동
    private void SceneChange()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level");
    }
}

