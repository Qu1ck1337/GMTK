using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dice : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _diceHubPanel;
    [SerializeField] private AudioClip _pickUpSound;
    [SerializeField] private AudioClip _dropDownSound;
    private AudioSource _audioSource;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public GameObject DiceHubPanel => _diceHubPanel;
    public GameObject DicePanel => _panel;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("BeginDrag");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Ondrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag");
        canvasGroup.blocksRaycasts = true;
        _audioSource.clip = _dropDownSound;
        _audioSource.Play();
        if (transform.parent == _panel.transform)
            ResetPlace();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(_panel.transform);
        _audioSource.clip = _pickUpSound;
        _audioSource.Play();
        //Debug.Log("Click");
    }

    public void ResetPlace()
    {
        transform.SetParent(_diceHubPanel.transform);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
    }
}
