using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch.Factory
{
    public static class Ch3PrefabFactory
    {
        public static GameObject Create(GameObject p_prefab, Vector3 p_position, Quaternion p_rotation, Transform p_parent = null)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(p_prefab, p_position, p_rotation);

            if (p_parent != null)
            {
                instance.transform.SetParent(p_parent, false);
            }

            return instance;
        }

        public static GameObject Create(GameObject p_prefab, Vector3 p_position, Quaternion p_rotation, GameObject p_parent)
        {
            return Create(p_prefab, p_position, p_rotation, p_parent.transform);
        }

        public static GameObject Create(GameObject p_prefab, Vector3 p_position, Quaternion p_rotation, Component p_parent)
        {
            return Create(p_prefab, p_position, p_rotation, p_parent.transform);
        }
    }
}