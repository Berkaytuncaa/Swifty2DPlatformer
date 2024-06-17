using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChSwipeControler : MonoBehaviour, IEndDragHandler
{
    private int _currentPage;
    private Vector3 _targetPos;
    private float _dragThreshould;

    [SerializeField] private int maxPage;
    [SerializeField] private Vector3 pageStep;
    [SerializeField] private RectTransform levelPagesRect;
    [SerializeField] private float tweenTime;
    [SerializeField] private LeanTweenType tweenType;
    [SerializeField] private Button previousButton, nextButton;

    private void Awake()
    {
        _currentPage = 1;
        _targetPos = levelPagesRect.localPosition;
        _dragThreshould = Screen.width / 15;
        UpdateArrowButton();
    }

    public void Next()
    {
        if (_currentPage < maxPage)
        {
            _currentPage++;
            _targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (_currentPage > 1)
        {
            _currentPage--;
            _targetPos -= pageStep;
            MovePage();
        }
    }

    private void MovePage()
    {
        levelPagesRect.LeanMoveLocal(_targetPos, tweenTime).setEase(tweenType);
        UpdateArrowButton();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > _dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
            MovePage();
        }
    }

    private void UpdateArrowButton()
    {
        nextButton.interactable = true;
        previousButton.interactable = true;

        if (_currentPage == 1) previousButton.interactable = false;
        else if (_currentPage == maxPage) nextButton.interactable = false;
    }
}
