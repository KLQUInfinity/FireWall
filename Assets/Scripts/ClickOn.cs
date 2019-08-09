using UnityEngine;

/*هذا الclass لغرض واحد وهو الضغط علي صورة التعليمات اثناء بداية الاختيار لتظهر التعليمات امام الشاشه
وهذا الclass مرتبطه بالobject الخاص بصوره التعليمات امام باب القلعة*/
public class ClickOn : MonoBehaviour
{
    /*هذه الداله تستدعي عند الضغط علي الobject المرتبطه به هذه الclass*/
    void OnMouseDown()
    {
        if (LevelManager.Instance.startCheck)
        {
            LevelManager.Instance.Hint();
        }
    }
}
