using UnityEngine;
using TMPro;

public class BoatMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float speed;
    private Vector2 movementDir;
    public float boatFuel;
    [SerializeField] private float fuelPerMove;

    [Header("Referenced Objects")]
    [SerializeField] private TextMeshProUGUI fuelText;

    // PRIVATE VARIABLES
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (boatFuel > 0)
        {
            movementDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                boatFuel -= fuelPerMove * Time.deltaTime;
            }
        }
        else { }
        fuelText.text = "Fuel: " + boatFuel.ToString("F1") + "%";
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementDir.normalized * speed;
    }
}
