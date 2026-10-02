using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction moveAction;
    InputAction scaleAction;
    InputAction rotateAction;
    InputAction deleteAction;
    InputAction scale2Action;
    InputAction rotate2Action;

    [SerializeField] float moveSpeed;
    [SerializeField] float scaleSpeed;
    [SerializeField] float rotateSpeed;
    private bool toggle;
    Rigidbody2D player;
    [SerializeField] GameObject obj;
    [SerializeField] GameObject obj2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        scaleAction = InputSystem.actions.FindAction("Scale");
        rotateAction = InputSystem.actions.FindAction("Rotate");
        scale2Action = InputSystem.actions.FindAction("Scale2");
        rotate2Action = InputSystem.actions.FindAction("Rotate2");
        deleteAction = InputSystem.actions.FindAction("Delete");
        toggle = true;
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveValue = moveAction.ReadValue<Vector2>();
        player.transform.position += moveValue * moveSpeed;

        float scaleValue = scaleAction.ReadValue<float>();
        player.transform.localScale += scaleValue * scaleSpeed * Vector3.one;

        float rotateValue = rotateAction.ReadValue<float>();
        player.transform.Rotate(0, 0, rotateValue * rotateSpeed);

        if (deleteAction.IsPressed())
        {
            if (toggle)
            {
                obj.SetActive(false);
            }
            if (!toggle)
            {
                obj.SetActive(true);
            }
            toggle = !toggle;
        }

        float scale2Value = scale2Action.ReadValue<float>();
        obj2.transform.localScale += scale2Value * scaleSpeed * Vector3.one;

        float rotate2Value = rotate2Action.ReadValue<float>();
        obj2.transform.Rotate(0, 0, rotate2Value * rotateSpeed);
    }
}
