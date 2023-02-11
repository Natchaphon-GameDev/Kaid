using Manager;

namespace Bullet
{
    public class Bullet_I26 : BaseBullet
    {
        private void Start()
        {
            hero = GetHeroBluePrint(HeroName.I26_Albacore);
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