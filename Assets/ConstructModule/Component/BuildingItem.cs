﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class BuildingItem : MonoBehaviour
{
    public QuadInfo quadInfo;
    private BoxCollider _boxCollider;
    private RaycastHit[] hits;
    private Vector3 colliderScale;
    public UnityAction<QuadInfo> onPositionChanged;
    private void OnEnable()
    {
        quadInfo = new global::QuadInfo();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void UpdateBuilding(Vector3 newPos)
    {
        transform.position = newPos;
        colliderScale = BuildingUtility.ScaleAndCubeScale(transform.localScale, _boxCollider.size);
        quadInfo.quad = UpdateQuad(transform.position,colliderScale.x * 1.2f,colliderScale.z * 1.2f);
        JudePosition();
    }
    private static Vector3[] UpdateQuad(Vector3 center,float wigth,float length)
    {
        var quad = new Vector3[4];
        quad[0] = center + new Vector3(-wigth * 0.5f,0.01f, length * 0.5f );
        quad[1] = center + new Vector3(-wigth * 0.5f, 0.01f, -length * 0.5f );
        quad[2] = center + new Vector3(wigth * 0.5f, 0.01f, -length * 0.5f);
        quad[3] = center + new Vector3(wigth * 0.5f, 0.01f, length * 0.5f);
        return quad;
    }
    private void JudePosition()
    {
        hits = Physics.BoxCastAll(
            _boxCollider.center + transform.position,
            colliderScale * 0.5f * 1.2f,
            Vector3.up,
            transform.rotation,
            10,
            LayerMask.GetMask(BuildingUtility.MoveItemLayerName));

        quadInfo.installAble = true;

        foreach (var item in hits)
        {
            if (item.collider.gameObject != gameObject)
            {
                quadInfo.installAble = false;
            }
        }
    }
}

