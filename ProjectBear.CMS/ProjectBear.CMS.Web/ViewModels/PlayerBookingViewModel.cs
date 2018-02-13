using ProjectBear.Data;
using System;
using System.Collections.Generic;

namespace ProjectBear.CMS.ViewModels
{
    public class PlayerBookingViewModel
    {
        public DateTime Date { get; set; }
        public string Game { get; set; }
        public string PlayerName { get; set; }  
        
        public bool DidNotPitch { get; set; }
    }
}