using UnityEngine;

public interface IInteractable
{
    string OnInteractMsg {  get; }
    void OnInteract();
}
