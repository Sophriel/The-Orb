using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    #region 버튼 클릭시 호출 함수

    public void ResumeButton()
    {
        gameObject.SetActive(false);
    }

    public void RestartButton()
    {

    }

    public void OptionButton()
    {

    }

    public void QuitButton()
    {
        GameManager.Instance.SaveData();

        Application.Quit();
    }

    #endregion

    void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
}
