﻿using ADM_Scada.Core.Models;
using Prism.Events;
namespace ADM_Scada.Cores.PubEvent
{
    public class ShiftInfoChangeEvent : PubSubEvent<ProdShiftDataModel>
    {
    }
    public class NewBagEvent : PubSubEvent
    {
    }
    public class EndSessionCommandEvent : PubSubEvent
    {
    }
}
