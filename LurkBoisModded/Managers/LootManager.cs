using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Managers
{
    public class LootManager
    {
        private static float MinimumQuality = 0.0f;
        private static float MaximumQuality = 0.0f;
        public static List<ItemWeightDefinition> Items = new List<ItemWeightDefinition>();

        public static void IncreaseMinimum(float amount)
        {
            MinimumQuality += amount;
        }

        public static void IncreaseMaximum(float amount)
        {
            MaximumQuality += amount;
        }

        public static List<ItemType> GetQualityItems(int amount)
        {
            List<ItemWeightDefinition> whitelist = Items.Where(x => x.Weight < MaximumQuality && x.Weight > MinimumQuality).ToList();
            float randomValue = Random.Range(MinimumQuality, MaximumQuality);
            List<ItemWeightDefinition> chosenItems = whitelist.Where(x => x.Weight < randomValue).ToList();
            List<ItemType> finalItems = chosenItems.Select(x => x.Item).ToList();
            List<ItemType> finalItemsLimited = new List<ItemType>();
            for (int i = 0; i < amount; i++)
            {
                if (finalItems.IsEmpty())
                {
                    finalItemsLimited.Add(ItemType.None);
                    continue;
                }
                ItemType item = finalItems.RandomItem();
                finalItems.Remove(item);
                finalItemsLimited.Add(item);
            }
            return finalItemsLimited;
        }
    }

    public class ItemWeightDefinition
    {
        public ItemType Item { get; set; } = ItemType.None;
        public float Weight { get; set; } = 0.0f;

        public ItemWeightDefinition()
        {

        }

        public ItemWeightDefinition(ItemType item, float weight)
        {
            Item = item;
            Weight = weight;
        }
    }
}
