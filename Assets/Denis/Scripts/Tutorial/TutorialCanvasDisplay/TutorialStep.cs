using UnityEngine;

public class TutorialStep : MonoBehaviour
{
    public Canvas tutorialCanvas;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PoliceOfficer"))
        {
            FindObjectOfType<TutorialManager>().TriggerTutorialStep(this);
        }
    }

    public void DeactivateStep()
    {
        gameObject.SetActive(false);
    }
}
