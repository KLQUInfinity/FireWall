using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*هذا الclass الخاص بالtimer والتحكم به بالكامل ومتي يتم انتهاء الوقت ومتي يبدا وهكذا*/
public class Timer : MonoBehaviour
{

    /*هذا المتغير استعمله لادخل الي هذا الملف من اللمفات الاخري وهذه تسمي singlton design pattern*/
    private static Timer instance;

    /* من هنا  انادي المتغير الذي اعلاه*/
    public static Timer Instance{ get { return instance; } }

    /*هذا المتغير يحمل قيمة الوقت لاتخاذ الاختيار*/
    public float timerSecond;

    /*هذا المتغير للتاكد من انتهاء الوقت*/
    private bool finish;

    /*هذه الدالة تنادي من البرنامج تلقائيا قبل كل الدوال*/
    void Awake()
    {
        instance = this;
        finish = false;
    }

    /*هذه الدالة تنادي من البرنامج تلقائيا باستمرار كل عدد ثابت من الframes ويتم فيها العدد التنازل للtimer*/
    void FixedUpdate()
    {
        if (timerSecond > 0 && !finish)
        {
            timerSecond -= Time.deltaTime;
            GetComponent<Text>().text = ((int)timerSecond).ToString();
        }
        else if (timerSecond <= 0)
        {
            finish = true;
            GetComponent<Text>().text = "Time Over";
            LevelManager.Instance.timeOver = true;

            timerSecond = 11;
            StartCoroutine(WaitSeconds());
        }
    }

    IEnumerator WaitSeconds()
    {
        UI_Manager_Levels.Instance.pauseBtn.interactable = false;
        yield return new WaitForSeconds(3f);
        UI_Manager_Levels.Instance.pauseBtn.interactable = true;
        finish = false;
        gameObject.SetActive(false);
    }
}
