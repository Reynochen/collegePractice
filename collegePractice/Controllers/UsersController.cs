using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using collegePractice.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace collegePractice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        ApplicationContext db = new ApplicationContext(); //База данных

        private readonly ILogger<UsersController> _logger;
        public UsersController(ILogger<UsersController> logger) 
        {
            _logger= logger;
        }

        [HttpGet]
        public string Get()
        {
            int size = db.Users.ToArray().Length;
            string tabName = "", tabEmail = "";
            //Чтение данных с бд
            User[] users = new User[size];
            string table = string.Format("{0,-12}   {1,-32}    {2,6}", "Имя"," Email"," Время" + '\n');

            //Инверсия массива
            for (int i = size - 1; i >= 0; i--)
                users[size - 1 - i] = db.Users.ToArray()[i];

            
            //Создание таблицы
            for (int i = 0; i < size - 1; i++)
            {
                //table += users[i].Name + "\t\t\t|" + users[i].email + "\t\t|" + users[i].time + '\n';
                table += string.Format("{0,-12}   |{1,-32}    |{2,5}", users[i].Name, users[i].email, users[i].time) + '\n';
            }

            if (db.Users.ToArray().Length - 1 == 0) table += "Нет зарегистрированных пользователей.";

            return table;
            
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public string Post(string name, string email)
        {
            if (name.Length > 12 || email.Length > 32) return "Максимальный размер имени 12 символов/почты 32.";
            User user = new User { Name = name, email = email };
            
            int timeFirst = Random.Shared.Next(3, 12);
            int timeSecond = Random.Shared.Next(0, 59);
            string time = (timeFirst > 9 ? timeFirst.ToString() : "0"+timeFirst.ToString()) + ":" + (timeSecond > 9 ? timeSecond.ToString() : "0"+timeSecond.ToString());
            user.time = time;

            string cond = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";

            try
            {
                var addr = new System.Net.Mail.MailAddress(user.email);
                User[] users = db.Users.ToArray();
                for (int i = 0; i < users.Length; i++)
                    if (users[i].email == email) return "Данная почта уже существует.";

                if (addr.Address == user.email && Regex.IsMatch(user.email, cond))
                {
                    //Сохранение данных в бд
                    db.Users.AddRange(user);
                    db.SaveChanges();

                    return "Я бегаю по утрам! В " + time;
                }
                else return "Введите корректный адрес электронной почты.";
            }
            catch
            {
                return "Введите корректный адрес электронной почты.";
            }

            return "";
            
        }


        [HttpDelete]
        public void Delete(string name)
        {
            try
            {
                //Удаление данных с бд
                User user = new User { Name = name };
                db.Attach(user);
                db.Remove(user);
                db.SaveChanges();
            }
            catch
            {

            }
        }
    }
}
