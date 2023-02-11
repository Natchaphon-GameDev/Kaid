using Manager;

namespace Bullet
{
    public class Bullet_U101 : BaseBullet
    {
        private void Start()
        {
            hero = GetHeroBluePrint(HeroName.U101_Jupiter);
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