using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoatMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float speed;
    private Vector2 movementDir;
    public float maxBoatFuel;
    public float boatFuel;
    [SerializeField] private float fuelPerMove;

    [Header("Referenced Objects")]
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private GameObject outOfFuelPopup;

    // PRIVATE VARIABLES
    private Rigidbody2D rb;
    private Transform myCamera;
    private SpriteRenderer spriteRenderer;
    private Sea_GameManager gameManager;

    void Awake()
    {
        gameManager = FindFirstObjectByType<Sea_GameManager>();
        boatFuel = maxBoatFuel;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (boatFuel > 0)
        {
            movementDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                SwitchRotation();
                boatFuel -= fuelPerMove * Time.deltaTime;
            }
        }
        else { if (!outOfFuelPopup.activeSelf) { outOfFuelPopup.SetActive(true); } }
        fuelText.text = boatFuel.ToString("F1") + "%";
        fuelSlider.value = boatFuel;

        if (boatFuel < 0) { boatFuel = 0; }
    }

    private void FixedUpdate()
    {
        if (boatFuel > 0) { rb.linearVelocity = movementDir.normalized * speed; }
    }

    void SwitchRotation()
    {
        if (movementDir.x > 0)
        {
            Debug.Log("D");
            transform.rotation = Quaternion.identity;
        }
        else if (movementDir.x < 0)
        {
            Debug.Log("A");
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (movementDir.y > 0)
        {
            Debug.Log("W");
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (movementDir.y < 0)
        {
            Debug.Log("S");
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Seal")
        {
            Debug.Log("Entered seal trigger");
            collision.GetComponentInParent<Seal_SeaBehaviour>().BoatTriggered();
        }
    }
}
