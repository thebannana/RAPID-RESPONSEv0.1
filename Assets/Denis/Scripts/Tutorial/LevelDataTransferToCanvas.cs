using UnityEngine;
using TMPro;

public class UpdateTextFields : MonoBehaviour
{
    public TextMeshProUGUI incapacitatedUnitsText;
    public TextMeshProUGUI allSuspectsText;
    public TextMeshProUGUI killedSuspectsText;
    public TextMeshProUGUI arrestedSuspectsText;
    public TextMeshProUGUI allCiviliansText;
    public TextMeshProUGUI detainedCiviliansText;
    public TextMeshProUGUI allEvidenceText;
    public TextMeshProUGUI collectedEvidenceText;

    private void Update()
    {
        if (LevelStatus.Instance != null)
        {
            incapacitatedUnitsText.text = "Incapacitated Units: " + LevelStatus.Instance.incapacitatedUnits.Count;
            allSuspectsText.text = "All Suspects: " + LevelStatus.Instance.allSuspects.Count;
            killedSuspectsText.text = "Killed Suspects: " + LevelStatus.Instance.killedSuspects.Count;
            arrestedSuspectsText.text = "Arrested Suspects: " + LevelStatus.Instance.arrestedSuspects.Count;
            allCiviliansText.text = "All Civilians: " + LevelStatus.Instance.allCivilians.Count;
            detainedCiviliansText.text = "Detained Civilians: " + LevelStatus.Instance.detainedCivilians.Count;
            allEvidenceText.text = "All Evidence: " + LevelStatus.Instance.allEvidence.Count;
            collectedEvidenceText.text = "Collected Evidence: " + LevelStatus.Instance.collectedEvidence.Count;
        }
    }
}
