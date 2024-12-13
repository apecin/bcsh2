using LiteDB;
using sportoviste_sem_bcsh2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sportoviste_sem_bcsh2.Services
{
    // Třída pro práci s databází LiteDB, implementuje CRUD operace pro modely
    public class LiteDbService
    {
        private readonly LiteDatabase _database;

        public LiteDbService()
        {
            _database = new LiteDatabase(@"sportoviste_data.db");
        }

        // Kolekce pro každou entitu
        public ILiteCollection<Sportoviste> Sportovistes => _database.GetCollection<Sportoviste>("sportoviste");
        public ILiteCollection<Hriste> Hristes => _database.GetCollection<Hriste>("hriste");
        public ILiteCollection<Rezervace> Rezervaces => _database.GetCollection<Rezervace>("rezervace");
        public ILiteCollection<Uzivatel> Uzivatele => _database.GetCollection<Uzivatel>("uzivatele");

        // CRUD operace pro Sportoviste
        public IEnumerable<Sportoviste> GetAllSportoviste()
        {
            var sportovisteList = Sportovistes.FindAll().ToList();
            Console.WriteLine($"Načteno {sportovisteList.Count} sportovišť");
            foreach (var sportoviste in sportovisteList)
            {
                Console.WriteLine($"Sportoviště: Název = {sportoviste.Nazev}, Id = {sportoviste.Id}");
            }
            return sportovisteList;
        }

        // Metoda pro načtení sportoviště podle Id
        public Sportoviste? GetSportovisteById(int id) => Sportovistes.FindById(id);

        // Metoda pro vložení sportoviště
        public void InsertSportoviste(Sportoviste sportoviste)
        {
            if (sportoviste.Id == 0)
            {
                // Použití Count pro ověření, zda kolekce obsahuje nějaké záznamy
                sportoviste.Id = Sportovistes.Count() > 0 ? Sportovistes.Max(x => x.Id) + 1 : 1;
            }

            // Inicializace prázdného seznamu, pokud je Hriste null
            sportoviste.Hriste ??= new List<Hriste>();

            // Vložení sportoviště do kolekce sportovišť
            Console.WriteLine($"Vkládám sportoviště: Název = {sportoviste.Nazev}, Id = {sportoviste.Id}");
            Sportovistes.Insert(sportoviste);
        }


        // Metoda pro aktualizaci sportoviště
        public void UpdateSportoviste(Sportoviste sportoviste) => Sportovistes.Update(sportoviste);

        // Metoda pro smazání sportoviště
        public void DeleteSportoviste(int id) => Sportovistes.Delete(id);

        // CRUD operace pro Hriste
        public IEnumerable<Hriste> GetAllHriste()
        {
            var hristes = Hristes.FindAll().ToList();
            foreach (var hriste in hristes)
            {
                hriste.Sportoviste = Sportovistes.FindById(hriste.SportovisteId);
            }
            return hristes;
        }


        // Metoda pro načtení hřiště podle Id
        public Hriste? GetHristeById(int id)
        {
            // Načtení hřiště
            var hriste = Hristes.FindById(id);
            if (hriste != null)
            {
                hriste.Sportoviste = Sportovistes.FindById(hriste.SportovisteId); // Načtení příslušného sportoviště
            }
            return hriste;
        }
        // Metoda pro vložení hřiště
        public void InsertHriste(Hriste hriste)
        {
            // Nastavení Id, pokud je 0
            if (hriste.Id == 0)
            {
                hriste.Id = Hristes.Count() > 0 ? Hristes.Max(x => x.Id) + 1 : 1;
            }

            // Uložit hřiště do kolekce hřišť
            Hristes.Insert(hriste);

            // Přidání hřiště do příslušného sportoviště
            var sportoviste = Sportovistes.FindById(hriste.SportovisteId);
            if (sportoviste != null)
            {
                sportoviste.Hriste.Add(hriste);
                Sportovistes.Update(sportoviste);
            }
        }

        // Metoda pro aktualizaci hřiště
        public void UpdateHriste(Hriste hriste) => Hristes.Update(hriste);

        // Metoda pro smazání hřiště
        public void DeleteHriste(int id) => Hristes.Delete(id);

        // CRUD operace pro Rezervace
        public IEnumerable<Rezervace> GetAllRezervace() => Rezervaces.FindAll();


        public void InsertRezervace(Rezervace rezervace)
        {
            if (rezervace.Id == 0)
            {
                // Bezpečná kontrola prázdné kolekce
                rezervace.Id = Rezervaces.Count() > 0 ? Rezervaces.Max(x => x.Id) + 1 : 1;
            }

            Console.WriteLine($"Vkládám rezervaci: Hřiště = {rezervace.Hriste}, Klient = {rezervace.Klient}, Id = {rezervace.Id}");
            Rezervaces.Insert(rezervace);
        }

        public void UpdateRezervace(Rezervace rezervace) => Rezervaces.Update(rezervace);

        public void DeleteRezervace(int id) => Rezervaces.Delete(id);

        public Rezervace? GetRezervaceById(int id)
        {
            var rezervace = Rezervaces.FindById(id);
            if (rezervace != null)
            {
                rezervace.Hriste = Hristes.FindById(rezervace.HristeId);
            }
            return rezervace;
        }


        // CRUD operace pro Uzivatel
        public IEnumerable<Uzivatel> GetAllUzivatele() => Uzivatele.FindAll();

        public Uzivatel? GetUzivatelById(int id) => Uzivatele.FindById(id);

        public void InsertUzivatel(Uzivatel uzivatel)
        {
            if (uzivatel.Id == 0)
            {
                // Zkontroluje, zda kolekce obsahuje záznamy, jinak nastaví Id na 1
                uzivatel.Id = Uzivatele.Count() > 0 ? Uzivatele.Max(x => x.Id) + 1 : 1;
            }

            Console.WriteLine($"Vkládám uživatele: Jméno = {uzivatel.Jmeno}, Id = {uzivatel.Id}");
            Uzivatele.Insert(uzivatel);
        }


        public void UpdateUzivatel(Uzivatel uzivatel) => Uzivatele.Update(uzivatel);

        public void DeleteUzivatel(int id) => Uzivatele.Delete(id);
    }
}
