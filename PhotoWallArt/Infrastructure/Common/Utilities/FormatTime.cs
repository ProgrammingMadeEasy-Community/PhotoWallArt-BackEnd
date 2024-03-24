using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambaBackEnd.Application.Common.Utilities;
public static class FormatTime
{
    // Method to format TimeSpan to a readable "time ago" format
   public static string FormatTimeSpan(DateTime pastDateTime)
    {

        // Calculate the time difference
        TimeSpan timeSpan = DateTime.UtcNow - pastDateTime;

        if (timeSpan.Days > 365)
        {
            int years = timeSpan.Days / 365;
            return $"{years} year{(years > 1 ? "s" : "")} ago";
        }
        if (timeSpan.Days > 30)
        {
            int months = timeSpan.Days / 30;
            return $"{months} month{(months > 1 ? "s" : "")} ago";
        }
        if (timeSpan.Days > 0)
        {
            return $"{timeSpan.Days} day{(timeSpan.Days > 1 ? "s" : "")} ago";
        }
        if (timeSpan.Hours > 0)
        {
            return $"{timeSpan.Hours} hour{(timeSpan.Hours > 1 ? "s" : "")} ago";
        }
        if (timeSpan.Minutes > 0)
        {
            return $"{timeSpan.Minutes} minute{(timeSpan.Minutes > 1 ? "s" : "")} ago";
        }

        return "Just now";
    }
}
