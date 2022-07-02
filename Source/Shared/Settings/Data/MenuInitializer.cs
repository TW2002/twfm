using Settings.Models;
using System;
using System.Linq;

namespace Settings.Data;

public static class MenuInitializer
{
    public static void Initialize(MenuContext context)
    {
        context.Database.EnsureCreated();

        // Return if there are any existing menus.
        // Database has already been seeded
        if (context.Menus.Any()) return;

        Guid proxyGuid = Guid.NewGuid();
        Guid datbaseGuid = Guid.NewGuid();

        context.Menus.Add(new Menu()
        {
            MenuID = proxyGuid,
            ParentID = null,
            Key = "@",
            Caption = "Proxy Menu",
            Prompt = "Proxy Menu - <?=Help> [Q} : ",
            Help = ""
        };

        context.Menus.Add(new Menu()
        {
            MenuID = datbaseGuid,
            ParentID = proxyGuid,
            Key = "D",
            Caption = "Database Menu",
            Prompt = "Database Menu - <?=Help> [Q} : ",
            Help = ""
        };

        context.SaveChanges();

        context.MenuItems.Add(new MenuItem()
        {
            MenuItemID = Guid.NewGuid,
            ParentID = proxyGuid,
            Key = "?",
            Caption = "Help",
            Help = "Display help for this menu."
        };

        context.MenuItems.Add(new MenuItem()
        {
            MenuItemID = Guid.NewGuid,
            ParentID = proxyGuid,
            Key = "q",
            Caption = "Quit",
            Help = "Quit this menu and return to terminal."
        };


        context.SaveChanges();
    }
}