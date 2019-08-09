using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*هذا الكلاس مسؤال عن استرجاع الbestScore من الذاكره الخاصه باللعبه ومسئول ايضا عن استمرار تواجد
ال object الذي يرتبط به الclass من اول بدايه اللعبه للنهاية*/
public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;

    public static GameMaster Instance{ get { return instance; } }
    /*هاتان الlists لتخيزين الانجازات والاخطاء لعرضها في اخر مشهد*/
    public List<Sprite> data;
    public List<bool> yourChoice;


    /*هذه الدالة تنادي من البرنامج تلقائيا قبل كل الدوال*/
    private void Awake()
    {
        data = new List<Sprite>();
        yourChoice = new List<bool>();
        /*هنا اتاكد ان الoject الذي يرتبط به هذا الclass l موجود ام لا لو ليس موجود متدمروش وخليه هو الاساسي
        اما اذا كان موجود ويوجد واحد اخر ليس هو قم بتدميره ليبقي واحد فقط*/
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        /*هنا اتاكد ان هل لعبة اللعبه من قبل او لا لاقوم بانشاء متغير الbestScore في الذاكره*/
        if (!PlayerPrefs.HasKey("bestScore"))
        {
            PlayerPrefs.SetInt("bestScore", 0);
        }
    }
}
