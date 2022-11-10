using Configuration.Models;
using System;
using System.Linq;

namespace Configuration.Data;

public static class CreateDatabase
{
    public static ConfigContext Initialize()
    {
        ConfigContext context = new();

        context.Database.EnsureCreated();

        // Return if there are any existing menus.
        // Database has already been seeded
        if (context.Menus.Any()) return context;

        Guid proxyGuid = Guid.NewGuid();
        Guid databaseGuid = Guid.NewGuid();

        context.Menus.Add(new Menu()
        {
            MenuID = proxyGuid,
            ParentID = null,
            Name = "ProcyMenu",
            Key = "@",
            Caption = "Proxy Menu",
            Prompt = "Proxy Menu - <?=Help> [Q} : ",
            Help = ""
        });

        context.Menus.Add(new Menu()
        {
            MenuID = databaseGuid,
            ParentID = proxyGuid,
            Name = "DatabaseMenu",
            Key = "D",
            Caption = "Database Menu",
            Prompt = "Database Menu - <?=Help> [Q} : ",
            Help = ""
        });

        context.SaveChanges();

        context.MenuItems.Add(new MenuItem()
        {
            MenuItemID = Guid.NewGuid(),
            ParentID = proxyGuid,
            Name = "ProxyHelp",
            Key = "?",
            Caption = "Help",
            Command = "CommandHelp",
            Parameters = "ProxyMenu",
            Help = "Display help for this menu."
        });

        context.MenuItems.Add(new MenuItem()
        {
            MenuItemID = Guid.NewGuid(),
            ParentID = proxyGuid,
            Name = "ProxyQuit",
            Key = "q",
            Caption = "Quit",
            Command = "CommandQuit",
            Help = "Quit this menu and return to terminal."
        });

        context.MenuItems.Add(new MenuItem()
        {
            MenuItemID = Guid.NewGuid(),
            ParentID = databaseGuid,
            Name = "DatabaseQuit",
            Key = "q",
            Caption = "Quit database menu",
            Command = "CommandQuit",
            Help = "Quit this menu and return to proxy menu."
        });


        context.SaveChanges();
        return context;
    }
}