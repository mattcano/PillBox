using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class EmailMedSummaryItem
    {
        List<Reminder> _reminders;

        public string Name
        {
            get
            {
                return _reminders.ElementAt(0).UserMedicineMap.Medicine.Name;
            }
        }
        //public int NumOfDaysInARow
        //{
        //    get
        //    {
        //        //TODO write logic to return number of days in a row

        //        //int numConsecutiveDaysInARow = 0;

        //        //foreach (var reminder in _reminders.Sort(r => r.)
        //        //{

        //        //}

        //        //return 0;
        //    }
        //}

        public List<string> DatesMissedList
        {
            get
            {
                return new List<string>();
            }
        }

        public int PointsEarnedThisWeek
        {
            get
            {
                // TODO write logic to return points earned this week
                return 0;
            }
        }


        public int DosesMissedThisWeek
        {
            get
            {
                return 0;
            }
        }



    }
}