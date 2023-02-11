using Manager;

namespace Bullet
{
    public class Bullet_W75 : BaseBullet
    {
        private void Start()
        {
            hero = GetHeroBluePrint(HeroName.W75_Valiant);
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