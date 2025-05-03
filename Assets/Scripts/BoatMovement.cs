using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class BoatMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float speed;
    public float rotationSpeed;
    public float maxBoatFuel;
    public float boatFuel;
    [SerializeField] private float fuelPerMove;

    [Header("Referenced Objects")]
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private GameObject outOfFuelPopup;
    private Sea_GameManager gameManager;

    // PRIVATE VARIABLES
    private Rigidbody2D rb;

    void Awake()
    {
        boatFuel = maxBoatFuel;
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<Sea_GameManager>();
    }

    void Update()
    {
        if (!gameManager.paused)
        {
            if (boatFuel > 0)
            {
                if (Input.GetAxisRaw("Vertical") != 0)
                {
                    boatFuel -= fuelPerMove * Time.deltaTime;
                }
            }
            else { if (!outOfFuelPopup.activeSelf) { outOfFuelPopup.SetActive(true); } }
            fuelText.text = boatFuel.ToString("F1") + "%";
            fuelSlider.value = boatFuel;

            if (boatFuel < 0) { boatFuel = 0; }
        }
    }

    private void FixedUpdate()
    {
        if (!gameManager.paused)
        {
            if (boatFuel > 0)
            {
                SetPlayerVelocity();
                SetPlayerRotation();
            }
        }
    }

    private void SetPlayerVelocity()
    {
        float movementInput = Input.GetAxisRaw("Vertical");
        transform.position += transform.right * movementInput * speed * Time.deltaTime;
    }

    private void SetPlayerRotation()
    {
        float rotationInput = Input.GetAxisRaw("Horizontal");
        transform.Rotate(Vector3.forward, -rotationInput * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Seal")
        {
            Debug.Log("Entered seal trigger");
            collision.GetComponentInParent<Seal_SeaBehaviour>().BoatTriggered();
        }
        if (collision.tag == "Trash")
        {
            Debug.Log("Entered trash trigger");
            collision.GetComponent<TrashScript>().BoatTriggered();
        }
    }
}
