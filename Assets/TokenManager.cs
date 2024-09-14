using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using System;

public class TokenManager : MonoBehaviour
{
    public string Address { get; private set; }
    public Button addColBtn;
    public Button replayBtn;
    public Button menuBtn;
    public Button rewind;
    public Button tokenForRewaidBtn;

    public Text ClaimingStatusText;

    string TokenAddressSmartContract = "0x50Aa1F062AB55925458c23fbc0d7B12FE89bD33a";

    private void Start()
    {
        ClaimingStatusText.gameObject.SetActive(false);
        TokenBalance();
    }

    public async void TokenBalance()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        var result = await contract.ERC20.BalanceOf(Address);
        ClaimingStatusText.text = "Token: " + result.displayValue;
        ClaimingStatusText.gameObject.SetActive(true);
    }

    private static int ConvertStringToRoundedInt(string numberStr)
    {
        // Convert the string to a double
        double number = double.Parse(numberStr);

        // Round the number
        double roundedNumber = Math.Round(number);

        // Convert to int and return
        return (int)roundedNumber;
    }

    public async void SpendTokenToAddColumn()
    {
        if (GameManager.Instance.isAddBox == true) return;
        if (GameManager.Instance.currentLevel == 0) return;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        addColBtn.interactable = false;
        replayBtn.interactable = false;
        menuBtn.interactable = false;
        rewind.interactable = false;
        tokenForRewaidBtn.interactable = false;

        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        var numberStr = await contract.ERC20.BalanceOf(Address);

        int roundedInt = ConvertStringToRoundedInt(numberStr.displayValue);
        if (roundedInt <= 0)
        {
            ClaimingStatusText.text = "Not Enough!";
            return;
        }
        await contract.ERC20.Burn("1");
        TokenBalance();

        GUIManager.Instance.WatchVideo3();

        addColBtn.interactable = true;
        replayBtn.interactable = true;
        menuBtn.interactable = true;
        rewind.interactable = true;
        tokenForRewaidBtn.interactable = true;
    }

    public async void SpendTokenToAddRewind()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        addColBtn.interactable = false;
        replayBtn.interactable = false;
        menuBtn.interactable = false;
        rewind.interactable = false;
        tokenForRewaidBtn.interactable = false;

        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        var numberStr = await contract.ERC20.BalanceOf(Address);

        int roundedInt = ConvertStringToRoundedInt(numberStr.displayValue);
        if (roundedInt <= 0)
        {
            ClaimingStatusText.text = "Not Enough!";
            return;
        }
        await contract.ERC20.Burn("1");
        TokenBalance();

        GUIManager.Instance.WatchVideo2();

        tokenForRewaidBtn.gameObject.SetActive(false);
        addColBtn.interactable = true;
        replayBtn.interactable = true;
        menuBtn.interactable = true;
        rewind.interactable = true;
        tokenForRewaidBtn.interactable = true;
    }
}
