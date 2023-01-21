namespace MiniJam_Warmth.Controllers;

/// <summary>
/// State interface, to create state inherit this interface and implement any functionality you need.
/// </summary>
public interface IState
{
    /// <summary>
    /// Called if current state is active, by the update method
    /// </summary>
    public void Tick() {}
    /// <summary>
    /// Called when this state begins to be active
    /// </summary>
    public void OnEnter() {}
    /// <summary>
    /// Called when this state stops being active
    /// </summary>
    public void OnExit() {}
}