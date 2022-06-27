using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using NaughtyAttributes;

namespace PrisonControl
{
    public class FoodPlatingUi : MonoBehaviour
    {
        public Action _OnDoneClicked;
        public Action <int>_OnFoodSelected;
        public Action <bool>_OnResponseClicked;

        [SerializeField]
        GameObject panelResponse, requestPopup, responsePopup;

        [SerializeField]
        GameObject pf_foodItem, content, foodPanel, btnDone;

        [Foldout("UI Text")]
        [SerializeField]
        private Text txt_positiveResponse, txt_negetiveResponse;

        //[Foldout("TextMeshPro")]
        //public TextMeshPro txt_popUpFoodRequest, txt_popUpResponse;

        [SerializeField]
        public TypewriterEffect txt_popUpFoodRequest, txt_popUpResponse;

        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        private int currPrisoner;

        private void OnEnable()
        {
            SetUi(0);
        }

        public void SetUi(int curr_prisoner)
        {
            currPrisoner = curr_prisoner;
            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }

            //for (int i = 0; i < _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].list_foodItems.Count; i++)
            for (int i = 0; i < _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetFoodPlatingInfo[curr_prisoner].list_displayFoodItems.Count; i++)
            {
                GameObject obj = Instantiate(pf_foodItem, content.transform);
                obj.transform.Find("img").GetComponent<Image>().sprite = Resources.Load("FoodIcons/" + _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetFoodPlatingInfo[curr_prisoner].list_displayFoodItems[i], typeof(Sprite)) as Sprite;
                obj.transform.Find("index").GetComponent<Text>().text = "" + (int)_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetFoodPlatingInfo[curr_prisoner].list_displayFoodItems[i];

                if(i == 0)
                    obj.transform.GetChild(3).gameObject.SetActive(true);

            }

            Debug.Log("show default");

        }

        public void OnDoneClicked()
        {
            ActivateFoodPanel(false);
            _OnDoneClicked?.Invoke();
        }

        public void ActivateFoodPanel(bool activate)
        {
            btnDone.SetActive(activate);
            foodPanel.SetActive(activate);

            Reset();
            content.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);

            content.transform.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
        }

        public void ActivateResponsePanel(bool activate, string positiveBtn, string negetiveBtn, string response, bool responseType, AudioClip aud_clip, Action callBack = null)
        {

            txt_positiveResponse.text = "Move Along";
            txt_negetiveResponse.text = "Add More";

            responsePopup.SetActive(activate);

            if (activate)
            {
                if (aud_clip)
                {
                    GetComponent<AudioSource>().clip = aud_clip;
                    GetComponent<AudioSource>().Play();
                }

                txt_popUpResponse.WholeText = response;
                txt_popUpResponse.ShowTextResponse(() =>
                {
                    Debug.Log("callback");
                    panelResponse.SetActive(responseType);
                    callBack?.Invoke();
                });
            }
        }

        public void CloseResponsePanel()
        {
            responsePopup.SetActive(false);
            panelResponse.SetActive(false);
            Progress.Instance.IncreamentRating(3);
        }

        public void ActivateRequestPopup(bool activate, string request, AudioClip aud_clip, Action callBack = null)
        {
            requestPopup.SetActive(activate);

            GetComponent<AudioSource>().clip = aud_clip;
            GetComponent<AudioSource>().Play();

            txt_popUpFoodRequest.WholeText = request;
            txt_popUpFoodRequest.ShowTextResponse(() =>
            {
                callBack?.Invoke();
            });
        }

        public void ActivateRequestPopup(bool activate)
        {
            requestPopup.SetActive(activate);
        }

        public void SelectFood(Text txt_index)
        {
            int foodIndex = Convert.ToInt16(txt_index.text);
            _OnFoodSelected?.Invoke(foodIndex);

            Reset();
            txt_index.transform.parent.GetChild(3).gameObject.SetActive(true);
        }

        private void Reset()
        {
            for (int i = 0; i < content.transform.childCount; i++)
            {
                content.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
            }
        }

        public void OnResponseClicked(bool responseType)
        {
            if (responseType) {

                Debug.Log("currPrisoner "+currPrisoner);
                if (currPrisoner == 2)
                {
                    Progress.Instance.WasBadDecision = true;
                    Debug.Log("Bad decision");
                }

                Progress.Instance.DecreamentRating(3);
            }

            _OnResponseClicked?.Invoke(responseType);

            panelResponse.SetActive(false);
        }
    }
}
