using System;
using System.Collections.Generic;
using System.Text;
using TodoListService.Models;

namespace TodoListService.Tests.Common
{
    [Obsolete]//Now it is not required to create database and feed db. I have create the Sqlite db in App_Data folder and using it in unit tests.
    public class FakeDataInitializer
    {
        public FakeDataInitializer()
        {
        }

        public void Seed(TodoListDBContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.User.AddRange(
                new User() { firstName = "Ajay", lastName = "Verma", userId="ajay.verma", emailId="ajay.verma@gmail.com", isActive=1 },
                new User() { firstName = "Lalita", lastName = "Verma", userId = "lalita.verma", emailId = "lalita.verma@gmail.com", isActive = 1 }
                
            );

            context.ToDo.AddRange(
                new Todo() { id = 1, description = "Create DB", userId = "ajay.verma", isCompleted=1, isActive=1 },
                new Todo() { id = 2, description = "Create Angular App", userId = "ajay.verma", isCompleted =1, isActive = 1 },
                new Todo() { id = 3, description = "Create WebApi", userId = "ajay.verma", isCompleted = 0, isActive = 1 },
                new Todo() { id = 4, description = "Create unit tests", userId = "ajay.verma", isCompleted = 0, isActive = 1 },
                new Todo() { id = 5, description = "wake up at 6:30 am", userId = "lalita.verma", isCompleted = 0, isActive = 1 },
                new Todo() { id = 6, description = "go to bed at 9:30pm", userId = "lalita.verma", isCompleted = 0, isActive = 1 }

            );
            context.SaveChanges();
        }
    }
}
