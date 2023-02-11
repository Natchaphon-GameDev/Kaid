using Manager;

namespace Bullet
{
    public class Bullet_I168 : BaseBullet
    {
        private void Start()
        {
            hero = GetHeroBluePrint(HeroName.I168_Memphis);
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