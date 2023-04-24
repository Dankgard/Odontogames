using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class escapeRoom1_1_logic : MonoBehaviour
{
    public GameObject panel; // El panel de la UI
    public GameObject prefab; // El prefab que se va a generar
    public GameObject door;
    public GameObject robot;

    public Countdown countdown;

    // un array de las plataformas que se tienen que llenar
    public escapeRoom1_1_AnswerBox[] platforms;

    private int numberOfPictures;

    private GameObject currentImage;

    public Texture2D[] textures;

    private int picturesSpawned = 0;
    private bool gameEnded = false;
    private bool lostByTime = false;

    private void Start()
    {
        door.transform.GetComponent<Animator>().enabled = false;
        numberOfPictures = textures.Length;
        CreateImagePrefab();
        CamerasManager.camerasManagerInstance.SwapCamera(0);
        picturesSpawned = numberOfPictures;
    }

    private void FixedUpdate()
    {
        if (picturesSpawned <= 0 && !gameEnded)
        {
            gameEnded = true;
            EndGame();
        }

        if (countdown.GetTimeLeft() <= 0.0f && !gameEnded)
        {
            gameEnded = true;
            lostByTime = true;
            EndGame();
        }

        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].IsPlatformMoving())
            {
                CheckAnswer(platforms[i].GetPlatformName());
                break;
            }
        }
    }

    public void CheckAnswer(string containerName)
    {
        string imageType = currentImage.GetComponent<ShowImage>().GetTextureName();
        if (imageType == containerName)
        {
            GameManager.instance.CorrectAnswer();
        }
        else
        {
            GameManager.instance.WrongAnswer();
        }

        Destroy(currentImage);
        CreateImagePrefab();
    }

    private void CreateImagePrefab()
    {
        currentImage = Instantiate(prefab, transform.GetChild(0).position, Quaternion.identity); // Crea una instancia del prefab en la posición especificada
        currentImage.GetComponent<ShowImage>().SetPanel(panel);

        // ponemos la textura al panel
        Texture2D texture = null;
        PickRandomTexture(out texture);
        currentImage.GetComponent<ShowImage>().SetTexture(texture);
        picturesSpawned--;
    }

    private void PickRandomTexture(out Texture2D texture)
    {
        if (numberOfPictures == 0)
        {
            texture = null;
            return;
        }

        int index = Random.Range(0, numberOfPictures); // Elige un índice aleatorio de la lista de texturas disponibles
        texture = textures[index]; // Obtiene la textura correspondiente al índice elegido

        // Desplaza las texturas restantes a la izquierda para llenar el vacío dejado por la textura eliminada
        for (int i = index; i < numberOfPictures - 1; i++)
        {
            textures[i] = textures[i + 1];
        }
        numberOfPictures--;
    }

    public void OnShowImagePressed()
    {
        SoundManager.instance.PlaySound(1);
        currentImage.GetComponent<ShowImage>().ShowPanel();
    }

    private void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        CamerasManager.camerasManagerInstance.SwapCamera(3);
        //if (!lostByTime)
        //{
        //    while (!robot.GetComponent<escaperoom1_1_robot>().GameHasEnded());
        //}
        SoundManager.instance.PlaySound(2);

        for (int i = 0; i < picturesSpawned; i++)
            GameManager.instance.WrongAnswer();

        GameManager.instance.ReceiveBonusPoints(countdown.GetBonusPoints());
        GameManager.instance.UpdatePlayerScore();

        door.transform.GetComponent<Animator>().enabled = true;
        door.transform.GetComponent<Animator>().Play("door_anim");
        SoundManager.instance.PlaySound(4);
        yield return new WaitForSeconds(1.5f);
        MySceneManager.instance.LoadScene("MinigameEnd");
    }
}
