namespace Enemies
{
    public interface IEnemyFactory<out T>
    {
        public T Create(EnemyTypes type);
    }
}