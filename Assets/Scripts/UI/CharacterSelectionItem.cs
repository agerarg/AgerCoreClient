using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharacterSelectionItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    private int id;
    public void Setup(int id, string namer, string level)
    {
        nameText.SetText(namer);
        levelText.SetText(level);
        this.id = id;
    }

    public void Select()
    {
        NetworkEvents.LoadCharacter?.Invoke(id);
    }
}
