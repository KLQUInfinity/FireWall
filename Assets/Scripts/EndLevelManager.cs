using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*هذا الclass تتحكم في كل شي في المشهد الاخير والذي يكون فيه ما انجزته وما اخفقت به*/
public class EndLevelManager : MonoBehaviour
{
    /*هذان المتغيران واحد الذي يحمل صور المحتويات والثاني صور المحتويات التي يتم توليدها بداخل الاول*/
    public GameObject lowerImgsContainer, lowerImg;
	
    /*هذه المتغيرات تحمل قيم ال
    bestscore, 
    score and
    هل كان اختيارك صحيح ام خاطئ*/
    public Text bestScoreTxt, scoreTxt, yourChoiceTxt;
	
    /*هذان المتغيران للbutton الخاصه بالتقليب بين صور المحتويات*/
    public Button nextBtn, previousBtn;
	
    /*هذا المتغير للتحكم في الصوت*/
    private AudioSource sound;
	
    /*هذا المتغير للتنقل بين صور المحتويات التي لعبها*/
    private int showIndex;
	
    /*هذا المتغير يحمل الloading progress bar*/
    public Transform loadingContainer;

    /*هذه الدالة تنادي من البرنامج تلقائيا قبل كل الدوال واقوم فيها باسترجاع بعض القيم من الذاكره وانشاء كل المحتويات التي لعبتها وهل اخطائت فيها ام انجزتها*/
    void Awake()
    {
        showIndex = 0;
        sound = GetComponent<AudioSource>();
        bestScoreTxt.text = "Best Score: " + PlayerPrefs.GetInt("bestScore");
        scoreTxt.text = "Score: " + PlayerPrefs.GetInt("score");
        for (int i = 0; i < GameMaster.Instance.data.Count; i++)
        {
            GameObject temp = Instantiate(lowerImg)as GameObject;
            temp.GetComponent<Image>().sprite = GameMaster.Instance.data[i];
            temp.transform.SetParent(lowerImgsContainer.transform, false);
            temp.SetActive(false);
            if (i == 0)
            {
                temp.SetActive(true);
                yourChoiceTxt.text = (GameMaster.Instance.yourChoice[i]) ? "Success" : "Failed";
                yourChoiceTxt.color = (GameMaster.Instance.yourChoice[i]) ? Color.green : Color.red;
            }
        }
    }

    /*هذه الداله تستخدم عند التنقل وتتحكم في الloading progress bar*/
    IEnumerator LoadAsynchronously(int sceneNum)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNum);
        while (!operation.isDone)
        {
            loadingContainer.GetChild(0).GetComponent<Slider>().value = operation.progress;
            yield return null;
        }
    }

    /*هذه الداله تستدعي عند الضغط علي الmain menu button*/
    public void ToMainMenu()
    {
        sound.Play();
        Time.timeScale = 1;
        loadingContainer.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(0));
    }

    /*هذه الداله تنادي عند الضغط علي next and previous buttons للتنقل بين المحتويات التي لعبتها*/
    public void EndMenu(int index)
    {
        sound.Play();
        switch (index)
        {
            case 0:
                showLevelGrid(++showIndex);
                break;
            case 1:
                showLevelGrid(--showIndex);
                break;
        }
    }

    /*هذه الداله تنادي عن طريق الداله التي فوقها وتستخدم للتنقل بين المحتويات*/
    void showLevelGrid(int index)
    {
        for (int i = 0; i < lowerImgsContainer.transform.childCount; i++)
        {
            if (i == index)
            {
                lowerImgsContainer.transform.GetChild(i).gameObject.SetActive(true);
                yourChoiceTxt.text = (GameMaster.Instance.yourChoice[i]) ? "Success" : "Failed";
                yourChoiceTxt.color = (GameMaster.Instance.yourChoice[i]) ? Color.green : Color.red;
                continue;
            }
            lowerImgsContainer.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (index == 0)
        {
            nextBtn.interactable = true; //nextBtn
            previousBtn.interactable = false;//previousBtn
        }
        else if (index > 0 && index < lowerImgsContainer.transform.childCount - 1)
        {
            nextBtn.interactable = true; //nextBtn
            previousBtn.interactable = true;//previousBtn
        }
        else if (index == lowerImgsContainer.transform.childCount - 1)
        {
            nextBtn.interactable = false; //nextBtn
            previousBtn.interactable = true;//previousBtn
        }
    }
}
