﻿using ADM_Scada.Core.Models;
using Prism.Events;

namespace ADM_Scada.Cores.PubEvent
{
    public class VariableValueChangeEvent : PubSubEvent<VariableModel>
    {
    }
}
