﻿using ProjectBear.Data;
using System;
using System.Collections.Generic;

namespace ProjectBear.CMS.ViewModels
{
    public class RosterManagementViewModel
    {
        public RosterManagementViewModel()
        {
            TimeSlots = new List<TimeSlot>();
        }
        public Guid RosterId { get; set; }

        public DateTime Date { get; set; }

        public bool IsPublished { get; set; }

        public List<TimeSlot> TimeSlots { get; set; }

        public bool Edited { get; set; }

        public int TimeSlotCount => TimeSlots.Count;
    }
}