﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class People : IEquatable<People>
{
    public string Name { get; set; }

    public string Username { get; set; }

    public int Id { get; set; }

    public String Password { get; set; }

    private static List<People> peopleList = new List<People>();

    public override string ToString()
    {
        return $"| {Id,5} | {Name,25} | {Username,15} | {Password,30} |";
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        People objAsPeople = obj as People;
        if (objAsPeople == null) return false;
        else return Equals(objAsPeople);
    }

    public override int GetHashCode()
    {
        int hashName = Name == null ? 0 : Name.GetHashCode();

        int hashId = Name.GetHashCode();

        return hashName ^ hashId;
    }

    public bool Equals(People other)
    {
        if (Object.ReferenceEquals(other, null)) return false;

        if (Object.ReferenceEquals(this, other)) return true;

        return Id.Equals(other.Id) && Name.Equals(other.Name);
    }

    public void AddData(String choice)
    {
        String password = null;
        String firstName = null;
        String lastName = null;
        String userName = null;
        int id = 0;

        do
        {
            var test = peopleList.Count;
            var user = peopleList.Find(x => x.Username.ToLower().Equals(userName.ToLower()));

            if (test == 0)
            {
                do
                {
                    Console.Write("Input First Name: ");
                    firstName = Console.ReadLine();
                    Console.WriteLine();
                } while (!NameValidation(firstName) == true);

                do
                {
                    Console.Write("Input First Name: ");
                    lastName = Console.ReadLine();
                    Console.WriteLine();
                } while (!NameValidation(lastName) == true);

                id = 1;
                userName = firstName.Substring(0, 2) + lastName.Substring(0, 2);
            }
            else
            {
                var lastPerson = peopleList[^1];
                id = lastPerson.Id + 1;
                do
                {
                    do
                    {
                        Console.Write("Input First Name: ");
                        firstName = Console.ReadLine();
                        Console.WriteLine();
                    } while (!NameValidation(firstName) == true);

                    do
                    {
                        Console.Write("Input First Name: ");
                        lastName = Console.ReadLine();
                        Console.WriteLine();
                    } while (!NameValidation(lastName) == true);

                    userName = firstName.Substring(0, 2) + lastName.Substring(0, 2);

                    if (user.Username == userName)
                    {
                        Random random = new Random();
                        int num = random.Next(10,99);

                        userName = userName + num;
                        Console.Clear();
                    }
                } while (user.Username == userName);
            }

            do
            {
                Console.Write("Input Password: ");
                password = Console.ReadLine();
                Console.WriteLine();
            } while (!PasswordValidation(password) == true);

            String fullName = $"{firstName} {lastName}";
            //userName = firstName.Substring(0, 2) + lastName.Substring(0, 2);

            peopleList.Add(new People() { Id = id, Name = fullName, Username = userName.ToLower(), Password = BCrypt.Net.BCrypt.HashPassword(password) });

            Console.WriteLine();
            Console.Clear();
            Console.WriteLine($"{firstName} {lastName} telah ditambahkan");
            Console.WriteLine();
            Console.WriteLine("Tambah Produk lagi? (Y/N)");
            choice = Console.ReadLine();
            Console.Clear();

        } while (choice.ToUpper() == "Y");
    }

    public void ShowData()
    {
        Console.WriteLine($"{null,3} ID {null,5} {null,7}  Name {null,10} {null,3} Username {null,5} {null,25} Password {null,5} ");
        //Console.WriteLine($"{null,3} __ {null,5} {null,10}  ____ {null,10} {null,5} ________ {null,5} {null,8} ________ {null,5} ");
        Console.WriteLine();
        foreach (People person in peopleList)
        {
            Console.WriteLine($"{person}");
        }
        Console.WriteLine();
    }

    public void DeleteData(String choice)
    {
        do
        {
            Console.Write("Input ID yang akan dihapus: ");
            int id = Convert.ToInt32(Console.ReadLine());
            peopleList.RemoveAt(id - 1);
            Console.Clear();
            Console.WriteLine($"User dengan ID {id} berhasil dihapus");
            Console.WriteLine();
            Console.Write("Hapus data lagi? (Y/N)");
            choice = Console.ReadLine();
        } while (choice.ToUpper() == "Y");
    }

    public void SearchData(String batas)
    {
        String[] menu = new string[2] { "ID", "Name" };

        for (int i = 0; i < menu.Length; i++)
        {
            Console.Write($"{i + 1,40}. {menu[i]}");
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine(batas);
        Console.WriteLine();
        Console.Write("Masukkan pilihan: ");
        int index = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();
        switch (index)
        {
            case 1:
                {
                    Console.Write("Input Search Username: ");
                    String username = Console.ReadLine();
                    Console.WriteLine();
                    Console.WriteLine(batas);
                    Console.WriteLine();
                    Console.WriteLine($"{null,3} ID {null,5} {null,10}  Name {null,10} {null,5} Username {null,5} {null,8} Password {null,5}");
                    Console.WriteLine();
                    Console.WriteLine(peopleList.Find(x => x.Username.Equals(username)));
                    Console.WriteLine();
                    Console.WriteLine(batas);
                    break;
                }
            case 2:
                {
                    Console.Write("Input Search Name: ");
                    string name = Console.ReadLine();
                    Console.WriteLine();
                    Console.WriteLine(batas);
                    Console.WriteLine();
                    Console.WriteLine($"{null,3} ID {null,5} {null,10} Name {null,10} {null,5} Username {null,5} {null,8} Password {null,5} ");
                    Console.WriteLine();
                    foreach (var search in peopleList.Where(x => x.Name.ToLower().Contains(name.ToLower())))
                    {
                        Console.WriteLine(search);
                    }
                    Console.WriteLine();
                    Console.WriteLine(batas);
                    break;
                }
            default:
                Console.WriteLine("Input Salah");
                break;
        }
    }

    public void LoginPage()
    {
        try
        {
            Console.Write("Input Username: ");
            string username = Console.ReadLine();
            Console.Write("Input Password: ");
            string password = Console.ReadLine();

            Console.Clear();
            Console.WriteLine();

            var user = peopleList.Find(x => x.Username.ToLower().Equals(username.ToLower()));

            if (user.Username == username && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                Console.WriteLine("Login Berhasil");
            }
            else
            {
                Console.WriteLine("Password Salah");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Username tidak ditemukan");
        }
    }

    public void UpdateData()
    {
        Console.Write("Input Username yang ingin diubah: ");
        String username = Console.ReadLine();
        Console.WriteLine();

        foreach (var update in peopleList.Where(x => x.Username == username))
        {
            Console.Write("Input First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Input Last Name: ");
            string lastName = Console.ReadLine();
            Console.Write("Input Password: ");
            String password = Console.ReadLine();
            String fullName = $"{firstName} {lastName}";
            username = firstName.Substring(0, 2) + lastName.Substring(0, 2);

            update.Name = fullName;
            update.Username = username;
            update.Password = password;
        }

        Console.WriteLine();
        Console.WriteLine($"Update data dengan username {username} berhasil");
    }

    private bool PasswordValidation(string password)
    {
        var input = password;

        if (string.IsNullOrWhiteSpace(input))
        {
            //throw new Exception("Password should not be empty");
            Console.WriteLine("Password should not be empty");
            Console.WriteLine();
            return false;
        }

        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMiniMaxChars = new Regex(@".{8,15}");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (!hasLowerChar.IsMatch(input))
        {
            Console.WriteLine("Password should contain At least one lower case letter");
            return false;
        }
        else if (!hasUpperChar.IsMatch(input))
        {
            Console.WriteLine("Password should contain At least one upper case letter");
            return false;
        }
        else if (!hasMiniMaxChars.IsMatch(input))
        {
            Console.WriteLine("Password should not be less than 8 or greater than 15 characters");
            return false;
        }
        else if (!hasNumber.IsMatch(input))
        {
            Console.WriteLine("Password should contain At least one numeric value");
            return false;
        }

        else if (!hasSymbols.IsMatch(input))
        {
            Console.WriteLine("Password should contain At least one special case characters");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool NameValidation(string name)
    {
        var input = name;

        if (string.IsNullOrWhiteSpace(input))
        {
            //throw new Exception();
            Console.WriteLine("Name input should not be empty");
            Console.WriteLine();
            return false;
        }

        var hasMiniMaxChars = new Regex(@".{2,10}");

        if (!hasMiniMaxChars.IsMatch(input))
        {
            Console.WriteLine("Name input should not be less than 2 or greater than 10 characters");
            Console.WriteLine();
            return false;
        }
        else
        {
            return true;
        }
    }
}