using System;

namespace Outlook.Model.Helpers
{
    public static class DateHelper
    {
        #region Methods

        public static string GetDateDifferenceText(DateTime date)
        {
            string dateDifference;
            int numberOfDays = (int)(DateTime.Now - date).TotalDays;

            if (numberOfDays >= 0)
            {
                switch (numberOfDays)
                {
                    case 0:
                    case 1:
                        {
                            int numberOfHours = (int)(DateTime.Now - date).TotalHours;
                            switch (numberOfHours)
                            {
                                case 0:
                                    {
                                        int numberOfMins = (int)(DateTime.Now - date).TotalMinutes;
                                        switch (numberOfMins)
                                        {
                                            case 0:
                                                dateDifference = "Today";
                                                break;

                                            case 1:
                                                dateDifference = "a minute ago";
                                                break;

                                            default:
                                                dateDifference = string.Format("{0} minutes ago", numberOfMins);
                                                break;
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        dateDifference = "an hour ago";
                                        break;
                                    }
                                default:
                                    {
                                        dateDifference = string.Format("{0} hours ago", numberOfHours);
                                        break;
                                    }
                            }
                        }
                        break;

                    default:
                        dateDifference = string.Format("{0} days ago", numberOfDays);
                        break;
                }
            }
            else
            {
                dateDifference = "Today";
            }

            return dateDifference;
        }

        #endregion Methods
    }
}