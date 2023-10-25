using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using udemy_server.Controllers;

namespace udemy_server.Models.Entities
{
    public class BackgroundTaskManager
    {
        private Timer timer;
        private readonly UdemyController udemyController;

        public BackgroundTaskManager(UdemyController controller)
        {
            udemyController = controller;
            timer = new Timer(RefreshDataCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
        }

        private void RefreshDataCallback(object state)
        {
            udemyController.RefreshData();
        }
    }
}