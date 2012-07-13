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

            createPersons(repository);

            createInquiries(repository);
        }

        private void createInquiries(EfRepository repository)
        {
            var clients = repository.Query<Person>(x => x.RelatedMails).
                Where(x => x.Role == PersonRole.Client).ToList();

            var randomClient = new RandomItem<Person>(clients, false);

            var iquiries = Builder<Inquiry>.CreateListOfSize(10)
                .All()
                .With(x => x.Id = 0)
                .With(x => x.Status = GetRandom.Int(0, 3))
                .With(x => x.Client = randomClient.Next())
                .With(x => x.ReferenceDate = GetRandom.DateTime(January.The1st, DateTime.Now))
                .With(x => x.Source = Builder<MailMessage>.CreateNew()
                                          .With(z => z.Date = GetRandom.DateTime(January.The1st, DateTime.Now))
                                          .With(z => z.Subject = GetRandom.Phrase(10))
                                          .With(z => z.Body = GetRandom.Phrase(GetRandom.Int(60, 500)))
                                          .With(z => z.Id = 0)
                                          .Build())
                .Build();

            foreach (var inquiry in iquiries)
            {
                repository.Save(inquiry);
            }
        }

        private void createPersons(EfRepository repository)
        {
            var persons = Builder<Person>.CreateListOfSize(10)
                .All()
                .With(x => x.FirstName = GetRandom.FirstName())
                .With(x => x.LastName = GetRandom.LastName())
                .With(x => x.Role = GetRandom.Int(0, 2))
                .With(x => x.Country = GetRandom.Phrase(10))
                .With(x => x.Email = GetRandom.Email())
                .With(x => x.CreationDate = GetRandom.DateTime(January.The1st, DateTime.Now))
                .With(x => x.Id = 0)
                .With(x=>x.PhotoPath="")
                .With(x => x.RelatedMails = Builder<MailMessage>.CreateListOfSize(5)
                    .All()
                    .With(z => z.Date = GetRandom.DateTime(January.The1st, DateTime.Now))
                    .With(z => z.Subject = GetRandom.Phrase(10))
                    .With(z => z.Body = GetRandom.Phrase(50))
                    .With(z => z.Id = 0)
                    .With(z => z.Sender = x)
                    .With(z => z.Receivers = new List<Person> {x})
                    .Build())
                .Build();

            foreach (var person in persons)
            {
                repository.Save(person);
            }
        }
    }
}