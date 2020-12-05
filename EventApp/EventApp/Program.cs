using System;
using System.Collections.Generic;

namespace EventApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //podaci koji mi služe da provjerim funkcionalnosti
            var ivnaBakovic = new Person("Ivna", "Bakovic", 762, 21374);
            var markoMajic = new Person("Marko", "Majic", 561, 12323);
            var anaAnic = new Person("Ana", "Anic", 123, 877164);
            var josipJosipovic = new Person("Josip", "Josipovic", 456, 123456);

            var date1 = new DateTime(2008, 3, 1, 20, 0, 0);
            var date2 = new DateTime(2008, 5, 1, 21, 0, 0);
            var date3 = new DateTime(2020, 1, 1, 0, 0, 0);
            var date4 = new DateTime(2020, 2, 2, 22, 0, 0);
            var eventBachConcert = new Event("Bach concert", date1, date2, EventType.Concert);
            var eventStudyDev = new Event("Study Dev", date3, date4, EventType.StudySession);

            List<Person> eventClassicConcert = new List<Person>
            {
                ivnaBakovic,
                markoMajic,
                anaAnic,
                josipJosipovic
            };
            List<Person> eventStudyDevelopment = new List<Person>
            {
                 ivnaBakovic,
                 markoMajic,
                 anaAnic,
            };

            var allEvents = new Dictionary<Event, List<Person>>
            {
                {
                    eventBachConcert, eventClassicConcert
                },
                {
                    eventStudyDev, eventStudyDevelopment
                },
            };

            bool shouldContinueFirst = true;
            while (shouldContinueFirst)
            {
                int number = DisplayMenu();
                switch (number)
                {
                    case 1:
                        Console.WriteLine("Napišite naziv vašeg eventa:");
                        string name = Console.ReadLine();
                        Console.Write("Unesite datum i vrijeme (e.g. dd/MM/yyy HH:mm:ss): ");
                        DateTime inputStartDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Unesite datum i vrijeme (e.g. dd/MM/yyy HH:mm:ss): ");
                        DateTime inputEndDate = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("Upišite tip eventa u brojevnom obliku (0 - Coffee, 1 - Lecture, 2 - Concert, 3 - StudySession): ");
                        string eventTypeNumber = Console.ReadLine();
                        EventType eventType = (EventType)Convert.ToInt32(eventTypeNumber);

                        if(ValidateEventExistence(allEvents,name))
                        {
                            Console.WriteLine("Već postoji event s danim imenom!");
                            break;
                        }
                        if(!ValidateTimeInterference(allEvents,inputStartDate,inputEndDate))
                        {
                            Console.WriteLine("Ne možemo kreirati event jer postoji vremenska interferencija s prijašnjim eventima!");
                            break;
                        }
                        if(!ValidateTimeOrder(inputStartDate,inputEndDate))
                        {
                            Console.WriteLine("Ne možemo kreirati event jer ne može završiti prije nego što je počeo!");
                            break;
                        }
                        if(Convert.ToInt32(eventTypeNumber) < 0 || Convert.ToInt32(eventTypeNumber) > 3)
                        {
                            Console.WriteLine("Ne možemo kreirati event jer nije dobro odabran event type!");
                            break;
                        }
                        AddEvent(allEvents, name, inputStartDate, inputEndDate, eventType);
                        Console.WriteLine("Event je dodan!");
                        break;
                    case 2:
                        Console.WriteLine("Upišite ime eventa koji želite izbrisati!");
                        name = Console.ReadLine();
                        if (!ValidateEventExistence(allEvents, name))
                        {
                            Console.WriteLine("Ne postoji event s tim imenom!");
                            break;
                        }
                        RemoveEvent(allEvents, name);
                        Console.WriteLine("Event je izbrisan");
                        break;
                    case 3:
                        Console.WriteLine("Upišite ime eventa koji želite editirati!");
                        name = Console.ReadLine();
                        if (!ValidateEventExistence(allEvents, name))
                        {
                            Console.WriteLine("Ne postoji event s tim imenom!");
                            break;
                        }
                        string tempAnswer;
                        Event targetEvent = FindingEventByName(allEvents, name);
                        DateTime primaryEventStartDate = targetEvent.StartTime;
                        DateTime primaryEventEndDate = targetEvent.EndTime;
                        if (AskingQuestions("Želite li promijeniti ime eventa?"))
                        {
                            tempAnswer = Editing("Upišite drugi naziv za vaš event.");
                            if (ValidateEventExistence(allEvents, tempAnswer))
                            {
                                Console.WriteLine("Već postoji event s danim imenom pa Vam ne možemo dopustiti promjenu!");
                            }
                            else
                            {
                                targetEvent.Name = tempAnswer;
                            }
                        };
                        if (AskingQuestions("Želite li promijeniti početak vašeg eventa?"))
                        {
                            tempAnswer = Editing("Promijenite početak vašeg eventa (e.g. dd/MM/yyy HH:mm:ss):");
                            targetEvent.StartTime = DateTime.Parse(tempAnswer);
                        };
                        if (AskingQuestions("Želite li promijeniti kraj vašeg eventa?"))
                        {
                            tempAnswer = Editing("Promijenite kraj vašeg eventa (e.g. dd/MM/yyy HH:mm:ss):");
                            targetEvent.EndTime = DateTime.Parse(tempAnswer);
                            
                        };
                        if (!ValidateTimeInterference(allEvents, targetEvent.StartTime, targetEvent.EndTime) ||
    !ValidateTimeOrder(targetEvent.StartTime, targetEvent.EndTime))
                        {
                            targetEvent.StartTime = primaryEventStartDate;
                            targetEvent.EndTime = primaryEventEndDate;
                            Console.WriteLine("Ne možemo prihvatiti vaše promjene pri unosu početka i kraja eventa jer su interferenciji" +
                                " s ostalim događajima ili nepravilan redoslijed vremena!");
                  
                        }
                        if (AskingQuestions("Želite li promijeniti tip vašeg eventa?"))
                        {
                            tempAnswer = Editing("Promijenite tip vašeg eventa (0 - Coffee, 1 - Lecture, 2 - Concert, 3 - StudySession):");
                            targetEvent.EventTip = (EventType)Convert.ToInt32(tempAnswer);
                        };
                        break;
                    case 4:
                        Console.WriteLine("Upišite ime eventa kojem želite dodati osobu!");
                        name = Console.ReadLine();
                        if (!ValidateEventExistence(allEvents, name))
                        {
                            Console.WriteLine("Ne postoji event s tim imenom!");
                            break;
                        }
                        Console.WriteLine("Upišite ime osobe koju dodajete: ");
                        string firstName = Console.ReadLine();
                        Console.WriteLine("Upišite prezime osobe koju dodajete: ");
                        string lastName = Console.ReadLine();
                        Console.WriteLine("Upišite OIB osobe koju dodajete: ");
                        int OIB = int.Parse(Console.ReadLine());
                        Console.WriteLine("Upišite broj mobitela osobe koju dodajete: ");
                        int phoneNumber = int.Parse(Console.ReadLine());
                        if(!ValidateGuestOnEvent(allEvents,name,OIB))
                        {
                            Console.WriteLine("Osoba koju ste unijeli je već na popisu.");
                            break;
                        }
                        AddPersonToEvent(allEvents, name, firstName, lastName, OIB, phoneNumber);
                        break;
                    case 5:
                        Console.WriteLine("Upišite ime eventa s kojeg želite ukloniti osobu!");
                        name = Console.ReadLine();
                        if (!ValidateEventExistence(allEvents, name))
                        {
                            Console.WriteLine("Ne postoji event s tim imenom!");
                            break;
                        }
                        Console.WriteLine("Upišite OIB osobe koju želite izbrisati: ");
                        OIB = int.Parse(Console.ReadLine());
                        if (ValidateGuestOnEvent(allEvents, name, OIB))
                        {
                            Console.WriteLine("Osoba koju ste unijeli nije na popisu pa je i ne možete izbrisati.");
                            break;
                        }
                        RemovePersonFromEvent(allEvents, name, OIB);
                        break;
                    case 6:
                        Console.WriteLine("Ušli ste u novi izbornik:");
                        bool shouldContinueSecond = true;
                        while (shouldContinueSecond)
                        {
                            number = DisplaySmallMenu();
                            switch (number)
                            {
                                case 1:
                                    Console.WriteLine("Upišite ime eventa o kojem želite informacije!");
                                    name = Console.ReadLine();
                                    if (!ValidateEventExistence(allEvents, name))
                                    {
                                        Console.WriteLine("Ne postoji event s tim imenom!");
                                        break;
                                    }
                                    DisplayEventInfo(allEvents, name);
                                    break;
                                case 2:
                                    Console.WriteLine("Upišite ime eventa te ćete dobiti popis gostiju!");
                                    name = Console.ReadLine();
                                    if (!ValidateEventExistence(allEvents, name))
                                    {
                                        Console.WriteLine("Ne postoji event s tim imenom!");
                                        break;
                                    }
                                    DisplayEventGuests(allEvents, name);
                                    break;
                                case 3:
                                    Console.WriteLine("Upišite ime eventa te ćete dobiti informacije i popis gostiju!");
                                    name = Console.ReadLine();
                                    if (!ValidateEventExistence(allEvents, name))
                                    {
                                        Console.WriteLine("Ne postoji event s tim imenom!");
                                        break;
                                    }
                                    DisplayEventInfo(allEvents, name);
                                    DisplayEventGuests(allEvents, name);
                                    break;
                                case 4:
                                    Console.WriteLine("Izašli ste iz podmenija!");
                                    shouldContinueSecond = false;
                                    break;
                                default:
                                    Console.WriteLine("krivi unos");
                                    break;
                            }
                        }
                        break;
                        
                    case 7:
                        Console.WriteLine("Izlaz iz aplikacije");
                        shouldContinueFirst = false;
                        break;
                    default:
                        Console.WriteLine("Neispravan unos broja");
                        break;

                }
            }
        }

        public static bool ValidateGuestOnEvent(Dictionary<Event, List<Person>> dict, string name, int OIB)
        {
            Event targetEvent = FindingEventByName(dict, name);
            foreach(var item in dict)
            {
                for(var i = 0; i < item.Value.Count; i++)
                {
                    if (item.Value[i].OIB == OIB)
                        return false;
                }
            }
            return true;
        }
        public static void DisplayEventInfo(Dictionary<Event, List<Person>> dict, string name)
        {
            Console.WriteLine("name – event type – start time – end time – trajanje – ispis broja ljudi na eventu");
            Event eventTarget = FindingEventByName(dict, name);
            Console.WriteLine(eventTarget.Name + " - " + eventTarget.EventTip + " - " + eventTarget.StartTime + " - " + eventTarget.EndTime + " - " + dict[eventTarget].Count);   
        }
        public static void DisplayEventGuests(Dictionary<Event, List<Person>> dict, string name)
        {
            Event targetEvent = FindingEventByName(dict, name);
            if (dict[targetEvent].Count == 0)
            {
                Console.WriteLine("Nema gostiju na eventu.");
                return;
            }
            Console.WriteLine("Gosti na događaju " + name + ":");
            Console.WriteLine("[Redni broj u listi].name – last name – broj mobitela");
            for (var i = 0; i < dict[targetEvent].Count; i++)
            {
                Console.WriteLine(i + "." + dict[targetEvent][i].FirstName + " - " + dict[targetEvent][i].LastName + " - " + dict[targetEvent][i].PhoneNumber);
            }
        }
        public static bool ValidateTimeOrder(DateTime startDate, DateTime endDate)
        {
            if(startDate<endDate)
            {
                return true;
            }
            return false;
        }
        public static bool ValidateTimeInterference(Dictionary<Event, List<Person>> dict, DateTime startDate, DateTime endDate)
        {
            foreach(var item in dict)
            {
                if((item.Key.StartTime<startDate && startDate<item.Key.EndTime) || (item.Key.StartTime < endDate && endDate < item.Key.EndTime))
                {
                    return false;
                }
            }
            return true;
        }
        public static int DisplayMenu()
        {
            Console.WriteLine("\n----IZBORNIK----");
            Console.WriteLine("Upišite odgovarajući broj za željenu akciju!");
            Console.WriteLine("1 - Dodajte event");
            Console.WriteLine("2 - Izbrišite odgovarajući event");
            Console.WriteLine("3 - Editirajte odgovarajući event");
            Console.WriteLine("4 - Dodajte novu osobu na odgovarajući event");
            Console.WriteLine("5 - Uklonite osobu sa odgovarajućeg eventa");
            Console.WriteLine("6 - Ispis detalja eventa");
            Console.WriteLine("7 - Izlaz iz aplikacije");
            Console.WriteLine("\n\n");
            int x = int.Parse(Console.ReadLine());
            return x;
        }
        public static int DisplaySmallMenu()
        {
            Console.WriteLine("\n----IZBORNIK----");
            Console.WriteLine("Upišite odgovarajući broj za željenu akciju!");
            Console.WriteLine("1 - Ispis detalja eventa");
            Console.WriteLine("2 - Ispis svih osoba na eventu");
            Console.WriteLine("3 - Ispis detalja eventa i svih osoba na eventu");
            Console.WriteLine("4 - Izlazak iz podmenija");
            Console.WriteLine("\n\n");
            int x = int.Parse(Console.ReadLine());
            return x;
        }
        public static bool AskingQuestions(string question)
        {
            Console.WriteLine(question +" da/ne");
            string answer = Console.ReadLine();
            if (answer == "da")
                return true;
            else if (answer == "ne")
                return false;
            else
            {
                Console.WriteLine("Neispravan unos!");
                AskingQuestions(question);
                return false;
            }    
        }
        public static Event FindingEventByName(Dictionary<Event, List<Person>> dict, string name)
        {
            var startDate = new DateTime();
            var endDate = new DateTime();
            Event targetEvent = new Event("", startDate, endDate, EventType.Coffee);
            foreach (var item in dict)
            {
                if (item.Key.Name == name)
                {
                    targetEvent = item.Key;
                }
            }
            return targetEvent;
        }
        public static string Editing(string question_input)
        {
            Console.WriteLine(question_input);
            string inputAnswer = Console.ReadLine();
            return inputAnswer;
        }
        public static bool ValidateEventExistence(Dictionary<Event, List<Person>> dict, string name)
        {
            foreach(var item in dict)
            {
                if (item.Key.Name == name)
                    return true;
            }
            return false;
        }
        public static void AddEvent(Dictionary<Event,List<Person>> dict, string name, DateTime startTime, DateTime endTime, EventType eventTip)
        {
            var newEvent = new Event(name, startTime, endTime, eventTip);
            List<Person> newListEvent = new List<Person>();
            dict.Add(newEvent, newListEvent);
        }
        public static void RemoveEvent(Dictionary<Event, List<Person>> dict, string name)
        {
            var startDate = new DateTime();
            var endDate = new DateTime();
            Event removedItem = new Event("", startDate, endDate, EventType.Coffee);
            foreach (var item in dict)
            {
                if (item.Key.Name == name)
                {
                    removedItem = item.Key;
                }
            }
            dict.Remove(removedItem);
        }
        public static void AddPersonToEvent(Dictionary<Event, List<Person>> dict, string nameEvent, string firstName, string lastName, int OIB, int phoneNumber)
        {
            Person addedPerson = new Person(firstName, lastName, OIB, phoneNumber);
            Event targetEvent = FindingEventByName(dict,nameEvent);
            dict[targetEvent].Add(addedPerson);
            Console.WriteLine("Osoba je dodana!");
        }
        public static void RemovePersonFromEvent(Dictionary<Event, List<Person>> dict, string nameEvent, int OIB)
        {
            Event targetEvent = FindingEventByName(dict, nameEvent);
            Person removedPerson = new Person("", "", OIB, 0);
            for(var i = 0; i < dict[targetEvent].Count; i++)
            {
                if(dict[targetEvent][i].OIB==OIB)
                {
                    removedPerson = dict[targetEvent][i];
                }
            }
            dict[targetEvent].Remove(removedPerson);
            Console.WriteLine("Osoba je izbrisana!");
        }
    }
}
