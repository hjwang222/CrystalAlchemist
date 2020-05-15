
public class SignalInteractable : Interactable
{
    public override void doSomethingOnSubmit() => MenuEvents.current.OpenCharacterCreation();    
}
