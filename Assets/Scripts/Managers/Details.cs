using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Details : Singleton<Details>
{
    [SerializeField]
    private Text    selectedName,
                    selectedDescription;

    [SerializeField]
    private Image   selectedImage;

    [SerializeField]
    private Image   backingImage;

    [SerializeField]
    private Image  primaryGauge,
                    secondaryGauge;

    [SerializeField]
    private Text    primaryGaugeText,
                    secondaryGaugeText;

    [SerializeField]
    private List<Button> actionButtons;

    private Selectable selected;

    [SerializeField]
    private Sprite buildingSprite;

    [SerializeField]
    private Sprite[] jobSprites;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public void ShowDetails(Selectable selected)
    {
        //  If nothing is selected, hide the details panel
        if (selected == null)
        {
            gameObject.SetActive(false);
            return;
        }
        //  Set this selected object as selected
        this.selected = selected;

        //  Populate the details panel with all the info
        UpdateBasicInfo();
        UpdateActionButtons();
        UpdatePrimaryGauge();
        HideSecondaryGauge();

        //  Show the details panel
        gameObject.SetActive(true);
    }

    public void UpdateBasicInfo()
    {
        selectedName.text           = selected.selectedName;
        selectedDescription.text    = selected.selectedDescription;

//        selectedImage.sprite        = selected.selectedImage;

        if(selected.GetComponent<Building>())
        {
            backingImage.sprite = buildingSprite;
            selectedImage.enabled = false;
        }
        else if(selected.GetComponent<Villager>())
        {
            Villager villager = selected.GetComponent<Villager>();

            if (villager.mainJob == Jobs.Villager)
                backingImage.sprite = jobSprites[7];
            else if (villager.mainJob < Jobs.Fighter)
                backingImage.sprite = jobSprites[8];
            else
                backingImage.sprite = jobSprites[9];
            selectedImage.sprite = jobSprites[(int)villager.mainJob];
        }
        else if(selected.GetComponent<Resource>())
        {
            backingImage.enabled = false;
            selectedImage.enabled = false;
        }
    }

    public void UpdateActionButtons()
    {
        foreach (Button button in actionButtons)
            button.gameObject.SetActive(false);

        Villager villager = selected.GetComponent<Villager>();

        if(villager)
        {
            actionButtons[0].gameObject.SetActive(villager.CanBuild());

            List<Jobs> options = villager.TrainingOptions();

            foreach(Jobs option in options)
                actionButtons[(int)option].gameObject.SetActive(true);
        }
    }

    public void TrainButtonPressed(int jobIndex)
    {
        StartCoroutine(selected.GetComponent<Villager>().TrainNewJob((Jobs) jobIndex));
    }

    public void UpdatePrimaryGauge()
    {
        if(selected.GetComponent<Resource>())
            primaryGauge.gameObject.SetActive(false);
        else
        {
            primaryGauge.fillAmount = (float) selected.health / selected.maxHealth;
            primaryGaugeText.text = selected.health + " / " + selected.maxHealth;
            primaryGauge.gameObject.SetActive(true);
        }
    }

    public void UpdateActionQueue()
    {

    }

    public void CancelQueuedAction(int queuePosition)
    {

    }

    public void HideSecondaryGauge()
    {
        secondaryGauge.gameObject.SetActive(false);
    }

    public void UpdateSecondaryGauge(int currentProductivity, int maxProductivity)
    {
        if(currentProductivity == maxProductivity)
        {
            secondaryGauge.gameObject.SetActive(false);
            return;
        }
        secondaryGauge.fillAmount = (float) currentProductivity / maxProductivity;
        secondaryGaugeText.text = currentProductivity + " / " + maxProductivity;
        secondaryGauge.gameObject.SetActive(true);
    }
}
