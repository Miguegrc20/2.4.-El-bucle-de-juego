using UnityEngine;

public class ControladorAnimacion : MonoBehaviour
{
    private Animator animator;

    private const string PARAM_MOVIENDO = "Moviendo";

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool estaMoviendo = Input.GetKey(KeyCode.RightArrow) ||
                             Input.GetKey(KeyCode.LeftArrow);

        animator.SetBool(PARAM_MOVIENDO, estaMoviendo);
    }
}