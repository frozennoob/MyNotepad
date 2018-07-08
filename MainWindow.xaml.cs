using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Configuration;

namespace MyNotepad
{
    public partial class MainWindow : Window
    {
        List<Note> items = new List<Note>(); // Этот список будет хранить в себе наши записи

        public MainWindow()
        {
            InitializeComponent();
            items.Add(Note.GetEmpty()); //Создаем пустую запись , она будет служить как для добавления новых записей , так и просто для удобства и красоты.
            // Установка соединения с БД и загрузка данных в программу
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    items.AddRange(session.QueryOver<Note>().List());
                    transaction.Commit();
                }
            }
            notesListBox.ItemsSource = items; // Инициализируем ListBox нашим списком
            notesListBox.SelectedIndex = 0; 
        }

        //В случае изменения выделенной записи , меняем содержимое соответствующих полей на форме.
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Note)notesListBox.SelectedItem) == Note.GetEmpty())
                textBoxLastName.Text = "";
            else
                textBoxLastName.Text = ((Note)notesListBox.SelectedItem).lastName;
            textBoxFirstName.Text = ((Note)notesListBox.SelectedItem).firstName;
            textBoxFathersName.Text = ((Note)notesListBox.SelectedItem).fathersName;
            textBoxPhone.Text = ((Note)notesListBox.SelectedItem).phoneNumber;
            textBoxMail.Text = ((Note)notesListBox.SelectedItem).email;
            if (((Note)notesListBox.SelectedItem).birthDate == DateTime.MinValue)
                datePicker.Text = "";
            else
                datePicker.Text = ((Note)notesListBox.SelectedItem).birthDate.ToString().Substring(0, 10);
        }

        // Создаем новую запись перемещая фокус на пустое поле в списке
        private void NewClicked(object sender, RoutedEventArgs e)
        {
            notesListBox.SelectedIndex = 0;
        }
        
        // Сохранение записи в наш список items и в базу данных
        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            //Фамилия единственное обязательное не пустое поле. Проверяем ввел ли ее пользователь
            if (textBoxLastName.Text.Length < 1)
            {
                StatusLabel.Content = "Укажите фамилию";
                return;
            }

            //Соединямся с БД
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Note note = Note.CreateInstance(textBoxLastName.Text, textBoxFirstName.Text, textBoxFathersName.Text,
                    textBoxPhone.Text, textBoxMail.Text, datePicker.DisplayDate, ref StatusLabel);
                    if (notesListBox.SelectedIndex == 0)
                    {
                        items.Add(note);
                        session.Save(note);
                        notesListBox.SelectedItem = note;
                    }
                    else
                    {
                        ((Note)notesListBox.SelectedItem).Clone(note);
                        session.Get<Note>(((Note)notesListBox.SelectedItem).id).Clone(note);
                    }
                    transaction.Commit();         
                }
            }
            items.Sort();
            notesListBox.Items.Refresh();
        }

        //Удаляем выделенную запись.
        private void DeleteClicked(object sender, RoutedEventArgs e)
        {
            //Нашу пустую запись удалить нельзя. Проверяем она ли это.
            if (notesListBox.SelectedIndex == 0)
                return;
            //Запрос подтверждения
            if (MessageBox.Show(this, "Вы уверены, что хотите удалить выбранный элемент?" + this.Name.ToString(), "Удалить?", 
                MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel) == MessageBoxResult.Cancel)
                return;
            //Подключение к БД и удаление
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    var note = session.Load<Note>(((Note)notesListBox.SelectedItem).id);
                    session.Delete(note);
                    transction.Commit();
                }
            }      
            items.Remove((Note)notesListBox.SelectedItem);
            notesListBox.SelectedIndex = 0;
            notesListBox.Items.Refresh();
        }
    }
}
