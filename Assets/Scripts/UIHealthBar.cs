using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour{
    public static UIHealthBar Instance{ get; private set; }

    public Image mask;
    private float _originalSize;

    private void Awake(){
        Instance = this;
    }

    private void Start(){
        _originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value){
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _originalSize * value);
    }
}