﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intuit.Players.Models.Interfaces
{
    public interface IStartupTask
    {
        Task Execute();
    }
}
