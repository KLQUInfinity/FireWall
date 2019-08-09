using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*هذا الclass يتحكم في اللعبه في المشهد التاني فقط من حيث توليد الاحصنة ووضع المحتويات العشوائيه ومتي تخسر ومتي تكسب*/
public class LevelManager : MonoBehaviour
{

    /*هذا المتغير استعمله لادخل الي هذا الملف من اللمفات الاخري وهذه تسمي singlton design pattern*/
    private static LevelManager instance;

    /* من هنا  انادي المتغير الذي اعلاه*/
    public static LevelManager Instance{ get { return instance; } }

    /*هذه المتغيرات خاصه باشياء في داخل المشهد
    مثل الtimer
    وباب القلعه والطريق القلعه
    والحصان بالعربه الي اقوم بتوليد نسخه منه
    وحاملين الUI المختلفين*/
    public GameObject cartHorse, decisionUI, castleRoad, castleDoor, startCanvas, timer, uiCanvas;

    /*هذا المتغير يحمل العربات المتنتظره في الطابور*/
    private Queue<GameObject> waitingLine;

    /*هذا المتغير يحمل صور المحتويات المتاحه كلها والتي اقوم بالاختيار منها عشوائي عند توليد عربه جديده*/
    public Sprite[] dataSprites;

    /*هذه المتغيرات خاصه بعمل check متي يبداء لعبه جديده ومتي يبداء الاختيار ومتي الوقت ينتهي*/
    public bool startNew, startCheck, timeOver;

    /*هذا المتغير لاقوم بالاختيار عشوائي بين اصوات النجاح المختلفه*/
    public int soundIndex;

    /*هذا المتغير يحمل الhealth*/
    public Slider healthSlider;

    /*هذان المتغيران يحملان قيم الscore والbestscore*/
    public Text scoreText, bestScoreText;

    /*هذا المتغير المختص بالobject الذي اقوم بتوليد نسخه منه لانشاء الالعاب الناريه*/
    public GameObject fireWorks;

    /*هذا المتغير المختص بالobject الذي اقوم بتوليد نسخه منه لانشاء الانفجرات*/
    public GameObject explosions;

    /*هذا متغير للتحكم في الصوت المرتبط بالobject الي يرتبط به هذا الclass*/
    private AudioSource sound;


    /*هذه الدالة تنادي من البرنامج تلقائيا قبل كل الدوال واستحضر فيها قيمة الbestscore المخزنه في الذاكره وانشاء نسختين من الاحصنه واقوم باضافتهم الي الqueue
    وتحضير بعد الاعدادات*/
    void Awake()
    {
        instance = this;

        healthSlider.value = 3;
        if (PlayerPrefs.GetInt("bestScore") == 0)
        {
            bestScoreText.gameObject.SetActive(false);
        }
        bestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("bestScore");
        sound = GetComponent<AudioSource>();
        startNew = true;
        startCheck = false;
        timeOver = false;
        waitingLine = new Queue<GameObject>();
        for (int i = 1; i >= 0; i--)
        {
            GameObject temp = Instantiate(cartHorse, new Vector3(0f, 2.57f, (float)(-16.6f / (i + 1))), cartHorse.transform.rotation)as GameObject;
            temp.GetComponent<Horse_Movement>().index = i;
            temp.GetComponent<Horse_Movement>().data = dataSprites[Random.Range(0, dataSprites.Length)];
            waitingLine.Enqueue(temp);
        }
        decisionUI.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = waitingLine.Peek().GetComponent<Horse_Movement>().data;
    }


    /*هذه الدالة تنادي من البرنامج تلقائيا باستمرار كل عدد من الframes غير ثابت*/
    void Update()
    {
        /*اذا انتهي الوقت قبل ان تختار تخسر health ويتم تغير محتويات العربه*/
        if (timeOver)
        {
            timeOver = false;
            decisionUI.SetActive(false);
            healthSlider.value--;
            if (healthSlider.value != 0)
            {
                uiCanvas.GetComponent<AudioSource>().Play();
                GameObject[] g = new GameObject[2];
                g[0] = waitingLine.Dequeue();
                g[1] = waitingLine.Dequeue();
                g[0].GetComponent<Horse_Movement>().data = g[1].GetComponent<Horse_Movement>().data;
                decisionUI.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = g[0].GetComponent<Horse_Movement>().data;
                g[1].GetComponent<Horse_Movement>().data = dataSprites[Random.Range(0, dataSprites.Length)];
                waitingLine.Enqueue(g[0]);
                waitingLine.Enqueue(g[1]);
                if (!decisionUI.transform.GetChild(0).gameObject.activeSelf)
                {
                    decisionUI.transform.GetChild(0).gameObject.SetActive(!decisionUI.transform.GetChild(0).gameObject.activeSelf);//allowBtn
                    decisionUI.transform.GetChild(1).gameObject.SetActive(!decisionUI.transform.GetChild(1).gameObject.activeSelf);//dropBtn
                    decisionUI.transform.GetChild(2).gameObject.SetActive(!decisionUI.transform.GetChild(2).gameObject.activeSelf);//hintImg
                    decisionUI.transform.GetChild(3).gameObject.SetActive(!decisionUI.transform.GetChild(3).gameObject.activeSelf);//DataPanel
                }
                startCheck = false;
                startNew = true;
                startCanvas.SetActive(true);
            }
        }
    }

    /*هذه الداله تعمل عند الضغط علي open the goods لتبداء الاختيار*/
    public void StartBtn()
    {
        sound.Play();
        startNew = false;
        startCheck = true;
        startCanvas.SetActive(false);
        decisionUI.SetActive(true);
        timer.SetActive(true);
    }

    /*هذه الداله تستخدم لادخال عربه في الطابور*/
    IEnumerator LineEnqueue()
    {
        yield return new WaitForSeconds(1f);
        GameObject temp = Instantiate(cartHorse, new Vector3(0f, 2.57f, -16.6f), cartHorse.transform.rotation)as GameObject;
        temp.GetComponent<Horse_Movement>().data = dataSprites[Random.Range(0, dataSprites.Length)];
        temp.GetComponent<Horse_Movement>().index = 0;
        waitingLine.Enqueue(temp);
        castleRoad.GetComponent<Animator>().SetBool("isClose", false);
    }

    /*هذه الداله تستخدم في لاخراج عربه من الطابور*/
    IEnumerator LineDequeue(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject temp = waitingLine.Dequeue();
        temp.GetComponent<Horse_Movement>().check = true;
        waitingLine.Peek().GetComponent<Horse_Movement>().check = true;
        decisionUI.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = waitingLine.Peek().GetComponent<Horse_Movement>().data;
        StartCoroutine(LineEnqueue());
    }

    /*هذه الداله تستدعي عند الضغط علي allow button*/
    public void Allow()
    {
        timer.GetComponent<Timer>().timerSecond = 11;
        timer.SetActive(false);

        sound.Play();
        startCheck = false;
        castleDoor.SetActive(false);
        CheckData("Allow");
        StartCoroutine(LineDequeue(0f));
    }

    /*هذه الداله تستدعي عند الضغط علي drop button*/
    public void Drop()
    {
        timer.GetComponent<Timer>().timerSecond = 11;
        timer.SetActive(false);

        sound.Play();
        startCheck = false;
        castleRoad.GetComponent<Animator>().SetBool("isClose", true);
        CheckData("Drop");
        StartCoroutine(LineDequeue(2f));
    }

    /*هذه الداله تنادي بعد الاختيار للتاكد هل اختيار صح ام خطاء*/
    void CheckData(string yourChoice)
    {
        //check data
        bool check = false;
        int x = -1;
        GameMaster.Instance.data.Add(waitingLine.Peek().GetComponent<Horse_Movement>().data);
        if (waitingLine.Peek().GetComponent<Horse_Movement>().data.name.Contains("Allow"))
        {
            if (yourChoice.Equals("Allow"))
            {//win
                check = true;
                x = 0;
                soundIndex = Random.Range(0, 2);
            }
            else if (yourChoice.Equals("Drop"))
            {//lose
                check = false;
                soundIndex = 2;
            }
        }
        else if (waitingLine.Peek().GetComponent<Horse_Movement>().data.name.Contains("Drop"))
        {
            if (yourChoice.Equals("Allow"))
            {//lose
                x = 1;
                check = false;
                soundIndex = 2;
            }
            else if (yourChoice.Equals("Drop"))
            {//win
                check = true;
                soundIndex = Random.Range(0, 2);
            }
        }

        GameMaster.Instance.yourChoice.Add(check);

        if (healthSlider.value == 1 && !check)
        {
            UI_Manager_Levels.Instance.gameOver = true;
        }

        //close the decision ui
        decisionUI.SetActive(false);
        StartCoroutine(WaitUntilEnd_AndStartAgain(check, x));
    }

    /*هذه الداله تستدعي وتنتظر حتي يدخل الحصان القلعه او يقع في الحمم البركانية لتنفذ المكافاءه او العقوبه علي حسب اختيارك*/
    IEnumerator WaitUntilEnd_AndStartAgain(bool check, int x)
    {
        yield return new WaitUntil(() => startNew == true);
        if (check)
        {
            scoreText.text = (int.Parse(scoreText.text) + 10).ToString();
            if (x == 0)
            {
                GameObject g1 = Instantiate(fireWorks, new Vector3(fireWorks.transform.position.x, fireWorks.transform.position.y, -1.94f), fireWorks.transform.rotation)as GameObject;
                GameObject g2 = Instantiate(fireWorks, new Vector3(fireWorks.transform.position.x, fireWorks.transform.position.y, -8.86f), fireWorks.transform.rotation)as GameObject;
                Destroy(g1, 2f);
                Destroy(g2, 2f);
            }
        }
        else if (!check)
        {
            if (x == 1)
            {
                GameObject g1 = Instantiate(explosions, explosions.transform.position, explosions.transform.rotation)as GameObject;
                Destroy(g1, 2f);
            }
            healthSlider.value--;
        }

        if (healthSlider.value != 0)
        {
            startCanvas.SetActive(true);
        }
    }

    /*هذه الداله تستدعي عند الضغط علي hint button او الضغط علي صوره المعتيات امام باب القلعه*/
    public void Hint()
    {
        sound.Play();
        decisionUI.transform.GetChild(0).gameObject.SetActive(!decisionUI.transform.GetChild(0).gameObject.activeSelf);//allowBtn
        decisionUI.transform.GetChild(1).gameObject.SetActive(!decisionUI.transform.GetChild(1).gameObject.activeSelf);//dropBtn
        decisionUI.transform.GetChild(2).gameObject.SetActive(!decisionUI.transform.GetChild(2).gameObject.activeSelf);//hintImg
        decisionUI.transform.GetChild(3).gameObject.SetActive(!decisionUI.transform.GetChild(3).gameObject.activeSelf);//DataPanel
    }
}
