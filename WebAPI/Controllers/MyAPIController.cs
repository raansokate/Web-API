using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Net;
using System.Xml.Linq;
using WebAPI.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyAPIController : ControllerBase
    {

        [HttpGet("View")]
        public async Task<ActionResult<List<Appointment>>> Get()
        {
            using (var _Entity = new AppointmentdbContext())
            {
                return await _Entity.Appointments.ToListAsync();
            }

        }

        [HttpPost("Create")]
        public async Task<string> CreateRecord(Appointment model)
        {

            using (var _Entity = new AppointmentdbContext())
            {
                try
                {
                    TimeSpan start = new TimeSpan(10, 0, 0); 
                    TimeSpan end = new TimeSpan(23, 0, 0); 
                    
                    var IsCorrectTime = IsBetween(Convert.ToDateTime(model.Date), start, end);

                    if (IsCorrectTime == false)
                    {
                        return "Appointment time must be between 10:00 AM and 3:00 PM";
                    }
                    var patientChck = _Entity.Appointments.AsNoTracking().Where(x => x.PatientName == model.PatientName && x.Date.Value.Date == model.Date.Value.Date).FirstOrDefault();
                    if (patientChck != null)
                    {
                        return model.PatientName + " has already booked an appointment with doctor." + patientChck.DoctorName + " for entered date.";
                    }
                    var doclst = _Entity.Appointments.AsNoTracking().Where(x => x.DoctorName == model.DoctorName && x.Date.Value.Date == model.Date.Value.Date).ToList();
                    if (doclst.Count >= 5)
                    {
                        return model.DoctorName + " has already having 5 appointments for entered date.";
                    }

                    model.Time = model.Date.Value.TimeOfDay.ToString();
                    _Entity.Entry(model).State = EntityState.Added;
                    _Entity.SaveChanges();
                }
                catch (Exception ex)
                {
                    return "An error occured while Creating appointment.";
                }
            }
            return "Appointment Created.";
        }

        [HttpDelete("Delete/{booking_id}")]
        public async Task<string> DeleteRecord(int booking_id)
        {
            using (var _Entity = new AppointmentdbContext())
            {
                var model = _Entity.Appointments.AsNoTracking().Where(x => x.Id == booking_id).FirstOrDefault();
                if (model != null)
                {
                    _Entity.Entry(model).State = EntityState.Deleted;
                    _Entity.SaveChanges();
                }
                else
                {
                    return "Appointment not found.";
                }
            }
            return "Appointment has been deleted.";
        }

        public static bool IsBetween(DateTime now, TimeSpan start, TimeSpan end)
        {
            var time = now.TimeOfDay;
            // Scenario 1: If the start time and the end time are in the same day.
            if (start <= end)
                return time >= start && time <= end;
            // Scenario 2: The start time and end time is on different days.
            return time >= start || time <= end;
        }
    }
}
