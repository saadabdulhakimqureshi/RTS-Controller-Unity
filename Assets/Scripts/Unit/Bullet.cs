using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Bullet Physics")]
    public float bulletSpreadAngle = 1f;
    public float speed;
    public string shooter;
    public Vector3 position;
    public Vector3 direction;
    public float attackDamage;
    public float bulletForce;
    public Rigidbody rb;

    [Header("Bullet Trail")]
    public float AutoDestroyTime = 5f;

    public BulletTrailScriptableObject TrailConfig;
    public TrailRenderer Trail;


    private bool IsDisabling = false;

    protected const string DISABLE_METHOD_NAME = "Disable";
    protected const string DO_DISABLE_METHOD_NAME = "DoDisable";

    protected virtual void OnEnable()
    {

    }

    void Start()
    {
        Trail = GetComponent<TrailRenderer>();
        IsDisabling = false;
        CancelInvoke(DISABLE_METHOD_NAME);
        ConfigureTrail();
        Invoke(DISABLE_METHOD_NAME, AutoDestroyTime);
        rb = GetComponent<Rigidbody>();
    }

    private void ConfigureTrail()
    {
        if (Trail != null && TrailConfig != null)
        {
            TrailConfig.SetupTrail(Trail);
        }
    }

    protected void Disable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        CancelInvoke(DO_DISABLE_METHOD_NAME);
        rb.velocity = Vector3.zero;
        if (Trail != null)
        {
            Trail.enabled = false;
        }


        if (Trail != null && TrailConfig != null)
        {
            IsDisabling = true;
            Invoke(DO_DISABLE_METHOD_NAME, TrailConfig.Time);
        }
        else
        {
            DoDisable();
        }
    }

    protected void DoDisable()
    {
        if (Trail != null && TrailConfig != null)
        {
            Trail.Clear();
        }

        Destroy(gameObject);
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }
    public void Setup(Vector3 shootPosition, Vector3 direction, string shooter, float attackDamage)
    {
        Vector3 spreadDirection = new Vector3(Random.Range(-bulletSpreadAngle, bulletSpreadAngle), Random.Range(-bulletSpreadAngle, bulletSpreadAngle), 0f) + direction;
        transform.LookAt(shootPosition + spreadDirection); // Normalize the direction vector

        this.position = shootPosition + spreadDirection;
        this.direction = spreadDirection;
        this.shooter = shooter;
        this.attackDamage = attackDamage;
    }   


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Bullet hit " + collision.gameObject.name);
        RTSController rtsController = collision.gameObject.GetComponent<RTSController>();
        if (rtsController != null && rtsController.tag != shooter) {
            rtsController.TakeDamage(attackDamage);
        }

        Destroyable destroyable = collision.gameObject.GetComponent<Destroyable>();
        if (destroyable != null) {
            destroyable.LoseHealth(attackDamage);
        }

        BodyPart bodyPart = collision.gameObject.GetComponent<BodyPart>();
        if (bodyPart != null)
        {
            bodyPart.LoseHealth(attackDamage);
        }

        Rigidbody rigidBody = collision.gameObject.GetComponent<Rigidbody>();

        if (rigidBody != null)
        {
            Vector3 forceDirection = transform.forward;
            Vector3 force = (forceDirection * bulletForce);
            rigidBody.AddForce(force, ForceMode.Force);
        }

        if (collision.gameObject.tag != "Bullet")
        {
            Disable();
        }

    }
}
