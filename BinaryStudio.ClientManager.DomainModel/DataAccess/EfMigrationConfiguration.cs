using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using FizzWare.NBuilder.Generators;

namespace BinaryStudio.ClientManager.DomainModel.DataAccess
{
    public class EfMigrationConfiguration : DbMigrationsConfiguration<EfDataContext>
    {
        public EfMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
#if DEBUG
            AutomaticMigrationDataLossAllowed = true;
#endif
        }

        protected override void Seed(EfDataContext context)
        {
            var repository = new EfRepository();

            if (!repository.Query<Tag>().Any())
                createTags(repository);
            if (!repository.Query<Person>().Any())
            {
                createPersons(repository);
            }
            var beginOfNowWeek = Clock.Now.GetStartOfBusinessWeek();
            var endOfNowWeek = Clock.Now.GetEndOfBusinessWeek();
            if (!repository.Query<Inquiry>()
                .Any(x => (x.ReferenceDate.Value >= beginOfNowWeek
                    && x.ReferenceDate.Value <= endOfNowWeek)))
                createInquiries(repository);
        }

        private void createTags(EfRepository repository)
        {
            var tags = new List<Tag>
            {
                new Tag {Name = "C++"},
                new Tag {Name = ".Net"},
                new Tag {Name = "Php"}
            };

            foreach (var tag in tags)
            {
                repository.Save(tag);
            }
        }

        private void createInquiries(EfRepository repository)
        {
            var clients = repository.Query<Person>(x => x.RelatedMails)
                .Where(x => x.Role == PersonRole.Client).ToList();
            var randomClient = new RandomItemPicker<Person>(clients, new RandomGenerator());

            var tags = repository.Query<Tag>().ToList();
            var randomTag = new RandomItemPicker<Tag>(tags, new RandomGenerator());

            var iquiries = Builder<Inquiry>.CreateListOfSize(45)
                .All()
                .With(x => x.Id = 0)
                .With(x => x.Client = randomClient.Pick())
                .With(x => x.Subject = "Need " + GetRandom.Int(2, 4) + " " + randomTag.Pick().Name + " developers")
                .With(x => x.Comments = new List<Comment>())
                .With(x => x.ReferenceDate = GetRandom.DateTime(Clock.Now.GetStartOfMonth(),
                    Clock.Now.GetEndOfMonth().AddDays(1)))
                .With(x => x.Source = Builder<MailMessage>.CreateNew()
                    .With(z => z.Date = GetRandom.DateTime(January.The1st, Clock.Now))
                    .With(z => z.Subject = GetRandom.Phrase(10))
                    .With(z => z.Body = GetRandom.Phrase(GetRandom.Int(60, 250)))
                    .With(z => z.Id = 0)
                    .Build())
                .With(x => x.Tags = new List<Tag> { randomTag.Pick() })
                .TheFirst(5)
                .With(x => x.ReferenceDate = GetRandom.DateTime(Clock.Now.GetStartOfBusinessWeek(),
                    Clock.Now.GetEndOfBusinessWeek().AddDays(1)))
                .TheNext(5)
                .With(x => x.ReferenceDate = null)
                .With(x => x.Tags = null)
                .TheNext(7)
                .With(x => x.ReferenceDate = Clock.Now.GetStartOfBusinessWeek())
                .Build();

            foreach (var inquiry in iquiries)
            {
                repository.Save(inquiry);
            }
        }

        private void createPersons(EfRepository repository)
        {
            var twitterUris = new List<string> {"https://twitter.com/1van1111", "https://twitter.com/mnzadornov"};
            var facebookUris = new List<string> { "http://www.facebook.com/ivan.zaporozhchenko", "http://www.facebook.com/dmitriy.stranger.7" };
            var linkedInUris = new List<string> { "http://ua.linkedin.com/in/titarenko", "http://ua.linkedin.com/in/olvia" };
            var countries = new List<string> {"Ukraine", "Poland", "Russia", "England", "USA", "Slovakia", "Finland"};

            var randomTwitterUri = new RandomItemPicker<string>(twitterUris, new RandomGenerator());
            var randomFacebookUri = new RandomItemPicker<string>(facebookUris, new RandomGenerator());
            var randomlinkedInUri = new RandomItemPicker<string>(linkedInUris, new RandomGenerator());
            var randomCountry = new RandomItemPicker<string>(countries, new RandomGenerator());

            var persons = Builder<Person>.CreateListOfSize(15)
                .All()
                .With(x => x.FirstName = GetRandom.FirstName())
                .With(x => x.LastName = GetRandom.LastName())
                .With(x => x.Role = PersonRole.Employee)
                .With(x => x.Country = randomCountry.Pick())
                .With(x => x.Email = x.FullName.Replace(" ", "") + "@example.com")
                .With(x => x.Phone = GetRandom.Usa.PhoneNumber())
                .With(x => x.CreationDate = GetRandom.DateTime(January.The1st, DateTime.Now))
                .With(x => x.Id = 0)
                .With(x => x.PhotoUri = "")
                .With(x => x.FacebookUri = randomFacebookUri.Pick())
                .With(x => x.LinkedInUri = randomlinkedInUri.Pick())
                .With(x => x.TwitterUri = randomTwitterUri.Pick())
                .TheFirst(10)
                .With(x => x.Role = PersonRole.Client)
                .Build();

            var clients = persons.Where(x => x.Role == PersonRole.Client).ToList();
            var employees = persons.Where(x => x.Role == PersonRole.Employee).ToList();

            var randomEmployee = new RandomItemPicker<Person>(employees, new RandomGenerator());

            foreach (var client in clients)
            {
                client.RelatedMails = new List<MailMessage>();

                var date = GetRandom.DateTime(January.The1st, DateTime.Now);

                for (int i = 0; i < 3; i++)
                {
                    var clientMail = Builder<MailMessage>.CreateNew()
                        .With(x => x.Id = 0)
                        .With(x => x.Sender = client)
                        .With(x => x.Receivers = new List<Person> { randomEmployee.Pick() })
                        .With(x => x.Subject = GetRandom.Phrase(10))
                        .With(x => x.Body = GetRandom.Phrase(GetRandom.Int(60, 250)))
                        .With(x => x.Date = date.AddDays(i * 7))
                        .Build();

                    client.RelatedMails.Add(clientMail);

                    var employeeMail = Builder<MailMessage>.CreateNew()
                        .With(x => x.Id = 0)
                        .With(x => x.Sender = randomEmployee.Pick())
                        .With(x => x.Receivers = new List<Person> { client })
                        .With(x => x.Subject = GetRandom.Phrase(10))
                        .With(x => x.Body = GetRandom.Phrase(GetRandom.Int(60, 250)))
                        .With(x => x.Date = date.AddDays(i * 7 + 3))
                        .Build();

                    client.RelatedMails.Add(employeeMail);
                }
            }

            foreach (var person in persons)
            {
                repository.Save(person);
            }
        }
    }
}