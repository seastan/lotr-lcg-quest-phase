using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour {
    public InputField inputSumWillpower;
    public InputField inputProgress;

    public Button buttonWillClearAll;
    public Button buttonWillClear;
    public Button buttonWillMinu1;
    public Button buttonWillPlus1;
    public Button buttonWillPlus2;
    public Button buttonWillPlus5;

    public Button buttonThreat;
    public Button buttonWillP1;
    public Button buttonWillP2;
    public Button buttonWillP3;
    public Button buttonWillP4;
    public Button buttonRounds;
    public Button buttonEngP1;
    public Button buttonEngP2;
    public Button buttonEngP3;
    public Button buttonEngP4;

    public Image buttonPanelThreat;
    public Image buttonPanelWillP1;
    public Image buttonPanelWillP2;
    public Image buttonPanelWillP3;
    public Image buttonPanelWillP4;
    public Image buttonPanelRounds;
    public Image buttonPanelEngP1;
    public Image buttonPanelEngP2;
    public Image buttonPanelEngP3;
    public Image buttonPanelEngP4;

    public Image panelThreat;
    public Image panelWillP1;
    public Image panelWillP2;
    public Image panelWillP3;
    public Image panelWillP4;
    public Image panelRounds;
    public Image panelEngP1;
    public Image panelEngP2;
    public Image panelEngP3;
    public Image panelEngP4;

    private Image[] scaledPanelsTop = new Image[5];
    private Image[] scaledPanelsBot = new Image[5];

    public Button buttonRoundsUp;
    public Button buttonRoundsDown;
    public Button buttonP1Up;
    public Button buttonP1Down;
    public Button buttonP2Up;
    public Button buttonP2Down;
    public Button buttonP3Up;
    public Button buttonP3Down;
    public Button buttonP4Up;
    public Button buttonP4Down;

    public Button buttonNewRound;
    public Button buttonAllMinu1;
    public Button buttonAllPlus1;

    public Button button0;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;
    public Button button8;
    public Button button9;
    public Button buttonClear;
    public Button buttonDone;

    public Dropdown playerDropdown;
    public Toggle toggleKeepStaging;
    public Toggle toggleScreenDim;
    public Button buttonNewGame;

    private Button[] buttonNum = new Button[10];

    private int[] totals = new int[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0}; // Threat, P1, P2, P3, P4. Rounds, Eng1, Eng2, Eng3, Eng4
    private int sumWillpower = 0;
    private int progress = 0;
    // private int rounds = 0;
    // private int[] engs = new int[] { 0, 0, 0, 0 };

    private int activeIndex = -1;

    private Button[] buttons = new Button[10];
    private Image[] buttonPanels = new Image[10];

    private int numPlayers = 4;
    public bool keepStaging = false;

    // Increment button i by value v. If i is -1, icrement the active value. If v is 0, set value to 0.
    public void ButtonIncrement(int i, int v) {
        if (i < 0 && activeIndex >= 0 && activeIndex <= 4) i = activeIndex;
        if (i < 0) return; // Can't increment if nothing is selected
        if (i <= 4 && i > numPlayers) return; // Don't increment willpower for hidden players
        if (i-5 <= 4 && i-5 > numPlayers) return; // Don't increment threat for hidden players
        if (v == 0) {
            totals[i] = 0;
        } else {
            totals[i] += v;
        }
        if (totals[i] < 0) totals[i] = 0;
        if (totals[i] > 999) totals[i] = 999;
        UpdateAll();
    }

    // Update everything on the screen to reflect changes.
    public void UpdateAll() {
        // Update all button text to reflect the totals[i].
        for (int i = 0; i<totals.Length; i++) {
            buttons[i].GetComponentInChildren<Text>().text = totals[i].ToString();
        }
        // Sum Willpower
        sumWillpower = totals[1] + totals[2] + totals[3] + totals[4];
        if (sumWillpower < 0) sumWillpower = 0;
        if (sumWillpower > 9999) sumWillpower = 9999;
        inputSumWillpower.text = sumWillpower.ToString();
        // Progress
        progress = sumWillpower - totals[0];
        if (progress < -999) progress = -999;
        if (progress > 9999) progress = 9999;
        inputProgress.text = progress.ToString();
    }

    // Clear the willpower staging threat and willpower values
    public void ClearAll() {
        if (!keepStaging) totals[0] = 0; // If the keepStagingThreat toggle is on, don't reset it.
        for (int i = 1; i < 5; i++) {
            totals[i] = 0;
        }
        UpdateAll();
    }

    // Set he active button text to 0
    public void ResetActive() {
        if (activeIndex < 0) return;
        totals[activeIndex] = 0;
        UpdateAll();
    }

    // Add the pressed digit to the selected value
    public void ButtonNumPressed(int i) {
        if (activeIndex < 0) return;
        int activeVal = totals[activeIndex];
        activeVal = (activeVal * 10) + i;
        if (activeVal < -99) activeVal = -99;
        if (buttons[activeIndex] == buttonRounds && activeVal < 0) activeVal = 0;
        if (activeVal > 999) activeVal = 999;
        totals[activeIndex] = activeVal;
        UpdateAll();
    }
    
    // Unselect and unhighlight button
    public void MakeInactive() {
        if (activeIndex < 0) return;
        buttonPanels[activeIndex].color = new Color(1f, 1f, 0f, 0f);
        activeIndex = -1;
    }

    // Slect and highlight button
    public void MakeActive(int i) {
        // Debug.Log(i);
        if (activeIndex == i) { // If button was active, make inactive
            MakeInactive();
            return;
        }
        MakeInactive();
        activeIndex = i;
        buttonPanels[activeIndex].color = new Color(1f, 1f, 0f, 1f);
    }

    // Prevent screen from dimming
    public void ScreenDim() {
        if (toggleScreenDim.isOn) Screen.sleepTimeout = SleepTimeout.NeverSleep;
        else Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    // Prevent staging threat from being reset
    public void KeepStagingThreat() {
        if (toggleKeepStaging.isOn) keepStaging = true;
        else keepStaging = false;
    }

    // Handle changing the number of players
    public void ChangeNumPlayers() {
        // Set numPlayers global
        numPlayers = playerDropdown.value + 1;
        // Make current selection inactive (in case it disappears)
        MakeInactive();
        // Hide panels starting at numPlayers+1.
        for (int i = numPlayers+1; i <= 4; i++) {
            totals[i] = 0; // Set Willpower to 0
            totals[i + 5] = 0; // Set Threat to 0;
        }
        // Stretch panels
        for (int i = 0; i < scaledPanelsTop.Length; i++) {
            scaledPanelsTop[i].rectTransform.anchorMin = new Vector2(i * 1f / (numPlayers + 1), 0);
            scaledPanelsTop[i].rectTransform.anchorMax = new Vector2((i + 1) * 1f / (numPlayers + 1), 1);
            scaledPanelsBot[i].rectTransform.anchorMin = new Vector2(i * 1f / (numPlayers + 1), 0);
            scaledPanelsBot[i].rectTransform.anchorMax = new Vector2((i + 1) * 1f / (numPlayers + 1), 1);
        }
        UpdateAll();
    }

    // Start a new game
    public void NewGame() {
        MakeInactive();
        totals = new int[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 };
        for (int p = 0; p < buttons.Length; p++) {
            buttons[p].GetComponentInChildren<Text>().text = totals[p].ToString();
        }
        UpdateAll();
    }

    void OnEnable() {
        // Selectable Buttons
        buttons[0] = buttonThreat;
        buttons[1] = buttonWillP1;
        buttons[2] = buttonWillP2;
        buttons[3] = buttonWillP3;
        buttons[4] = buttonWillP4;
        buttons[5] = buttonRounds;
        buttons[6] = buttonEngP1;
        buttons[7] = buttonEngP2;
        buttons[8] = buttonEngP3;
        buttons[9] = buttonEngP4;
        // Button Panels
        buttonPanels[0] = buttonPanelThreat;
        buttonPanels[1] = buttonPanelWillP1;
        buttonPanels[2] = buttonPanelWillP2;
        buttonPanels[3] = buttonPanelWillP3;
        buttonPanels[4] = buttonPanelWillP4;
        buttonPanels[5] = buttonPanelRounds;
        buttonPanels[6] = buttonPanelEngP1;
        buttonPanels[7] = buttonPanelEngP2;
        buttonPanels[8] = buttonPanelEngP3;
        buttonPanels[9] = buttonPanelEngP4;
        // On click
        for (int i = 0; i < buttons.Length; i++) {
            int capturedIndex = i;
            buttons[capturedIndex].onClick.AddListener(() => MakeActive(capturedIndex));
        }

        // Scaled Panels
        scaledPanelsTop[0] = panelThreat;
        scaledPanelsTop[1] = panelWillP1;
        scaledPanelsTop[2] = panelWillP2;
        scaledPanelsTop[3] = panelWillP3;
        scaledPanelsTop[4] = panelWillP4;
        scaledPanelsBot[0] = panelRounds;
        scaledPanelsBot[1] = panelEngP1;
        scaledPanelsBot[2] = panelEngP2;
        scaledPanelsBot[3] = panelEngP3;
        scaledPanelsBot[4] = panelEngP4;

        // Number Buttons
        buttonNum[0] = button0;
        buttonNum[1] = button1;
        buttonNum[2] = button2;
        buttonNum[3] = button3;
        buttonNum[4] = button4;
        buttonNum[5] = button5;
        buttonNum[6] = button6;
        buttonNum[7] = button7;
        buttonNum[8] = button8;
        buttonNum[9] = button9;
        // On click
        for (int i = 0; i < buttonNum.Length; i++) {
            int capturedIndex = i;
            buttonNum[capturedIndex].onClick.AddListener(delegate { ButtonNumPressed(capturedIndex); });
        }

        // Clear / Done
        buttonClear.onClick.AddListener(delegate { ResetActive(); });
        buttonDone.onClick.AddListener(delegate { MakeInactive(); });

        // Increment Buttons
        buttonWillClearAll.onClick.AddListener(() => ClearAll());
        buttonWillMinu1.onClick.AddListener(()  => ButtonIncrement(-1,-1));
        buttonWillPlus1.onClick.AddListener(()  => ButtonIncrement(-1,1));
        buttonWillPlus2.onClick.AddListener(()  => ButtonIncrement(-1,2));
        buttonWillPlus5.onClick.AddListener(()  => ButtonIncrement(-1,5));

        buttonRoundsUp.onClick.AddListener(()   => ButtonIncrement(5, 1));
        buttonRoundsDown.onClick.AddListener(() => ButtonIncrement(5, -1));
        buttonP1Up.onClick.AddListener(()       => ButtonIncrement(6, 1));
        buttonP1Down.onClick.AddListener(()     => ButtonIncrement(6, -1));
        buttonP2Up.onClick.AddListener(()       => ButtonIncrement(7, 1));
        buttonP2Down.onClick.AddListener(()     => ButtonIncrement(7, -1));
        buttonP3Up.onClick.AddListener(()       => ButtonIncrement(8, 1));
        buttonP3Down.onClick.AddListener(()     => ButtonIncrement(8, -1));
        buttonP4Up.onClick.AddListener(()       => ButtonIncrement(9, 1));
        buttonP4Down.onClick.AddListener(()     => ButtonIncrement(9, -1));

        // New Rounds / Increment All
        buttonNewRound.onClick.AddListener(delegate {
            if (!keepStaging) ButtonIncrement(0, 0);
            ButtonIncrement(1, 0);
            ButtonIncrement(2, 0);
            ButtonIncrement(3, 0);
            ButtonIncrement(4, 0);
            ButtonIncrement(5, 1);
            ButtonIncrement(6, 1);
            ButtonIncrement(7, 1);
            ButtonIncrement(8, 1);
            ButtonIncrement(9, 1);
        });
        buttonAllMinu1.onClick.AddListener(delegate {
            ButtonIncrement(6, -1);
            ButtonIncrement(7, -1);
            ButtonIncrement(8, -1);
            ButtonIncrement(9, -1);
        });
        buttonAllPlus1.onClick.AddListener(delegate {
            ButtonIncrement(6, 1);
            ButtonIncrement(7, 1);
            ButtonIncrement(8, 1);
            ButtonIncrement(9, 1);
        });

        // Settings Panel
        playerDropdown.ClearOptions();
        for (int i=1; i<=4; i++) {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = i.ToString();
            playerDropdown.options.Add(data);
        }
        playerDropdown.value = 3;
        playerDropdown.onValueChanged.AddListener(delegate { ChangeNumPlayers(); });

        buttonNewGame.onClick.AddListener(() => NewGame());
        toggleKeepStaging.onValueChanged.AddListener(delegate { KeepStagingThreat(); });
        toggleScreenDim.onValueChanged.AddListener(delegate { ScreenDim(); });
        //buttonNewGame.onClick.AddListener(() => ClearAll());

    }



    void OnDisable() {

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

}
