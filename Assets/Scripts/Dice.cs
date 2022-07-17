using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private int _number;
    [SerializeField]
    private Animator _animator;
    private Sprite _defaultSprite;
    [SerializeField]
    private Sprite[] _sprites;

    public GameObject DiceHubPanel => _diceHubPanel;
    public GameObject DicePanel => _panel;
    private void Awake()
    {
        _defaultSprite = GetComponent<Image>().sprite;
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
        GetComponent<Image>().overrideSprite = null;
        transform.SetParent(_diceHubPanel.transform);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
    }

    public event Action<Dice, int> OnAnimationEnd;

    public void Roll()
    {
        _animator.SetTrigger("ok");
        _number = UnityEngine.Random.Range(1, 7);
    }

    public void OnAnimationEnd_AnimationEvent()
    {
        GetComponent<Image>().overrideSprite = _sprites[_number - 1];
        OnAnimationEnd?.Invoke(this, _number);
    }
}
