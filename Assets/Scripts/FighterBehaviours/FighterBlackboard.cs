using System.Collections.Generic;
using UnityEngine;

namespace Assets.BHTree
{
    public class FighterBlackboard : Blackboard
    {
        public Fighter fighter { get; set; } // not like this

        public Behaviour scanningBehaviour { get; private set; }
        public Behaviour weaponBehaviour { get; private set; }
        public Behaviour navigationTree { get; private set; }

        public Navigator navigator { get; set; }
        public GameObject target { get; set; }
        public GameObject wingMan { get; set; }
        public GameObject mothership { get; set; }
        public SpaceManager spaceManager { get; private set; }

        public FighterBlackboard(Fighter fighter, GameObject parentObject)
        {
            this.fighter = fighter;
            this.parentObject = parentObject;
            mothership = fighter.mothership;
            tickInterval = .1f;
            // add some variance in update speed because some dudes react faster than others (but mostly for smoothing fps drops when clumps spawn)
            tickInterval = UnityEngine.Random.Range(-tickInterval * .2f, tickInterval * .2f) + tickInterval;
            spaceManager = fighter.spaceManager;

        }


        public void AddScanner()
        {
            scanningBehaviour = new ScanningBehaviour(this);
            scanningBehaviour = new Regulator(scanningBehaviour, tickInterval);
        }


        public void AddWeapon(Weapon weapon)
        {
            weaponBehaviour = new WeaponBehaviour(this, weapon);
            weaponBehaviour = new Regulator(weaponBehaviour, tickInterval);
        }


        public void AddNavigation(Propulsion prop)
        {
            navigator = new Navigator(prop);
            navigationTree = new NavigationTree(this, navigator, prop);
            navigationTree = new Regulator(navigationTree, tickInterval);
        }        


        public override void BlackboardUpdate()
        {
            scanningBehaviour.BTick();
            weaponBehaviour.BTick();
            navigationTree.BTick();
        }


        public List<GameObject> GetFriendlies()
        {
           return spaceManager.shipCounter.fighterTeams[fighter.teamNumber];
        }


        public List<GameObject> GetEnemies()
        {
            List<GameObject> enemies = new List<GameObject>();
            for (int i = 1; i <= spaceManager.shipCounter.fighterTeams.Count; i++)
            {
                if (i != fighter.teamNumber)
                {
                    enemies.AddRange(spaceManager.shipCounter.fighterTeams[i]);
                }
            }
            return enemies;
        }
    }
}
