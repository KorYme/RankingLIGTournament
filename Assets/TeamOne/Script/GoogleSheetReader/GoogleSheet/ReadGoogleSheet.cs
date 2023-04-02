using TeamOne_SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

namespace TeamOne
{
    public class ReadGoogleSheet : MonoBehaviour
    {
        [SerializeField] string link;
        [HideInInspector] public bool isLoading;

        public JSONNode currentNode;

        private void Awake()
        {
            RefreshData();
        }

        public void RefreshData()
        {
            isLoading= true;
            StartCoroutine(ObtainSheetData());
        }

        private IEnumerator ObtainSheetData()
        {
            UnityWebRequest www = UnityWebRequest.Get(link);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError || www.timeout > 2)
            {
                Debug.Log("Error" + www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                currentNode = JSON.Parse(json);
            }
            isLoading= false;
        }
    }
}