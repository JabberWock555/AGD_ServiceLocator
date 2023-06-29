using System.Collections.Generic;
using UnityEngine;
using ServiceLocator.Player.Projectile;
using ServiceLocator.Main;

namespace ServiceLocator.Player
{
    public class PlayerService
    {
        private PlayerScriptableObject playerScriptableObject;
        private ProjectilePool projectilePool;

        private List<MonkeyController> activeMonkeys;
        private MonkeyView selectedMonkeyView;
        private int health;
        private int money;

        public PlayerService(PlayerScriptableObject playerScriptableObject, Transform projectileContainer)
        {
            this.playerScriptableObject = playerScriptableObject;
            projectilePool = new ProjectilePool(playerScriptableObject.ProjectilePrefab, playerScriptableObject.ProjectileScriptableObjects, projectileContainer);
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            health = playerScriptableObject.Health;
            money = playerScriptableObject.Money;
            GameService.Instance.UIService.UpdateHealthUI(health);
            GameService.Instance.UIService.UpdateMoneyUI(money);
            activeMonkeys = new List<MonkeyController>();
        }

        public void Update()
        {
            foreach(MonkeyController monkey in activeMonkeys)
            {
                monkey?.UpdateMonkey();
            }

            if(Input.GetMouseButtonDown(0))
            {
                UpdateSelectedMonkey();
            }
        }

        private void UpdateSelectedMonkey()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider!= null && hit.collider.GetComponent<MonkeyView>() != null)
            {
                selectedMonkeyView?.MakeRangeVisible(false);
                selectedMonkeyView = hit.collider.GetComponent<MonkeyView>();
                selectedMonkeyView.MakeRangeVisible(true);
            }
            else
            {
                selectedMonkeyView?.MakeRangeVisible(false);
            }
        }

        public void TrySpawningMonkey(MonkeyType monkeyType, int monkeyCost, Vector3 dropPosition)
        {
            if (monkeyCost > money)
                return;

            if (GameService.Instance.MapService.TryGetSpawnPosition(dropPosition, out Vector3 spawnPosition))
            {
                SpawnMonkey(monkeyType, spawnPosition);
                GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.SpawnMonkey);
            }
        }

        public void SpawnMonkey(MonkeyType monkeyType, Vector3 spawnPosition)
        {
            MonkeyScriptableObject monkeySO = playerScriptableObject.MonkeyScriptableObjects.Find(so => so.Type == monkeyType);
            MonkeyController monkey = new MonkeyController(monkeySO, projectilePool);
            monkey.SetPosition(spawnPosition);
            activeMonkeys.Add(monkey);

            money -= monkeySO.Cost;
            GameService.Instance.UIService.UpdateMoneyUI(money);
        }

        public void ReturnProjectileToPool(ProjectileController projectileToReturn) => projectilePool.ReturnItem(projectileToReturn);
        
        public void TakeDamage(int damageToTake)
        {
            health = health - damageToTake <= 0 ? 0 : health - damageToTake;
            GameService.Instance.UIService.UpdateHealthUI(health);
            if(health <= 0)
            {
                PlayerDeath();
            }
        }

        public void GetReward(int reward)
        {
            money += reward;
            GameService.Instance.UIService.UpdateMoneyUI(money);
        }

        private void PlayerDeath()
        {
            // Game Over UI.
            // Stop Bloon Spawning.
        }
    }
}