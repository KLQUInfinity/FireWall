using UnityEngine;
using System.Collections;

/*هذا الclass خاص بحركة الحصان بالعربة في path والتحكم فيها متي يقف ومتي يتحرك والتحكم في الanimations الخاصه بالحصان*/
public class Horse_Movement : MonoBehaviour
{
    /*هذا المتغير يتحكم في الخواص الفزيائيه للحصان*/
    private Rigidbody myRB;

    /*هذا المتغير للتحكم في الanimation الخاصه بالحصان*/
    private Animator myAnim;

    /*هذا المتغير يحمل نقطين يتتبعهم الحصان في مساره عند الحركه*/
    private float[] targetZPoint = { -8.3f, 7.06f };

    /*هذا المتغير يحمل اسم متغيران يتحكمان في الanimation*/
    private string[] animStr = { "isRun", "isWalk" };

    /*هذا المتغير يحمل سرعتان سرعه دخول امام القلعة وسرعة دخول القلعة*/
    private float[] speed = { 10f, 5f };

    /*هذا المتغير للاختيار في المتغيرات التي تحمل شياءن*/
    public int index;

    /*هذا المتغير للتحكم هل يتحرك الحصان اول لا*/
    public bool check;

    /*هذا المتغير لحمل محتويات العربه*/
    public Sprite data;

    /*هذه الداله تنادي عند بداء الclass من خلال البرنامج لاكن بعد داله Awake*/
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAnim = transform.GetChild(1).GetComponent<Animator>();
    }


    /*هذه الدالة تنادي من البرنامج تلقائيا باستمرار كل عدد من الframes غير ثابت
    ويتم تحريك الحصان من خلالها*/
    void Update()
    {
        if (!check)
        {
            myRB.isKinematic = false;
        }
        else if (check)
        {
            if (transform.position.z != targetZPoint[index])
            {
                myAnim.SetBool(animStr[index], true);

                Vector3 targetPos = new Vector3(0, 2.57f, targetZPoint[index]);
                Vector3 pos = Vector3.MoveTowards(transform.position, targetPos, speed[index] * Time.deltaTime);
                myRB.MovePosition(pos);
                if (transform.position.z >= targetZPoint[index] && index == 0)
                {
                    myAnim.SetBool(animStr[index], false);
                    myRB.isKinematic = true;
                    check = false;
                    index = (index + 1) % targetZPoint.Length;
                }
            }
            else if (index == 0)
            {
                myAnim.SetBool(animStr[index], false);
                myRB.isKinematic = true;
                check = false;
                index = (index + 1) % targetZPoint.Length;
            }
        } 
    }
}
