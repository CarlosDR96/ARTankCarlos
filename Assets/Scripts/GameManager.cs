using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerController tank1Controller;
    public PlayerController tank2Controller;
    public TankShootController tank1ShootController;
    public TankShootController tank2ShootController;
    public TankHealth tank1Health;
    public TankHealth tank2Health;
    public TextMeshProUGUI turnText; // Asigna el TextMeshPro desde el Inspector
    public AudioClip victoryMusic; // Asigna el clip de música de victoria desde el Inspector
    private AudioSource audioSource;

    private bool isTank1Turn = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        tank1Health.OnTankDestroyed += HandleTankDestroyed;
        tank2Health.OnTankDestroyed += HandleTankDestroyed;

        StartTurn();
    }

    private void StartTurn()
    {
        if (turnText != null)
        {
            turnText.gameObject.SetActive(true);
        }

        UpdateTurnText();
        Debug.Log("StartTurn: " + (isTank1Turn ? "Tank 1's turn" : "Tank 2's turn"));
        if (isTank1Turn)
        {
            EnableTank(tank1Controller, tank1ShootController);
            DisableTank(tank2Controller, tank2ShootController);
        }
        else
        {
            EnableTank(tank2Controller, tank2ShootController);
            DisableTank(tank1Controller, tank1ShootController);
        }
    }

    private void EnableTank(PlayerController controller, TankShootController shootController)
    {
        Debug.Log("Enabling: " + controller.gameObject.name);
        controller.enabled = true;
        shootController.enabled = true;
        shootController.OnShoot += EndTurn;
    }

    private void DisableTank(PlayerController controller, TankShootController shootController)
    {
        Debug.Log("Disabling: " + controller.gameObject.name);
        controller.enabled = false;
        shootController.enabled = false;
        shootController.OnShoot -= EndTurn;
    }

    private void EndTurn()
    {
        Debug.Log("EndTurn: " + (isTank1Turn ? "Tank 1's turn ended" : "Tank 2's turn ended"));

        if (isTank1Turn)
        {
            Debug.Log("Disabling Tank 1 ShootController");
            DisableTank(tank1Controller, tank1ShootController);
        }
        else
        {
            Debug.Log("Disabling Tank 2 ShootController");
            DisableTank(tank2Controller, tank2ShootController);
        }

        isTank1Turn = !isTank1Turn;
        StartTurn();
    }

    private void UpdateTurnText()
    {
        if (turnText != null)
        {
            turnText.text = isTank1Turn ? "Turno de Tanque 1" : "Turno de Tanque 2";
        }
    }

    private void HandleTankDestroyed(GameObject destroyedTank)
    {
        if (turnText != null)
        {
            string winner = destroyedTank == tank1Controller.gameObject ? "Tanque 2" : "Tanque 1";
            turnText.text = winner + " ha ganado!";
        }

        if (audioSource != null && victoryMusic != null)
        {
            audioSource.PlayOneShot(victoryMusic);
        }

        Invoke("RestartGame", 10f);
    }

    private void RestartGame()
    {
        // Reiniciar la escena actual
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
