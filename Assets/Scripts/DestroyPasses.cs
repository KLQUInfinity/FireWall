using UnityEngine;
using System.Collections;

/*هذا الclass لها عمل واحد فقط وهو تدمير العربه بعد سقوطها في الحمم البركانية او بعد دخولها القلعة واصدار صوت المكسب او الخساره
وتوليد موثرات تناثر الحمم البركانية عند سقوط العربه بها*/
public class DestroyPasses : MonoBehaviour
{
    public float delay;
    public GameObject lavaExplosion;
    public AudioClip[] gameSounds;

    /*هذه الداله تسدعي عند عبور وخروج اي collider بالcollider المرتبط بالobject المرتبطه به نفس الclass*/
    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Horse"))
        {
            LevelManager.Instance.castleDoor.SetActive(true);
            LevelManager.Instance.startNew = true;

            GameObject g = Instantiate(lavaExplosion, other.transform.position, lavaExplosion.transform.localRotation)as GameObject;
            g.GetComponent<AudioSource>().clip = gameSounds[LevelManager.Instance.soundIndex];
            if (!UI_Manager_Levels.Instance.gameOver)
            {
                g.GetComponent<AudioSource>().mute = (PlayerPrefs.GetInt("Sound") == 1) ? false : true;
                g.GetComponent<AudioSource>().Play();
            }
            Destroy(other.gameObject, delay);
            Destroy(g, delay);
        }
    }
}
