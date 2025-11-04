using UnityEngine;
using UnityEngine.EventSystems;

public class button_script : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource hover_button_sound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("why");
        hover_button_sound.Play();

    }


}
