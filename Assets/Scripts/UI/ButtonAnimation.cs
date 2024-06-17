using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    private Button _button;
    private Vector3 _upScale;

    private void Awake()
    {
        _upScale = new Vector3(1.2f, 1.2f, 1);

        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(Anim);
    }

    private void Anim()
    {
        LeanTween.scale(gameObject, _upScale, 0.1f);
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setDelay(0.1f);
    }
}
