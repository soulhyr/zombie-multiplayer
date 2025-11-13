using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    public Image icon;
    public float speed = -400f;
    
    void Update() => icon.transform.Rotate(0, 0, speed * Time.deltaTime);
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}