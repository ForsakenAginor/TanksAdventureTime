namespace Characters
{
    public interface ISwitchable<in T>
    {
        public void Switch(T target);
    }
}