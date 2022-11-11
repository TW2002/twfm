using Configuration.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Configuration.Data;

public static class DatabaseMnager 
{
    public static ConfigContext LoadConfig(string mode)
    {
        ConfigContext config = new();

        config.Version = "v22.11a";
        config.Banner = $"FirstMate {mode} {config.Version} - Ready for Combat!\n\r" +
                        $"Copyright (c) {DateTime.Now.Year} David McCartney, All rights reserved.\n\r";


        config.Database.EnsureCreated();

        // Return if there are any existing menus.
        // Database has already been seeded
        if (config.Menus.Any()) return config;

        Guid proxyGuid = Guid.NewGuid();
        Guid databaseGuid = Guid.NewGuid();

        config.Menus.Add(new Menu()
        {
            MenuID = proxyGuid,
            ParentID = null,
            Name = "ProcyMenu",
            Key = "@",
            Caption = "Proxy Menu",
            Prompt = "Proxy Menu - <?=Help> [Q} : ",
            Help = ""
        });

        config.Menus.Add(new Menu()
        {
            MenuID = databaseGuid,
            ParentID = proxyGuid,
            Name = "DatabaseMenu",
            Key = "D",
            Caption = "Database Menu",
            Prompt = "Database Menu - <?=Help> [Q} : ",
            Help = ""
        });

        config.SaveChanges();

        config.MenuItems.Add(new MenuItem()
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

        config.MenuItems.Add(new MenuItem()
        {
            MenuItemID = Guid.NewGuid(),
            ParentID = proxyGuid,
            Name = "ProxyQuit",
            Key = "q",
            Caption = "Quit",
            Command = "CommandQuit",
            Help = "Quit this menu and return to terminal."
        });

        config.MenuItems.Add(new MenuItem()
        {
            MenuItemID = Guid.NewGuid(),
            ParentID = databaseGuid,
            Name = "DatabaseQuit",
            Key = "q",
            Caption = "Quit database menu",
            Command = "CommandQuit",
            Help = "Quit this menu and return to proxy menu."
        });


        config.SaveChanges();
        return config;
    }
}