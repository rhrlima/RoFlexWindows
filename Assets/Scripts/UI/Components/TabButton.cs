using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler
{
    public TabGroup TabGroup { get; set; }
    private Image background;

    [SerializeField] private Sprite tabActive;
    [SerializeField] private Sprite tabIdle;

    void Start()
    {
        background = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TabGroup.OnTabSelected(this);
        TabGroup.OnTabEnter(this);
    }

    public void SetActive(bool isActive)
    {
        background.sprite = isActive ? tabActive : tabIdle;
    }
}
