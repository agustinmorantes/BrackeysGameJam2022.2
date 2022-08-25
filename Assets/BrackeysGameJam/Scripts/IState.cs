namespace BrackeysGameJam
{
    public interface IState
    {
        void OnEnter(StateMachine stateMachine) {}
        void Update(StateMachine stateMachine);
    }
}