using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlockchainManager : MonoBehaviour
{
    public string Address { get; private set; }

    public Button nftButton;
    public Button playButton;
    public Button claimTokenBtn;

    public TextMeshProUGUI nftButtonText;
    public TextMeshProUGUI playButtonText;
    public TextMeshProUGUI claimTokenBtnText;

    public TextMeshProUGUI tokenBalanceText;    

    string NFTAddressSmartContract = "0xAFBbd54017CAdbe4d9437743F734896018e6A9d6";
    string tokenAddressSmartContract = "0x50Aa1F062AB55925458c23fbc0d7B12FE89bD33a";

    private void Start()
    {
        nftButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        claimTokenBtn.gameObject.SetActive(false);
        tokenBalanceText.gameObject.SetActive(false);
    }

    public async void Login()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(Address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        List<NFT> nftList = await contract.ERC721.GetOwned(Address);
        if (nftList.Count == 0)
        {
            nftButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(true);
            claimTokenBtn.gameObject.SetActive(true);
            TokenBalance();
        }
    }

    public async void ClaimNFTPass()
    {
        nftButtonText.text = "Claiming...";
        nftButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        var result = await contract.ERC721.ClaimTo(Address, 1);
        nftButtonText.text = "Claimed NFT Pass!";
        nftButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public async void TokenBalance()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(tokenAddressSmartContract);
        var result = await contract.ERC20.BalanceOf(Address);
        tokenBalanceText.text = "Token Owned: " + result.displayValue;
        tokenBalanceText.gameObject.SetActive(true);
    }

    public async void ClaimToken()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        claimTokenBtn.interactable = false;
        playButton.interactable = false;
        claimTokenBtnText.text = "Claiming...";
        var contract = ThirdwebManager.Instance.SDK.GetContract(tokenAddressSmartContract);

        int tokenclaimed = 5;

        var result = await contract.ERC20.ClaimTo(Address, tokenclaimed.ToString());

        claimTokenBtn.interactable = true;
        playButton.interactable = true;
        claimTokenBtnText.text = "Token Claimed";

        TokenBalance();
    }

}
