
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppBZork.Entities;


namespace GruppBZork
{
    class Program
    {
        Player player = new Player("Börje KrachDumi");
        static Room currentRoom;

        public static void Go(Exit destination)
        {
            foreach (var exit in currentRoom.listOfExits)
            {
                if (exit == destination)
                {
                    currentRoom = exit.GoThrough(currentRoom);
                }
            }
        }

        /// <summary>
        /// Metoden är kanske inte den effektivaste med fungerar såhär! 
        /// två lokala instanser av Item skapas. itemOne och itemTwo
        /// User input splittas med hjälp av .Split(' ') till en array av strings.
        /// Exempel: > USE KEY ON LOCKEDDOOR. 
        /// Loopar igenom listan currentRoom.listOfItems om ett objekt matchar 4 ordet(LOCKED DOOR).
        /// På samma sätt loopar man igenom inventory men med andra ordet från input.
        /// Objekten som man hittar kopierar man till en an de lokala instanserna. 
        /// 
        /// Sedan kollar man if (itemOne.Name == itemTwo.PairWith1) 
        /// </summary>
        public static void UseItemOnItem(string userinput)
        {
            Item itemOne = new Item("", "", true);
            Item itemTwo = new Item("", "", true);
            userinput = userinput.ToUpper();
            var sb = userinput.Split(' ');
            if (sb.Length == 4)
            {
                foreach (var item in currentRoom.listOfItems)
                {
                    if (item.Name == sb[3].ToString())
                    {
                        itemTwo = (Item)item;

                        foreach (var item2 in Player.inventory)
                        {
                            if (item2.Name == sb[1].ToString())
                            {
                                itemOne = (Item)item2;
                            }
                        }
                    }
                }
                if (itemOne.Name == itemTwo.pairUpWith1)
                { Console.WriteLine($"Du låste upp: {itemTwo.Name} med {itemOne.Name}"); return; }
                else { }
            }
        }

        /// <summary>
        /// ShowInventory() Loopar igenom Player.inventory och skriver ut vad som finns där
        /// </summary>
        public static void ShowInventory()
        {
            Console.Write($"You have: ");
            bool noItemsInList = true;
            foreach (var item in Player.inventory)
            {
                if (Player.inventory.Count > 0)
                {
                    Console.Write($"{item.Name} ");
                    noItemsInList = false;
                }
            }
            if (noItemsInList == true)
            { Console.WriteLine($"Zero items in your inventory!"); }
            else
            { Console.WriteLine($"in your inventory!"); }
        }

        /// <summary>PickUpItem(string userInput, Room currentRoom)       
        /// Metoden används för att plocka upp ett objekt. 
        /// Inparametrarna är här:
        /// <param name="userInput"></param>
        /// <param name="currentRoom"></param>
        /// Ifall objeket finns i rummet och "canBetakeable" så flyttas objeketet från 
        /// currentRoom.itemList ===> Player.inventory
        /// </summary>
        public static void PickUpItem(string userInput, Room currentRoom)
        {
            foreach (var item in currentRoom.listOfItems)
            {
                if (item.CanBeTaken == true && item.Name == userInput.ToUpper())
                {
                    Console.WriteLine($"You picked up {item.Name}!");
                    Player.inventory.Add(item);
                    currentRoom.listOfItems.Remove(item);
                    return;
                }
                else if (item.CanBeTaken == false && item.Name == userInput.ToUpper())
                {
                    Console.WriteLine($"You cannot pick up {item.Name}!");
                }
                else
                {
                    Console.WriteLine($"There is no {userInput} in this room");
                }
            }
        }

        /// <summary>DropItem(string userInput, Room currentRoom)       
        /// Metoden används för att droppa ett objekt. 
        /// Inparametrarna är här:
        /// <param name="userInput"></param>
        /// <param name="currentRoom"></param>
        /// Ifall objeket finns i Player.inventory så flyttas objeketet från 
        /// Player.inventory ===> currentRoom.itemList 
        /// </summary>
        public static void DropItem(string userInput, Room currentRoom)
        {
            foreach (var item in Player.inventory)
            {
                if (item.Name == userInput.ToUpper())
                {
                    Console.WriteLine($"You droped {item.Name}!");
                    currentRoom.listOfItems.Add(item);
                    Player.inventory.Remove(item);
                    return;
                }
                else
                {
                    Console.WriteLine($"There is no {userInput} in your inventory");
                }

            }
        }




        static void Main(string[] args)
        {
            ///Ändrade namnen på testrummen och object för att lättare skilja på dem

            //Room 1
            Room blueRoom = new Room(name: "BLUEROOM", description: "Welcome to the blue room");

            //Room 2
            Room redRoom = new Room(name: "REDRROOM", description: "Welcome to the red room");

            //Items
            blueRoom.listOfItems.Add(new Item(name: "KEY", description: "This is a test key", canBeTaken: true) { pairUpWith1 = "LOCKEDDOOR" });
            blueRoom.listOfItems.Add(new Item(name: "LOCKEDDOOR", description: "This is a test key", canBeTaken: true) { pairUpWith1 = "KEY" });
            blueRoom.listOfItems.Add(new Item(name: "SOFA", description: "This is a test sofa", canBeTaken: false));

            //Exits
            blueRoom.listOfExits.Add(new Exit(name: "REDDOOR", description: "Denna dörr leder ingenstans", locked: true) { room1 = blueRoom, room2 = redRoom });
            redRoom.listOfExits.Add(new Exit(name: "BLUEDOOR", description: "Du står inne i ", locked: false) { room1 = blueRoom, room2 = redRoom });





            while (true)
            {
                currentRoom = blueRoom;
                currentRoom.DescribeRoom();

                string input = Console.ReadLine().ToUpper();

                if (input == "GO")
                {
                    Go(currentRoom.listOfExits[0]);
                }
                if (input == "PICK KEY" || input == "PICK SOFA")///Testar PickUpItem och dropItem
                {
                    if (input == "PICK KEY")
                    { input = "KEY"; }
                    else { input = "SOFA"; }

                    foreach (var item in currentRoom.listOfItems)
                    {
                        if (item.Name == input)
                        { PickUpItem(input, currentRoom); break; }
                    }

                    ShowInventory();
                }
                if (input == "DROP KEY")    ///Testar DropIte´m
                {
                    input = "KEY";

                    foreach (var item in Player.inventory)
                    {
                        if (item.Name == "KEY")
                        { DropItem(input, currentRoom); break; }
                    }

                    ShowInventory();
                }
                if (input == "USE KEY ON LOCKEDDOOR")
                {
                    UseItemOnItem(input);
                }
            }
        }
    }
}

