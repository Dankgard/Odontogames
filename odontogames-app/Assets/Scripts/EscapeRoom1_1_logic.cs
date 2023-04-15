using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeRoom1_1_logic : MonoBehaviour
{
    public GameObject panel; // El panel de la UI
    public GameObject prefab; // El prefab que se va a generar

    // un array de las plataformas que se tienen que llenar
    public EscapeRoom1_1_AnswerBox[] platforms;

    private int numberOfPictures;

    private GameObject currentImage;

    public Texture2D[] textures;

    private int picturesSpawned = 0;
    private int score = 0;

    void Start()
    {
        numberOfPictures = textures.Length;
        CreateImagePrefab();
        CamerasManager.camerasManagerInstance.SwapCamera(0);
        picturesSpawned = numberOfPictures;
    }

    void Update()
    {
        if (picturesSpawned <= 0)
        {
            //Debug.Log("Juego terminado. Bien hecho!");
            //StrapiComponent._instance.UpdatePlayerScore(score);
        }

        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].IsPlatformMoving())
                CheckAnswer(platforms[i].GetPlatformName());
        }
    }

    public void CheckAnswer(string containerName)
    {
        if (picturesSpawned > 1)
        {
            CamerasManager.camerasManagerInstance.SwapCamera(1);
        }
        string imageType = currentImage.GetComponent<ShowImage>().GetTextureName();
        if (imageType == containerName) score++; else score--;
        Destroy(currentImage);
        CreateImagePrefab();
    }

    void CreateImagePrefab()
    {
        currentImage = Instantiate(prefab, transform.GetChild(0).position, Quaternion.identity); // Crea una instancia del prefab en la posici�n especificada
        currentImage.GetComponent<ShowImage>().SetPanel(panel);

        // ponemos la textura al panel
        Texture2D texture = null;
        PickRandomTexture(out texture);
        currentImage.GetComponent<ShowImage>().SetTexture(texture);
        picturesSpawned--;
    }

    void PickRandomTexture(out Texture2D texture)
    {
        if (numberOfPictures == 0)
        {
            texture = null;
            return;
        }

        int index = Random.Range(0, numberOfPictures); // Elige un �ndice aleatorio de la lista de texturas disponibles
        texture = textures[index]; // Obtiene la textura correspondiente al �ndice elegido

        // Desplaza las texturas restantes a la izquierda para llenar el vac�o dejado por la textura eliminada
        for (int i = index; i < numberOfPictures - 1; i++)
        {
            textures[i] = textures[i + 1];
        }
        numberOfPictures--;
    }
}
