using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

	void OnEnable ()
    {
        iTween.FadeTo(gameObject, iTween.Hash("alpha", 0.0f, 
            "time", 0.3f, "easetype", "easeInCubic", "oncomplete", "Disable"));
    }

    private void Disable()
    {
        Destroy(gameObject);
    }
}
