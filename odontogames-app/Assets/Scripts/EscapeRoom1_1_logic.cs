using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeRoom1_1_logic : MonoBehaviour
{
    public GameObject panel; // El panel de la UI
    public GameObject playerCharacter; // El jugador
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
    }

    void Update()
    {
        if (picturesSpawned >= numberOfPictures)
        {
            Debug.Log("Juego terminado. Bien hecho!");
        }

        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].IsPlatformMoving())
                CheckAnswer(platforms[i].GetPlatformName());
        }
    }

    public void CheckAnswer(string containerName)
    {
        string imageType = currentImage.GetComponent<ShowImage>().GetTextureName();
        if (imageType == containerName) score++; else score--;
        Destroy(currentImage);
        CreateImagePrefab();
    }

    void CreateImagePrefab()
    {
        currentImage = Instantiate(prefab, transform.position, Quaternion.identity); // Crea una instancia del prefab en la posición especificada
        currentImage.GetComponent<ShowImage>().SetPanel(panel);
        currentImage.GetComponent<ShowImage>().SetPlayerCharacter(playerCharacter);

        // ponemos la textura al panel
        Texture2D texture = null;
        PickRandomTexture(out texture);
        currentImage.GetComponent<ShowImage>().SetTexture(texture);
        picturesSpawned++;
    }

    void PickRandomTexture(out Texture2D texture)
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
}
