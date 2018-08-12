using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public RectTransform PlayButton;
    public RectTransform Credits;

    public string SceneToLoad;

	// Use this for initialization
	void Start ()
    {
        PlayButton.localPosition = new Vector3(0.0f, (-Screen.height * 0.2f), 0.0f);

        Credits.localPosition = new Vector3(((Screen.width * 0.45f) - (Credits.sizeDelta.x / 2)), ((-Screen.height * 0.45f) + (Credits.sizeDelta.y / 2)), 0.0f);

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnPlay()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}
