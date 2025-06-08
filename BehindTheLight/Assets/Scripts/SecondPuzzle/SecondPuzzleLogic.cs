using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SecondPuzzleLogic : MonoBehaviour
{

    [SerializeField] private Door doorToUnlock;


    [SerializeField] private int numOfButtons = 3;
    [SerializeField] private List<ButtonPuzzle> buttons;


    [SerializeField] private List<Renderer> slotRenderers;


    [SerializeField] private List<Texture2D> numberTextures;
    [SerializeField] private ChangeAlbedoOnTrigger changeAlbedoOnTrigger;


    int[] sequence;          
    int currentIndex;       
    bool puzzleSolved;

    void Start() => RestartSequence();

    public bool OnButtonPressedSequence(int idButton, ButtonPuzzle pressedButton)
    {
        if (puzzleSolved) return true;  

        if (sequence[currentIndex] == idButton) 
        {
            currentIndex++;
            if (currentIndex >= sequence.Length)
                CompletePuzzle();

            return true;  
        }

        RestartSequence();
        return false;   
    }

    void CompletePuzzle()
    {
        puzzleSolved = true;
        doorToUnlock.UnlockDoor();
        changeAlbedoOnTrigger.ChangeAlbedoOnActivate();
        Debug.Log("¡Puzzle completado!");
    }
    void RestartSequence()
    {
        sequence = GenerateSequence(numOfButtons);
        currentIndex = 0;
        puzzleSolved = false;

        for (int i = 0; i < slotRenderers.Count && i < sequence.Length; i++)
        {
            int textureIndex = sequence[i] - 1;
            if (textureIndex < 0 || textureIndex >= numberTextures.Count)
            {
                Debug.LogError($"Número {sequence[i]} fuera de rango en numberTextures.");
                continue;
            }

            var rend = slotRenderers[i];
            var newMat = new Material(rend.material);
            newMat.mainTexture = numberTextures[textureIndex];
            rend.material = newMat;

            var lightComp = rend.GetComponent<LightedObjects>();
            if (lightComp != null)
            {
                var fi = typeof(LightedObjects)
                    .GetField("mat", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null)
                    fi.SetValue(lightComp, newMat);
            }
        }

        foreach (var b in buttons)
            b.ResetPosition();

        Debug.Log("Nueva secuencia: " + string.Join(",", sequence));
    }

    static int[] GenerateSequence(int n)
    {
        var seq = new int[n];
        var pool = new List<int>();
        for (int i = 1; i <= n; i++) pool.Add(i);

        for (int i = 0; i < n; i++)
        {
            int k = Random.Range(0, pool.Count);
            seq[i] = pool[k];
            pool.RemoveAt(k);
        }
        return seq;
    }
}
