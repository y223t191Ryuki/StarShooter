using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectActivator : MonoBehaviour
{
    public GameObject CreditScreen;
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject BackButton;
    void Start()
    {
        CreditScreen.SetActive(false);
    }

    public void ActivateCreditScreen()
    {
        SeManager.Instance.PlayButtonPushSe();
        CreditScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(BackButton);
    }

    public void DeactivateCreditScreen()
    {
        SeManager.Instance.PlayButtonPushSe();
        CreditScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(StartButton);
    }
}
