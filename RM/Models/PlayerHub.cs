using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace RM.Models
{
    public class PlayerHub:Hub
    {
        public  void play()
        {
            SendName();
                Case.button = "play";
                Case.flag = true;
         
        }
        public void next()
        {
            SendName();
            Case.button = "next";
            Case.flag = true;

        }
        public void prev()
        {
            SendName();
            Case.button = "previous";
            Case.flag = true;

        }

        public async void SendName()
        {
            await Clients.All.SendAsync("SendNa", Case.name);
        }

    }
}
