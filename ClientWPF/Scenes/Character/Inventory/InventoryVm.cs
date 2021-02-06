using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using Shared;
using Shared.Alerts;
using Shared.Requests.Character;
using Shared.Responses;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClientWPF.Scenes.Character
{
    public class InventoryVm : Scene
    {
        public DroppableList Items { get; set; }
        public DroppableList DroppedItems { get; set; }
        public Equipment Equipment { get; set; }

        private readonly RequestClient client;
        private readonly Player player;
        private readonly SceneManagerVm sceneManager;

        public InventoryVm()
        {
            Items = new DroppableList();
            DroppedItems = new DroppableList();
        }

        public InventoryVm(RequestClient client, Player player, SceneManagerVm sceneManager)
        {
            this.client = client;
            this.player = player;
            this.sceneManager = sceneManager;

            client.SubscribeTo<DroppedItemAlert>(this, OnDroppedItemsAlert);
            client.SubscribeTo<PickupItemAlert>(this, OnPickupItemAlert);

            Items = new DroppableList();
            DroppedItems = new DroppableList();
            Equipment = new Equipment();

            var result = client.SendRequest(new GetInventoryRequest(), player);

            if (result.Success && result.data is InventoryResponse response)
            {
                foreach (var item in response.Inventory)
                {
                    var droppableItem = new DroppableItem(item, Items);
                    droppableItem.OnItemMoved += OnItemMoved;
                }
                foreach (var item in response.DroppedItems)
                {
                    var droppableItem = new DroppableItem(item, DroppedItems);
                    droppableItem.OnItemMoved += OnItemMoved;
                }
                Equipment.EquipAll(response.EquippedItems, OnItemMoved);
            }
            else
            {
                throw new Exception("Got wrong result");
            }
        }

        private void OnPickupItemAlert(PickupItemAlert alert)
        {
            var existingItem = DroppedItems.FirstOrDefault(i => i.ItemId == alert.Item.ItemId);
            if (existingItem != null)
            {
                existingItem.SetCount(existingItem.Count - alert.Item.Count);
                if (existingItem.Count <= 0)
                    DroppedItems.Remove(existingItem);
            }
            Notify("DroppedItems");
        }

        private void OnDroppedItemsAlert(DroppedItemAlert alert)
        {
            var existingItem = DroppedItems.FirstOrDefault(i => i.ItemId == alert.DroppedItem.ItemId);
            if (existingItem != null)
            {
                existingItem.SetCount(existingItem.Count + alert.DroppedItem.Count);
            }
            else
            {
                var item = new DroppableItem(alert.DroppedItem, DroppedItems);
                item.OnItemMoved += OnItemMoved;
            }
            Notify("DroppedItems");
        }

        private void OnItemMoved(object sender, DroppableItem.ItemMovedEventArgs e)
        {
            if(e.From == DroppedItems)
            {
                var item = e.Item.AsDescriptor().First();
                item.Count = 1;
                client.SendAction(new PickupItemRequest
                {
                    Item = item
                }, player);
            }
            if (e.From is EquipmentSlot fromSlot)
            {
                client.SendAction(new UnequipItemRequest
                {
                    Type = fromSlot.Type,
                    Slot = fromSlot.Slot
                }, player);
            }


            if (e.To is EquipmentSlot toSlot)
            {
                var item = e.Item.AsDescriptor().First();
                item.Count = 1;
                client.SendAction(new EquipItemRequest
                {
                    Item = item,
                    Type = toSlot.Type,
                    Slot = toSlot.Slot
                }, player);
            }
            if (e.To == DroppedItems)
            {
                client.SendAction(new DropItemsRequest
                {
                    DroppedItems = e.Item.AsDescriptor()
                }, player);
            }
        }

        public RelayCommand BackButton { get
            {
                return new RelayCommand(o =>
                {
                    //Return to previous scene
                    sceneManager.PopScene();
                });
            }
        }



        public override void Unload()
        {
            client.Unsubscribe(this);
        }
    }
}
