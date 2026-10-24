using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Lecture 6/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string displayName = "Crystal";
    [SerializeField] private Color color = Color.cyan;
    [SerializeField] private int value = 1;

    public string DisplayName => displayName;
    public Color Color => color;
    public int Value => value;
}
