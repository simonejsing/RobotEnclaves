namespace ActionPlatformer
{
    internal interface IMonoInput
    {
        void Update();
        bool MoveLeft();
        bool MoveRight();
        bool Jump();
    }
}