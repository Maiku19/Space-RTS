using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SuperTransform2D))]
public class TestScript : MonoBehaviour
{
    [SerializeField] float speed;

    SuperTransform2D st;

    private void Start()
    {
        st = GetComponent<SuperTransform2D>();
    }

    void Update()
    {
        st.Move(speed * Time.deltaTime, 0);
    }
}
