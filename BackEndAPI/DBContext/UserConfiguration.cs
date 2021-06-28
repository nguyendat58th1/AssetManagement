using System;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEndAPI.DBContext
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.ToTable("User");

            builder.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(e => e.DateOfBirth)
                    .IsRequired();

            builder.Property(e => e.JoinedDate)
                    .IsRequired();

            builder.Property(e => e.Gender)
                    .IsRequired();

            builder.Property(e => e.Type)
                    .IsRequired();

            builder.Property(e => e.Location)
                    .IsRequired();

            builder.Property(e => e.Status)
                    .IsRequired();

            builder.Property(e => e.OnFirstLogin)
                    .IsRequired()
                    .HasDefaultValue(OnFirstLogin.Yes);

            builder.HasData(
                    new User
                    {
                        Id = 1,
                        StaffCode = "SD0001",
                        FirstName = "Binh",
                        LastName = "Nguyen Van",
                        DateOfBirth = new DateTime(1993, 01, 20),
                        JoinedDate = new DateTime(2021, 12, 05),
                        Gender = Gender.Male,
                        Type = UserType.Admin,
                        UserName = "binhnv",
                        Password = "binhnv@20011993",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = 2,
                        StaffCode = "SD0002",
                        FirstName = "Binh",
                        LastName = "Nguyen Thi",
                        DateOfBirth = new DateTime(1994, 01, 12).Date,
                        JoinedDate = new DateTime(2021, 12, 05).Date,
                        Gender = Gender.Female,
                        Type = UserType.User,
                        UserName = "binhnt",
                        Password = "binhnt@12011994",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = 3,
                        StaffCode = "SD0003",
                        FirstName = "Binh",
                        LastName = "Nguyen Thi",
                        DateOfBirth = new DateTime(1997, 01, 12).Date,
                        JoinedDate = new DateTime(2019, 12, 05).Date,
                        Gender = Gender.Female,
                        Type = UserType.User,
                        UserName = "binhnt2",
                        Password = "binhnt2@12011997",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = 4,
                        StaffCode = "SD0004",
                        FirstName = "Anh",
                        LastName = "Nguyen Duc",
                        DateOfBirth = new DateTime(2000, 01, 20),
                        JoinedDate = new DateTime(2018, 09, 25),
                        Gender = Gender.Female,
                        Type = UserType.Admin,
                        UserName = "anhnd",
                        Password = "anhnd@20012000",
                        Location = Location.HoChiMinh,
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = 5,
                        StaffCode = "SD0005",
                        FirstName = "Van",
                        LastName = "Nguyen Thi",
                        DateOfBirth = new DateTime(1990, 01, 12).Date,
                        JoinedDate = new DateTime(2021, 12, 05).Date,
                        Gender = Gender.Female,
                        Type = UserType.User,
                        UserName = "binhnt",
                        Password = "binhnt@12011990",
                        Location = Location.HoChiMinh,
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = 6,
                        StaffCode = "SD0006",
                        FirstName = "Binh",
                        LastName = "Nguyen Thi",
                        DateOfBirth = new DateTime(1987, 01, 12).Date,
                        JoinedDate = new DateTime(2019, 12, 05).Date,
                        Gender = Gender.Male,
                        Type = UserType.User,
                        UserName = "binhnt2",
                        Password = "binhnt2@120187",
                        Location = Location.HoChiMinh,
                        Status = UserStatus.Active
                    }
            );
        }
    }
}