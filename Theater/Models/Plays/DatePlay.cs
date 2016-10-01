using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Theater.Models.Plays
{
    public class DatePlay
    {
        public int Id { get; set; }
        public int PlayId { get; set; }        
        public DateTime Date { get; set; }

        public DatePlay()
        {
            Date = new DateTime();
        }

        public DatePlay(int id, int playId, DateTime date)
        {
            Id = id;
            PlayId = playId;
            Date = date;
        }       
    }
}