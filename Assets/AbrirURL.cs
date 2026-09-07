using UnityEngine;

public class AbrirURL : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AbrirFacebook()
    {
        Application.OpenURL("https://www.facebook.com/");
    }

    public void AbrirSteam()
    {
        Application.OpenURL("https://store.steampowered.com/");
    }

    public void AbrirEpicGames()
    {
        Application.OpenURL("https://www.epicgames.com/store/en-US/");
    }

    public void AbrirDiscord()
    {
        Application.OpenURL("https://discord.com/");
    }
}
