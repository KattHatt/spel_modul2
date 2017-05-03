﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace GameEngine
{
    class InventorySystem : ISystem
    {
        public void Update(GameTime gameTime)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            foreach (var entity in cm.GetComponentsOfType<PlayerControlComponent>())
            {
                InventoryComponent invenComp = cm.GetComponentForEntity<InventoryComponent>(entity.Key);
                EquipmentComponent equipComp = cm.GetComponentForEntity<EquipmentComponent>(entity.Key);
                PlayerControlComponent playerComp = (PlayerControlComponent)entity.Value;
                if (invenComp != null)
                {
                    //Test to add item
                    if (playerComp.ActionBar1.IsButtonDown())
                    {
                        AddItemToInventory(entity.Key, 10);
                    }
                    //Test to add item
                    if (playerComp.ActionBar2.IsButtonDown())
                    {
                        AddItemToInventory(entity.Key, 11);

                    }
                    if (playerComp.Inventory.IsButtonDown())
                    {
                        MoveComponent moveComp = cm.GetComponentForEntity<MoveComponent>(entity.Key);
                        AttackComponent attackComp = cm.GetComponentForEntity<AttackComponent>(entity.Key);
                        if (invenComp.IsOpen)
                        {
                            attackComp.CanAttack = true;
                            moveComp.canMove = true;
                            invenComp.HeldItem = 0;
                            invenComp.IsOpen = false;
                        }
                        else
                        {
                            attackComp.CanAttack = false;
                            moveComp.canMove = false;
                            invenComp.IsOpen = true;
                        }
                    }
                    if (invenComp.IsOpen)
                    {
                        if (invenComp.selectSlotCurCooldown <= 0.0f)
                        {
                            Vector2 stickDir = new Vector2(playerComp.Movement.GetDirection().Y, playerComp.Movement.GetDirection().X);
                            if (Math.Abs(stickDir.X) > 0.5f || Math.Abs(stickDir.Y) > 0.5f)
                            {
                                Point direction = SystemManager.GetInstance().GetSystem<MoveSystem>().CalcDirection(stickDir.X, stickDir.Y);
                                Point nextSlot = invenComp.SelectedSlot + direction;
                                if (nextSlot.Y >= 0
                                 && nextSlot.X < invenComp.ColumnsRows.Y
                                 && nextSlot.Y < invenComp.ColumnsRows.X)
                                {
                                    if(equipComp != null && nextSlot.X >= -3)
                                    {
                                        invenComp.SelectedSlot = nextSlot;
                                    }
                                    else if (nextSlot.X >= 0)
                                    {
                                        invenComp.SelectedSlot = nextSlot;
                                    }
                                    invenComp.selectSlotCurCooldown = invenComp.SelectSlotDelay;
                                }
                            }
                            if (playerComp.Interact.IsButtonDown())
                            {
                                //calculate the location of the selected slot in the inventory items array
                                int selectedArraySlot = invenComp.SelectedSlot.Y + (invenComp.ColumnsRows.X) * invenComp.SelectedSlot.X;
                                if (invenComp.HeldItem == 0)
                                {
                                    if (equipComp != null && invenComp.SelectedSlot.X < 0)
                                    {
                                        //Weapon pos
                                        if (invenComp.SelectedSlot.X == -1)
                                        {
                                            if (AddItemToInventory(entity.Key, equipComp.Weapon))
                                                equipComp.Weapon = 0;
                                        }
                                        //body pos
                                        if (invenComp.SelectedSlot.X == -2)
                                        {
                                            if (AddItemToInventory(entity.Key, equipComp.Body))
                                                equipComp.Body = 0;
                                        }
                                        //head pos
                                        if (invenComp.SelectedSlot.X == -3)
                                        {
                                            if(AddItemToInventory(entity.Key, equipComp.Head))
                                                equipComp.Head = 0;
                                        }
                                    }else
                                        //Picked an item to hold
                                        invenComp.HeldItem = invenComp.Items[selectedArraySlot];
                                    //Debug.WriteLine(invenComp.HeldItem + "   " + (invenComp.SelectedSlot.Y + (invenComp.ColumnsRows.X) * invenComp.SelectedSlot.X));
                                }else
                                {
                                    ItemComponent heldItemComp = cm.GetComponentForEntity<ItemComponent>(invenComp.HeldItem);
                                    if (equipComp != null && invenComp.SelectedSlot.X < 0)
                                    {
                                        //Weapon pos
                                        if (invenComp.SelectedSlot.X == -1)
                                        {
                                            if(equipComp.Weapon == 0)
                                            {
                                                equipComp.Weapon = invenComp.HeldItem;
                                                invenComp.Items[heldItemComp.InventoryPosition] = 0;
                                                heldItemComp.InventoryPosition = -1;
                                            }
                                            else
                                            {
                                                int equipToSwap = equipComp.Weapon;
                                                equipComp.Weapon = invenComp.HeldItem;
                                                invenComp.Items[heldItemComp.InventoryPosition] = equipToSwap;
                                                cm.GetComponentForEntity<ItemComponent>(equipToSwap).InventoryPosition = heldItemComp.InventoryPosition;
                                                heldItemComp.InventoryPosition = -1;
                                            }
                                        }
                                        //body pos
                                        else if (invenComp.SelectedSlot.X == -2)
                                        {
                                            if (equipComp.Body == 0)
                                            {
                                                equipComp.Body = invenComp.HeldItem;
                                                invenComp.Items[heldItemComp.InventoryPosition] = 0;
                                                heldItemComp.InventoryPosition = -2;
                                            }
                                            else
                                            {
                                                int equipToSwap = equipComp.Weapon;
                                                equipComp.Weapon = invenComp.HeldItem;
                                                invenComp.Items[heldItemComp.InventoryPosition] = equipToSwap;
                                                cm.GetComponentForEntity<ItemComponent>(equipToSwap).InventoryPosition = heldItemComp.InventoryPosition;
                                                heldItemComp.InventoryPosition = -2;
                                            }
                                        }
                                        //head pos
                                        else if (invenComp.SelectedSlot.X == -3)
                                        {
                                            if (equipComp.Head == 0)
                                            {
                                                equipComp.Head = invenComp.HeldItem;
                                                invenComp.Items[heldItemComp.InventoryPosition] = 0;
                                                heldItemComp.InventoryPosition = -3;
                                            }
                                            else
                                            {
                                                int equipToSwap = equipComp.Weapon;
                                                equipComp.Weapon = invenComp.HeldItem;
                                                invenComp.Items[heldItemComp.InventoryPosition] = equipToSwap;
                                                cm.GetComponentForEntity<ItemComponent>(equipToSwap).InventoryPosition = heldItemComp.InventoryPosition;
                                                heldItemComp.InventoryPosition = -3;
                                            }
                                        }
                                    }
                                    else if (invenComp.Items[selectedArraySlot] == 0)
                                    {
                                        //Moved held item
                                        invenComp.Items[selectedArraySlot] = invenComp.HeldItem;
                                        invenComp.Items[heldItemComp.InventoryPosition] = 0;
                                        heldItemComp.InventoryPosition = selectedArraySlot;
                                    }
                                    else
                                    {
                                        //Swap item locations
                                        int itemToSwap = 0;
                                        itemToSwap = invenComp.Items[selectedArraySlot];
                                        invenComp.Items[selectedArraySlot] = invenComp.HeldItem;
                                        invenComp.Items[heldItemComp.InventoryPosition] = itemToSwap;
                                        cm.GetComponentForEntity<ItemComponent>(itemToSwap).InventoryPosition = heldItemComp.InventoryPosition;
                                        heldItemComp.InventoryPosition = selectedArraySlot;
                                    }
                                    invenComp.HeldItem = 0;
                                }
                            }
                        }
                        else
                            invenComp.selectSlotCurCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
            }
        }

        public bool AddItemToInventory(int player, int item)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            ItemComponent itemComp = cm.GetComponentForEntity<ItemComponent>(item);
            if (itemComp == null)
            {
                //Debug.WriteLine("Trying to add an entity to a players inventory which does not have an ItemComponent or does not exist");
                return false;
            }
            InventoryComponent invenComp = cm.GetComponentForEntity<InventoryComponent>(player);
            for(int invSpot = 0; invSpot < invenComp.Items.Length; invSpot++)
            {
                if (invenComp.Items[invSpot] == 0)
                {
                    invenComp.Items[invSpot] = item;
                    itemComp.InventoryPosition = invSpot;
                    return true;
                }
            }
            //inventory is full
            return false;
        }
    }
}
