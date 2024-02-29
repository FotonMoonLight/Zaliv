using UnityEngine;
using TMPro;

public class StatsSpace : MonoBehaviour
{
    public string Table;

    [SerializeField] private TextMeshProUGUI _textBox;

    public void Start()
    {
        if(!_textBox)
        _textBox = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void TextBoxMessage(string msg)
    {
        _textBox.text = msg;
    }
}