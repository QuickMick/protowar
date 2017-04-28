using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay
{
    public class StaticsLibrary
    {
        #region singleton
        private static StaticsLibrary instance;

        public static StaticsLibrary Instance
        {
            get { return instance; }
        }

        private StaticsLibrary() { }
        #endregion singleton

        private Dictionary<string, BulletType> bulletLib = new Dictionary<string, BulletType>();

        public void Init()
        {
        }

        public BulletType GetBullet(string name)
        {
            return this.bulletLib[name];
        }
    }

    public class BulletType
    {
        public readonly float Radius;
        public readonly float Damage;

        public readonly float BulletSpeed = 0.1f;
        public readonly float BounceCount = 2;
        public readonly float SlugCount = 2;

    }
}
