using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*هذا الclass تتحكم بكل الcontrols الخاصه بالUI الموجود في المشهد التاني فقط*/
public class UI_Manager_Levels : MonoBehaviour
{

    /*هذا المتغير استعمله لادخل الي هذا الملف من اللمفات الاخري وهذه تسمي singlton design pattern*/
    private static UI_Manager_Levels instance;

    /* من هنا  انادي المتغير الذي اعلاه*/
    public static UI_Manager_Levels Instance{ get { return instance; } }

    public Transform pauseMenu, loadingContainer, winMenu, uiCanvas;

    //pauseBtn
    public Button pauseBtn;

    /*هذا المتغير الذي يحمل صور الsound button */
    public Image soundBtn;
    public Sprite[] soundBtnSprites;

    /*هذا المتغير الذي يحمل الhealth*/
    public Slider healthSlider;

    public Transform cameraGameOverSound;

    public bool gameOver;
  
    /*هذا المتغير لحمل مصادر الصوت */
    private AudioSource sound;

    /*هذا المتغير يحمل الlava*/
    public Transform lava;


    /*هذه الدالة تنادي من البرنامج تلقائيا قبل كل الدوال*/
    void Awake()
    {
        instance = this;

        gameOver = false;
        sound = GetComponent<AudioSource>();


        /*اقوم هنا بجلب القيمه المخزنه للsound في الذاكره لاعرف هل هو مخزن mute او لا 
        0 يعني الصوت مفتوح
        1 يعني الصوت مغلق*/
        if (PlayerPrefs.HasKey("Sound"))
        {
            soundBtn.sprite = (PlayerPrefs.GetInt("Sound") == 1) ? soundBtnSprites[0] : soundBtnSprites[1];
            sound.mute = (PlayerPrefs.GetInt("Sound") == 1) ? false : true;
            lava.GetComponent<AudioSource>().mute = (PlayerPrefs.GetInt("Sound") == 1) ? false : true;
            uiCanvas.GetComponent<AudioSource>().mute = (PlayerPrefs.GetInt("Sound") == 1) ? false : true;
        }
    }

    /*هذه الدالة تقوم باظهار الpause menu*/
    public void Toggel_PauseMenu()
    {
        sound.Play();

        /*لغلق صوت الlava*/
        if (lava.GetComponent<AudioSource>().isPlaying)
        {
            lava.GetComponent<AudioSource>().Stop();
        }
        else if (!lava.GetComponent<AudioSource>().isPlaying)
        {
            lava.GetComponent<AudioSource>().Play();
        }

        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        /*لغلق وفتح الUI الذي كان مفتوح قبل فتح او غلق الpause menu*/
        if (LevelManager.Instance.startCanvas.activeSelf || LevelManager.Instance.decisionUI.activeSelf)
        {
            LevelManager.Instance.startCanvas.SetActive(false);
            LevelManager.Instance.decisionUI.SetActive(false);

        }
        else if (!LevelManager.Instance.startCanvas.activeSelf && !LevelManager.Instance.decisionUI.activeSelf)
        {
            if (LevelManager.Instance.startNew)
            {
                LevelManager.Instance.startCanvas.SetActive(true);
                LevelManager.Instance.decisionUI.SetActive(false);
            }
            else if (LevelManager.Instance.startCheck)
            {
                LevelManager.Instance.startCanvas.SetActive(false);
                LevelManager.Instance.decisionUI.SetActive(true);
            }
        }
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
        pauseBtn.interactable = !pauseBtn.interactable;
    }

    /*هذه الدالة يتم استدعاءها كلما حدث تغير في الhealth slider وينفذ ما بداخلها حين يكون الhealth بيساوي صفر*/
    public void DieMenu()
    {
        if (healthSlider.value <= 0)
        {
            if (lava.GetComponent<AudioSource>().isPlaying)
            {
                lava.GetComponent<AudioSource>().Stop();
            }
            cameraGameOverSound.GetComponent<AudioSource>().Play();
            if (PlayerPrefs.GetInt("bestScore") < int.Parse(LevelManager.Instance.scoreText.text))
            {
                PlayerPrefs.SetInt("bestScore", int.Parse(LevelManager.Instance.scoreText.text));
            }
            PlayerPrefs.SetInt("score", int.Parse(LevelManager.Instance.scoreText.text));
            Time.timeScale = (Time.timeScale == 0) ? 1 : 0;

            winMenu.GetChild(0).GetChild(2).GetComponent<Text>().text = "Score: " + LevelManager.Instance.scoreText.text;
            winMenu.GetChild(0).GetChild(3).GetComponent<Text>().text = "Best Score: " + PlayerPrefs.GetInt("bestScore").ToString();
            LevelManager.Instance.bestScoreText.gameObject.SetActive(false);

            OpenWinWindow();
        }
    }

    /*تقوم بفتح القائمة النهائيه ويتم استدعاءها في الداله التي قبلها*/
    void OpenWinWindow()
    {
        pauseBtn.gameObject.SetActive(false);

        winMenu.gameObject.SetActive(!winMenu.gameObject.activeSelf);
    }

    /*هذه الدالة تقوم باعاده المستوي عند الضغط علي restart button*/
    public void RestartLevel()
    {
        sound.Play();
        Time.timeScale = 1;
        loadingContainer.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }

    /*هذه الداله تقوم بالذهاب الي المشهد الاول (الرئيسي) عند الضغط علي main menu and home buttons*/
    public void ToMainMenu()
    {
        sound.Play();
        Time.timeScale = 1;
        loadingContainer.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(0));
    }

    /*هذه الداله تقوم بالذهاب للمشهد النهائي عند الضغط علي next button*/
    public void NextLevel()
    {
        sound.Play();
        Time.timeScale = 1;
        loadingContainer.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
    }

    /*هذه الداله تستدعي في اي داله تقوم بالتنقل بين المشاهد وتتحكم في الloading progress*/
    IEnumerator LoadAsynchronously(int sceneNum)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNum);
        while (!operation.isDone)
        {
            loadingContainer.GetChild(0).GetComponent<Slider>().value = operation.progress;
            yield return null;
        }
    }

    /*هذه الداله تقوم بالتحكم في الصوت اذا كان mute او لا*/
    public void PauseMenuUI(int index)
    {
        sound.Play();
        if (index == 0)
        {
            soundBtn.sprite = (soundBtn.sprite == soundBtnSprites[1]) ? soundBtnSprites[0] : soundBtnSprites[1];
            int soundValue = (soundBtn.sprite == soundBtnSprites[0]) ? 1 : 0;
            PlayerPrefs.SetInt("Sound", soundValue);
            sound.mute = !sound.mute;
            lava.GetComponent<AudioSource>().mute = !lava.GetComponent<AudioSource>().mute;
            uiCanvas.GetComponent<AudioSource>().mute = !uiCanvas.GetComponent<AudioSource>().mute;
        }
    }
}
