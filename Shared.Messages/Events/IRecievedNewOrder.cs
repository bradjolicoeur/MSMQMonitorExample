﻿using Shared.Messages.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages.Events
{
    public interface IRecievedNewOrder : IOrderBase, IContainShippingCost
    {

    }
}
