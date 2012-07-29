﻿using System;
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

            createTags(repository);

            createPersons(repository);

            createInquiries(repository);
        }

        private void createTags(EfRepository repository)
        {
            if (!repository.Query<Tag>().Any())
            {
                repository.Save(new Tag
                                    {
                                        Name = "C++"
                                    });
                repository.Save(new Tag
                                    {
                                        Name = ".Net"
                                    });
            }
        }

        private void createInquiries(EfRepository repository)
        {
            var clients = repository.Query<Person>(x => x.RelatedMails)
                .Where(x => x.Role == PersonRole.Client).ToList();
            var randomClient = new RandomItemPicker<Person>(clients, new RandomGenerator());

            var tags = repository.Query<Tag>().ToList();
            var randomTag = new RandomItemPicker<Tag>(tags, new RandomGenerator());

            var iquiries = Builder<Inquiry>.CreateListOfSize(30)
                .All()
                .With(x => x.Id = 0)
                .With(x => x.Client = randomClient.Pick())
                .With(x => x.Comments = new List<Comment>())
                .With(x => x.ReferenceDate = GetRandom.DateTime(Clock.Now.GetStartOfMonth(),
                    Clock.Now.GetEndOfMonth().AddDays(1)))
                .With(x => x.Source = Builder<MailMessage>.CreateNew()
                                          .With(z => z.Date = GetRandom.DateTime(January.The1st, Clock.Now))
                                          .With(z => z.Subject = GetRandom.Phrase(10))
                                          .With(z => z.Body = GetRandom.Phrase(GetRandom.Int(60, 500)))
                                          .With(z => z.Id = 0)
                                          .Build())
                .With(x => x.Tags = new List<Tag> { randomTag.Pick() })
                .TheFirst(5)
                .With(x => x.ReferenceDate = GetRandom.DateTime(Clock.Now.GetStartOfBusinessWeek(),
                    Clock.Now.GetEndOfBusinessWeek().AddDays(1)))
                .TheNext(5)
                .With(x => x.ReferenceDate = null)
                .With(x => x.Tags = null)
                .TheNext(5)
                .With(x=>x.ReferenceDate=Clock.Now.GetEndOfBusinessWeek())
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

            var randomTwitterUri = new RandomItemPicker<string>(twitterUris, new RandomGenerator());
            var randomFacebookUri = new RandomItemPicker<string>(facebookUris, new RandomGenerator());
            var randomlinkedInUri = new RandomItemPicker<string>(linkedInUris, new RandomGenerator());

            var persons = Builder<Person>.CreateListOfSize(15)
                .All()
                .With(x => x.FirstName = GetRandom.FirstName())
                .With(x => x.LastName = GetRandom.LastName())
                .With(x => x.Role = PersonRole.Employee)
                .With(x => x.Country = GetRandom.Phrase(10))
                .With(x => x.Email = x.FullName.Replace(" ", "") + "@example.com")
                .With(x => x.Phone = GetRandom.Usa.PhoneNumber())
                .With(x => x.CreationDate = GetRandom.DateTime(January.The1st, DateTime.Now))
                .With(x => x.Id = 0)
                .With(x=>x.PhotoUri="")
                .With(x=>x.FacebookUri=randomFacebookUri.Pick())
                .With(x=>x.LinkedInUri=randomlinkedInUri.Pick())
                .With(x=>x.TwitterUri=randomTwitterUri.Pick())
                .With(x => x.RelatedMails = Builder<MailMessage>.CreateListOfSize(5)
                    .All()
                    .With(z => z.Date = GetRandom.DateTime(January.The1st, DateTime.Now))
                    .With(z => z.Subject = GetRandom.Phrase(10))
                    .With(z => z.Body = GetRandom.Phrase(50))
                    .With(z => z.Id = 0)
                    .With(z => z.Sender = x)
                    .With(z => z.Receivers = new List<Person> {x})
                    .Build())
                .TheFirst(10)
                .With(x=>x.Role=PersonRole.Client)
                .Build();

            foreach (var person in persons)
            {
                repository.Save(person);
            }
        }
    }
}