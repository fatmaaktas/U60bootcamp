﻿using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    private void Awake()
    {
        // Checkpoint'i yalnızca oyuncu katmanıyla etkileşime girecek şekilde Checkpoint katmanına ayarlıyoruz.
        gameObject.layer = LayerMask.NameToLayer("Checkpoint");
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller == null)
            return;

        controller.SetCheckpoint(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue * 0.75f;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}