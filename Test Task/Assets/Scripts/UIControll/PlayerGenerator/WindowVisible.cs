using UnityEngine;

public class WindowVisible : MonoBehaviour
{
    [SerializeField] private DotWeenUI[] _dotWeen;

    [SerializeField] private Canvas[] _otherCanvas;

    public void SetVisible(int index)
    {
        for (int j = 0; j < _dotWeen.Length; j++)
        {
            _dotWeen[j].PanelOrigin();
        }

        for (int i = 0; i < _otherCanvas.Length; i++)
        {
            _otherCanvas[i].enabled = false;
        }

        _otherCanvas[index].enabled = true;
    }
}