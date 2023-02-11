using Manager;

namespace Bullet
{
    public class Bullet_I58 : BaseBullet
    {
        private void Start()
        {
            hero = GetHeroBluePrint(HeroName.I58_Essex);
            GetStatus();
        }

        private void Update()
        {
            MoveToTarget();
        }

        private new void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
        }
    }
}