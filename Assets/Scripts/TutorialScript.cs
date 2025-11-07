using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using UnityEngine.SceneManagement;
public class TutorialScript : MonoBehaviour
{
    [Header("Refrences")]
    PlayerAttack attack;



    public TMP_Text tutorialtext;
    public Button idk;
    public int textnumber;
    public GameObject enemie;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        tutorialtext.text = "Welcom to (insert game name hear";
        textnumber = 0;
        idk.onClick.AddListener(numberup);
    }

    // Update is called once per frame
    void Update()
    {
        if(textnumber == 1)
        {
            tutorialtext.text = "Use WASD to move, space to dash and look around by moving your mouse, but be wary of what lurkes in the dark.";


        }
        if (textnumber == 2)
        {
            tutorialtext.text = "Press your left mouse button to swing your sword.";


        }
        if (textnumber == 3)
        {

            tutorialtext.text = "Or if youd rather shoot a gun thas fine to";

        }
        if (textnumber == 4) 
        {
            tutorialtext.text = "Along the course of the game you may face a few enemies. like this one. ";
            enemie.SetActive(true);

        }
        if (textnumber == 5)
        {
            tutorialtext.text = "Well thats all im telling you, good luck. ";

        }
        if(textnumber == 6)
        {
            SceneManager.LoadScene("Maine Scean");

        }
        
    }
    void numberup()
    {
        textnumber += 1;
    }
}
