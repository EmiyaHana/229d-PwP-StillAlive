using UnityEngine;

public class IntermissionManager : MonoBehaviour
{
    public GameObject intermissionPanel;
    private WaveManager waveManager;
    private PlayerController player;

    void Start()
    {
        waveManager = Object.FindFirstObjectByType<WaveManager>();
        player = Object.FindFirstObjectByType<PlayerController>();
    }

    public void ShowIntermission()
    {
        intermissionPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SelectUpgradeWeapon()
    {
        player.rangedDamageBonus += 0.2f;

        Debug.Log("Upgrade approve!");
        CloseIntermission();
    }

    public void SelectMaxAmmo()
    {
        player.maxAmmo += 5;
        player.AddAmmo(5);
        Debug.Log("Increase ammo approve!");
        CloseIntermission();
    }

    private void CloseIntermission()
    {
        intermissionPanel.SetActive(false);
        Time.timeScale = 1f;
        waveManager.StartNextWave();
    }
}