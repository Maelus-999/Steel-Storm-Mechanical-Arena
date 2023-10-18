using UnityEngine;


[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour{

    [Header("Player Movements Settings")]
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float sensitivity = 3f;
    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Joint options")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 50f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start(){
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
    }

    void Update(){

        // Calculer la vélocité du mouvement du joueur
        float xMov = Input.GetAxisRaw("Horizontal"); // Gauche ou droite
        float zMov = Input.GetAxisRaw("Vertical"); // Avancer ou Arrière

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);

        // Calcule de la rotation du joueur en Vector3

        float yRot = Input.GetAxisRaw("Mouse X");
        
        Vector3 rotation = new Vector3(0, yRot, 0) * sensitivity;

        motor.Rotate(rotation);

        // Calcule de la rotation de la Camera en Vector3

        float xRot = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRot * sensitivity;

        motor.RotateCamera(cameraRotationX);

        Vector3 thrusterVelocity = Vector3.zero;
        if (Input.GetButton("Jump")) {

            thrusterVelocity = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        } else {
            SetJointSettings(jointSpring);
        }

        motor.applyThruster(thrusterVelocity);
    }

    private void SetJointSettings(float _jointSpring) {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }
}
