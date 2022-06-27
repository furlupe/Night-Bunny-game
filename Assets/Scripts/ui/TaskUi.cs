using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace ui
{
    public class TaskUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI taskName;
        [SerializeField] private TextMeshProUGUI taskDescr;

        public void SetTask(string name, string descr)
        {
            taskName.text = name;
            taskDescr.text = descr;
        }

        public void UpdateTask(string descr)
        {
            taskDescr.text = descr;
        }
    }
}