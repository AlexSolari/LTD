using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player
    {
        private GameController gameController;
        private int _gold = 10;
        private int _health = 100;

        public int Gold
        {
            get
            {
                return _gold;
            }
            set
            {
                _gold = value;
                OnGoldChange();
            }
        }

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
                OnHealthChange();
            }
        }

        protected void OnGoldChange()
        {
            gameController.SetGold(_gold);
        }

        protected void OnHealthChange()
        {
            
            gameController.SetHP(_health);
        }

        #region Singleton

        private static Player _instance;

        public static Player Current
        {
            get
            {
                if (_instance == null)
                    _instance = new Player();

                return _instance;

            }
        }

        public Player()
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        #endregion
    }
}
