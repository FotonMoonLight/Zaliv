using UnityEngine;
using DG.Tweening;

public class DotWeenUI : MonoBehaviour
{
    [SerializeField] private Vector3 _transformTo;
    [SerializeField] private float _transformSpeed;

    [SerializeField] private Vector3 _trasformOrigin;

    public void PanelOrigin() =>
        _trasformOrigin = transform.localPosition;

    public void HidePanle() =>
        transform.DOLocalMove(_transformTo, _transformSpeed);   

    public void ShowPanle() =>
        transform.DOLocalMove(_trasformOrigin, _transformSpeed);
}