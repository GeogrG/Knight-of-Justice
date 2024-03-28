using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAnim : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;

    public void ChangePictureInUISlider(float procent)
    {
        //Counitng which image to show based on percentage of the slider
        //and changing the image according to that
        int whichPhotoToShow = Mathf.RoundToInt( procent * (sprites.Count - 1));
        GetComponent<Image>().sprite = sprites[whichPhotoToShow];
    }

    public void HideSlider()
    {
        this.gameObject.SetActive(false);
    }
}
