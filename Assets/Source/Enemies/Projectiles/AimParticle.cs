namespace Enemies
{
    public class AimParticle : SpawnableParticle, IAimParticle
    {
        public void Show()
        {
            Play();
        }

        public void Hide()
        {
            Push();
            Stop();
        }
    }
}