using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDBase.UI.Custom {

    [RequireComponent(typeof(Image))]
    public class HPBar : MonoBehaviour {

        float playerHP = 0f;
        Image hpBar = null;

        void Start() {

            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            playerHP = obj.GetComponent<Player>().MyStats.CurHP;

            hpBar = GetComponent<Image>();
            hpBar.fillAmount = playerHP / 100f;
        }

        public void Update()
        {
            SetHPUI();   
        }

        void SetHPUI()
        {
            hpBar.fillAmount = playerHP / 100f;
        }
    }
}
