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
    private Slider  primaryGauge,
                    secondaryGauge;

    [SerializeField]
    private Text    primaryGaugeText,
                    secondaryGaugeText;

    [SerializeField]
    private Transform actionButtonsContentFolder;
    [SerializeField]
    private ActionButton actionButtonPrefab;
    [SerializeField]
    private List<ActionButton> actionButtons;

    [SerializeField]
    private GameObject actionQueue;
    [SerializeField]
    private List<ActionQueueButton> actionQueueButtons;

    private Selectable selected;

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
        UpdateActionQueue();
        HideSecondaryGauge();

        //  Show the details panel
        gameObject.SetActive(true);
    }

    public void UpdateBasicInfo()
    {
        selectedName.text           = selected.selectedName;
        selectedDescription.text    = selected.selectedDescription;
        selectedImage.sprite        = selected.selectedImage;

        //  set the primary gauge to be the unit or building health, or resource amount
    }

    public void UpdateActionButtons()
    {
        //  Figure out what action this selectable needs buttons for...
        Action[] actions = selected.GetActions();

        //  Do I already have enough action buttons?  If not make some more!
        if (actions.Length > actionButtons.Count)
            for (int i = actionButtons.Count; i != actions.Length; ++i)
                actionButtons.Add(Instantiate(actionButtonPrefab, actionButtonsContentFolder));

        //  iterate through action buttons and assign their functions, disable extra buttons
        for (int i = 0; i != actionButtons.Count; ++i)
            if (i < actions.Length)
            {
                actionButtons[i].PopulateButton(selected, actions[i]);
                actionButtons[i].gameObject.SetActive(true);
            }
            else
                actionButtons[i].gameObject.SetActive(false);
    }
    public void UpdateActionQueue()
    {
        //  Action queueing is only for buildings
        if (selected as Building)
        {
            //  Get a list of queued actions from the building
            List<Action> queuedActions = (selected as Building).GetQueuedActions();

            //  iterate through action queue buttons and assign their icons, disable extra buttons
            for (int i = 0; i != actionQueueButtons.Count; ++i)
                if (i < queuedActions.Count)
                {
                    actionQueueButtons[i].PopulateButton(queuedActions[i].icon);
                    actionQueueButtons[i].gameObject.SetActive(true);
                }
                else
                    actionQueueButtons[i].gameObject.SetActive(false);

            //  show the action queue
            actionQueue.SetActive(true);
        }
        //  hide the action queue if this is not a building
        else
            actionQueue.SetActive(false);
    }

    public void CancelQueuedAction(int queuePosition)
    {
        if (!(selected as Building))
            return;

        (selected as Building).CancelQueuedAction(queuePosition);
        UpdateActionQueue();
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
        secondaryGauge.value = (float) currentProductivity / maxProductivity;
        secondaryGaugeText.text = currentProductivity + " / " + maxProductivity;
        secondaryGauge.gameObject.SetActive(true);
    }
}
