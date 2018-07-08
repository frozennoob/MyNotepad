using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MyNotepad
{
    //Класс экземпляры которого реализуют наши записи  
    public class Note : IComparable<Note>  //Будем  сортировать его в списке по ToString, который переопределен ниже
    {
        //Открываем поля, так как этого требует NHibernate
        public virtual int id { get; set; }
        public virtual string lastName { get; set; }
        public virtual string firstName { get; set; }
        public virtual string fathersName { get; set; }
        public virtual string phoneNumber { get; set; }
        public virtual string email { get; set; }
        public virtual DateTime birthDate { get; set; }

        // Конструктор по умолчанию для совместимости с NHibernate
        public Note()
        { }

        //Пустая запись-Singleton для нашего списка
        private static Note Emptynote = new Note("<...>", "", "", "", "", new DateTime());

        public static Note GetEmpty()
        {
            return Emptynote;
        }

        //Для создания экзмпляров используюем фабрику, так-как хотим проверить поля на валидность до того, как размещать их с помощью new
        public static Note CreateInstance(string lastName, string firstName, string fathersName, string phoneNumber, string email, DateTime birthDate, ref Label errorOut)
        {
            //Проверяем переданные аргументы на валидность.
            //В случае неудачи выводим текст ошибки в переданную в аргументе метода Label errorOut
            try
            {
                ValidateName(lastName);
                ValidateName(firstName);
                ValidateName(fathersName);
                ValidatePhone(phoneNumber);
                ValidateEmail(email);
                ValidateDate(birthDate);
                errorOut.Content = "";
            }
            catch (ArgumentOutOfRangeException e)
            {
                errorOut.Content = e.Message;
                return null;
            }
            return new Note(lastName, firstName, fathersName, phoneNumber, email, birthDate);
        }

        // Функции проверки допустимости значений полей класса
        private static void ValidateName(string name)
        {
            if (!Regex.IsMatch(name, @"^$|^[a-zA-Zа-яА-ЯЁё]+$"))
                throw new ArgumentOutOfRangeException(name);
        }
        private static void ValidatePhone(string name)
        {
            string pattern = @"^$|^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
            if (!Regex.IsMatch(name, pattern))
                throw new ArgumentOutOfRangeException(name);
        }
        private static void ValidateEmail(string name)
        {
            string pattern = @"^$|^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (!Regex.IsMatch(name, pattern))
                throw new ArgumentOutOfRangeException(name);
        }
        private static void ValidateDate(DateTime date)
        {
            if (date.ToShortDateString() == "")
                return;
            if (date > DateTime.Today)
                throw new ArgumentOutOfRangeException(date.ToShortDateString());
        }

        //Закрытый конструктор от всех - шести параметров
        private Note(string lastName, string firstName, string fathersName, string phoneNumber, string email, DateTime birthDate)
        {
            this.lastName = UpdateString(lastName);
            this.firstName = UpdateString(firstName);
            this.fathersName = UpdateString(fathersName);
            this.birthDate = birthDate;
            this.phoneNumber = phoneNumber;
            this.email = email;
        }

        //Этот метод позволяет скопировать в уже существующую запись данные другой записи.
        //Он нам нужен потому что мы не хотим давать доступ к полям на прямую, так-как им нужна валидация
        public virtual void Clone(Note other)
        {
            this.lastName = other.lastName;
            this.firstName = other.firstName;
            this.fathersName = other.fathersName;
            this.birthDate = other.birthDate;
            this.phoneNumber = other.phoneNumber;
            this.email = other.email;
        }

        // Форматируем строку , первая буква - заглавная , остальные - строчные
        private string UpdateString(string str)
        {
            if (str.Length < 1)
                return str;
            return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1).ToLower();
        }

        public override string ToString()
        {
            return lastName + " " + firstName + " " + fathersName;
        }

        public virtual int CompareTo(Note other)
        {
            if (other == null)
                return -1;
            return ToString().CompareTo(other.ToString());
        }

    }
}
