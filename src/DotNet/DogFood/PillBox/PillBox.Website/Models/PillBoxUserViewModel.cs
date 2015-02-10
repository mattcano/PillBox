using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class PillBoxUserViewModel
    {
        private PillBoxUser user;
        private IEnumerable<MedicineRowViewModel> userPillBox;
        private IEnumerable<WeeklyProgressRowViewModel> weeklyProgress;
        private IEnumerable<PeopleICareForRowViewModel> peopleICareFor;
        private IEnumerable<string> peopleCheeringMeOn;
        private DateTime startOfWeek;
        private DateTime endOfWeek;

        public PillBoxUserViewModel(PillBoxUser user)
        {
            this.user = user;
            userPillBox = PopulateUserPillBox();
            weeklyProgress = PopulateWeeklyProgress();
            peopleICareFor = PopulatePeopleICareFor();
            peopleCheeringMeOn = PopulatePeopleCheeringMeOn();

            SetWeekString();
        }

        public PillBoxUser User { get { return user; } }
        public IEnumerable<MedicineRowViewModel> UserPillBox { get { return userPillBox; } }
        public IEnumerable<WeeklyProgressRowViewModel> WeeklyProgress { get { return weeklyProgress; } }
        public IEnumerable<PeopleICareForRowViewModel> PeopleICareFor { get { return peopleICareFor; } }
        public IEnumerable<string> PeopleCheeringMeOn { get { return peopleCheeringMeOn; } }

        public string StartOfWeek { get; set; }
        public string EndOfWeek { get; set; }

        private IEnumerable<MedicineRowViewModel> PopulateUserPillBox()
        {
            IList<MedicineRowViewModel> list = 
                new List<MedicineRowViewModel>();

            foreach (var med in user.Medicines)
            {
                list.Add(
                    new MedicineRowViewModel(med.Name, 
                        med.RemindTime.Value.ToShortTimeString(), 
                        med.Id));
            }
            
            return list;
        }

        private IEnumerable<WeeklyProgressRowViewModel> PopulateWeeklyProgress()
        {
            IList<WeeklyProgressRowViewModel> list = 
                new List<WeeklyProgressRowViewModel>();

            foreach(var med in user.Medicines)
            {
                //TODO Get Total Count for this week
                //Get hits from reminders for this week
                int hits = 0;

                var reminders = from r in user.Reminders
                                where r.MedicineId == med.Id &&
                                r.ReminderSendTime >= startOfWeek
                                select r;

                hits = reminders.Where(r => r.IsTaken.Value == true).Count();

                list.Add(new WeeklyProgressRowViewModel(med.Name, hits, 7));
            }

            return list;
        }

        private IEnumerable<PeopleICareForRowViewModel> PopulatePeopleICareFor()
        {
            IList<PeopleICareForRowViewModel> list =
                new List<PeopleICareForRowViewModel>();

            //TODO allow you to follow people

            return list;
        }

        private IEnumerable<string> PopulatePeopleCheeringMeOn()
        {
            IEnumerable<string> list = new List<string>();

            //TODO get the people following you

            return list;
        }

        private static void GetWeek(DateTime now, CultureInfo cultureInfo, out DateTime begining, out DateTime end)
        {
            if (now == null)
                throw new ArgumentNullException("now");
            if (cultureInfo == null)
                throw new ArgumentNullException("cultureInfo");

            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            int offset = firstDayOfWeek - now.DayOfWeek;
            if (offset != 1)
            {
                DateTime weekStart = now.AddDays(offset);
                DateTime endOfWeek = weekStart.AddDays(6);
                begining = weekStart;
                end = endOfWeek;
            }
            else
            {
                begining = now.AddDays(-6);
                end = now;
            }
        }


        private void SetWeekString()
        {
            DateTime start;
            DateTime end;

            GetWeek(DateTime.Now, CultureInfo.CurrentCulture, out start, out end);

            startOfWeek = start;
            endOfWeek = end;

            StartOfWeek = startOfWeek.ToShortDateString();
            EndOfWeek = endOfWeek.ToShortDateString();
        }

    }
}