using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*هذا الclass اقوم يوجد به كل الcontrols الخاصه بمشهد البداية*/
public class UI_Manager : MonoBehaviour
{
    /*هذا المتغير الذي يحمل كل الUI الخاصه بمشهد البداية فقط*/
    public Transform uiContainer;

    /*هذا المتغير الذي يحمل صور الsound button */
    public Image soundBtn;
    public Sprite[] soundBtnSprites;

    /*هذان المتغيران لحمل مصادر الصوت سواء موسيقي او الصوت */
    private AudioSource sound;
    public Transform lava;


    /*هذه الدالة تنادي من البرنامج تلقائيا قبل كل الدوال*/
    void Awake()
    {
        sound = GetComponent<AudioSource>();
        /*اقوم هنا بجلب القيمه المخزنه للsound في الذاكره لاعرف هل هو مخزن mute او لا 
        0 يعني الصوت مفتوح
        1 يعني الصوت مغلق*/
        if (PlayerPrefs.HasKey("Sound"))
        {
            soundBtn.sprite = (PlayerPrefs.GetInt("Sound") == 1) ? soundBtnSprites[0] : soundBtnSprites[1];
            sound.mute = (PlayerPrefs.GetInt("Sound") == 1) ? false : true;
            lava.GetComponent<AudioSource>().mute = (PlayerPrefs.GetInt("Sound") == 1) ? false : true;
        }
    }

    /*هذه الداله التي تنادي عند الانتقال للمشهد التالي هي والداله التي تليها وتتحكم في قيم الprogress الموجوده في الloading bar*/
    private void LoadLevel(int sceneNum)
    {
        uiContainer.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(sceneNum));
    }

    IEnumerator LoadAsynchronously(int sceneNum)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNum);
        while (!operation.isDone)
        {
            uiContainer.GetChild(1).GetChild(0).GetComponent<Slider>().value = operation.progress;
            yield return null;
        }
    }

    /*هذه الداله بها كل الcontrols الخاصه بالbuttons الموجوده في القائمه الرائيسيه فقط*/
    public void MainMenuUI(int index)
    {
        sound.Play();
        if (index == 0)
        {
            LoadLevel(1);
        }
        else if (index == 1)
        {
            Application.Quit();
        }
        else if (index == 2)
        {
            soundBtn.sprite = (soundBtn.sprite == soundBtnSprites[1]) ? soundBtnSprites[0] : soundBtnSprites[1];
            int soundValue = (soundBtn.sprite == soundBtnSprites[0]) ? 1 : 0;
            PlayerPrefs.SetInt("Sound", soundValue);
            sound.mute = !sound.mute;
            lava.GetComponent<AudioSource>().mute = !lava.GetComponent<AudioSource>().mute;
        }
    }
}
