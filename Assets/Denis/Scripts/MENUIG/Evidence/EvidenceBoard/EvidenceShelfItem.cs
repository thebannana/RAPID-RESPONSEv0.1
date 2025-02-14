using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvidenceShelfItem : MonoBehaviour
{
    public GameObject detailsMenu; // Assign in Inspector
    public TMP_Text evidenceNameText;
    public TMP_Text levelPickedText;
    public TMP_Text descriptionText;
    public Button cancelButton;

    [Header("Manual Input")]
    public string evidenceName;
    public string levelPicked;
    [Multiline]
    public string description;

    void Start()
    {
        // Assign button click listener
        cancelButton.onClick.AddListener(OnCancelClicked);
    }

    public void Initialize(string name, string level, string desc)
    {
        evidenceName = name;
        levelPicked = level;
        description = desc;
    }

    void OnMouseDown()
    {
        detailsMenu.SetActive(true);
        UpdateDetailsText();
    }

    void OnCancelClicked()
    {
        detailsMenu.SetActive(false);
    }

    void UpdateDetailsText()
    {
        evidenceNameText.text = evidenceName;
        levelPickedText.text = levelPicked;
        descriptionText.text = description;
    }
}
