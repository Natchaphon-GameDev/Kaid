using Manager;

namespace Bullet
{
    public class Bullet_Z46 : BaseBullet
    {
        private void Start()
        {
            hero = GetHeroBluePrint(HeroName.Z46_Genesis);
            GetStatus();
        }

        private void Update()
        {
            MoveToTarget();
        }

        private new void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();;
        }
    }
}