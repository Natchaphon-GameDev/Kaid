using Manager;

namespace Bullet
{
    public class Bullet_U556 : BaseBullet
    {
        private void Start()
        {
            hero = GetHeroBluePrint(HeroName.U556_Radford);
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