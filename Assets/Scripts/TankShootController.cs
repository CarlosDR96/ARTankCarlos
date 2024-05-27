using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TankShootController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button shootButton;
    public GameObject bulletPrefab;
    public Transform canonEnd;
    public Transform tankTower;
    public Transform tankCanon;
    public float maxBulletForce = 500f;
    public AudioClip shootSound; // Asigna el clip de audio desde el Inspector
    private float holdTime = 0f;
    private bool isHolding = false;
    private AudioSource audioSource;

    public delegate void ShootAction();
    public event ShootAction OnShoot;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        EventTrigger trigger = shootButton.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        pointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(pointerUp);
    }

    void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!enabled) return; // Asegúrate de que el script esté habilitado
        Debug.Log(gameObject.name + " OnPointerDown");
        isHolding = true;
        holdTime = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!enabled) return; // Asegúrate de que el script esté habilitado
        Debug.Log(gameObject.name + " OnPointerUp");
        isHolding = false;
        float bulletForce = Mathf.Clamp(holdTime, 0, maxBulletForce);
        ShootBullet(bulletForce);
        OnShoot?.Invoke();
    }

    private void ShootBullet(float force)
    {
        Debug.Log(gameObject.name + " Shooting with force: " + force);
        GameObject bullet = Instantiate(bulletPrefab, canonEnd.position, canonEnd.rotation);
        Vector3 shootDirection = tankCanon.forward;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(shootDirection * force, ForceMode.Impulse);
        }

        // Reproducir el sonido de disparo
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        Destroy(bullet, 5f);
    }
}
