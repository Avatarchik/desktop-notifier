﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace desktop_notifier
{
    interface DesktopNotifierInterface
    {
        void ShowNotification(Message message, int timeout);
    }
}