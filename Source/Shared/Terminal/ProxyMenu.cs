using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Configuration.Data;

namespace Terminal;

public class ProxyMenu
{
    private Guid? currentMenu = null;
    private Guid? lastMenu = null;
    private Guid? proxyMenu = null;
    private string? prompt;

    public NetworkStream? Stream { get; set; }

    private bool IsBlocking;
    public bool MyProperty
    {
        get { return currentMenu == null; }
    }


    public ProxyMenu() 
    {
    }

    public string ProcessCommands(NetworkStream stream, string s)
    {
        Stream = stream;

        StringBuilder sb = new();

        using var db = new ConfigContext();

        if (proxyMenu == null)
        {
            var menu = db.Menus.Where(m => m.Key == "@").First();
            if (menu != null)
            {
                proxyMenu = menu.MenuID;
            }
        }

        foreach (char c in s)
        {
            bool echoChar = true;

            char key = char.ToLower(c);

            var menus = db.Menus.Where(m => m.Key == char.ToString(key) && m.ParentID == currentMenu);
            if (menus.Count() > 0)
            {
                echoChar = false;

                var menu = menus.First();

                lastMenu = currentMenu;
                currentMenu = menu.MenuID;

                prompt = menu.Prompt;

                Write(stream, $"{char.ToString(c)}\n\r{prompt}");
            }

            var menuitems = db.MenuItems.Where(m => m.Key == char.ToString(c) && m.ParentID == currentMenu);
            if (menuitems.Count() > 0)
            {
                echoChar = false;

                var menuitem = menuitems.First();

                if (!string.IsNullOrEmpty(menuitem.Command))
                {
                    object[] parameters = new object[1];

                    if (!string.IsNullOrEmpty(menuitem.Parameters))
                    {
                        parameters[0] = menuitem.Parameters;
                    }

                    try
                    {
                        MethodInfo? method = GetType().GetMethod(menuitem.Command);
                        if(method != null)
                        {
                            if (method != null) method.Invoke(this, parameters);
                        }
                        else
                        {
                            Write(stream, $"\n\r\n\rThis Command has not been implimented.\n\r\n\r{prompt}");
                        }
                    }
                    catch (Exception e)
                    {
                        Write(stream, $"\n\r\n\r<=-ERRROR-=> An unexpected error has occured.\n\r{e.Message}\n\r");
                    }
                    //Write(stream, $"{char.ToString(c)}\n\r{menu.Prompt}");
                }
                else
                {
                    Write(stream, $"\n\r\n\rThis Command has not been defined.\n\r\n\r{prompt}");
                }
            }

            if (echoChar && currentMenu == null)
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    private void Write(NetworkStream stream, string? s)
    {
        byte[] requestNameData = Encoding.ASCII.GetBytes(s);
        stream.Write(requestNameData, 0, requestNameData.Length);
    }

    public void CommandQuit(object[] parameters)
    {
        using var db = new ConfigContext();

        if (Stream == null) return;

        if (lastMenu != null)
        {
            currentMenu = lastMenu;
            lastMenu = null;
        }
        else
        {
            if (currentMenu != proxyMenu)
            {
                currentMenu = proxyMenu;
            }
            else
            {
                currentMenu = null;
            }

            var menus = db.Menus.Where(m => m.MenuID == proxyMenu);
            if (menus.Count() > 0)
            {
                var menu = menus.First();

                Write(Stream, $"\n\r{menu.Prompt}\n\r");
            }
        }
    }
}