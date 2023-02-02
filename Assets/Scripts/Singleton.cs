using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T m_instance = null;

    public static T instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType(typeof(T)) as T;
            }
            if (m_instance == null) {
                GameObject obj = new GameObject(typeof(T).ToString());
                m_instance = obj.AddComponent<T>();
            }
            return m_instance;
        }
    }

    private void OnApplicationQuit() {
        m_instance = null;
    }
}