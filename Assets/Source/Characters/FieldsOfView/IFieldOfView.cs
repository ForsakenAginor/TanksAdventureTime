namespace Characters
{
    public interface IFieldOfView
    {
        public bool IsBlockingByWall();

        public bool CanView();
    }
}